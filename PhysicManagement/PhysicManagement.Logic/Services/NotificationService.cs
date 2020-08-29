using PhysicManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class NotificationService
    {
        AlarmService alarmService;
        MedicalRecordService MedicalRecordService;
        PhysicUserService physicUserService;
        ResidentService residentService;

        public NotificationService()
        {
            alarmService = new AlarmService();
            MedicalRecordService = new MedicalRecordService();
            physicUserService = new PhysicUserService();
            residentService = new ResidentService();
        }


        #region SMSandNotification

        public void SendSMSAlarm(long? medicalRecordId, Enums.AlarmEventType alarmEventType)
        {
            if (medicalRecordId == null)
                return;

            var correspondentMedicalRecord = MedicalRecordService.GetMedicalRecordById((long)medicalRecordId);
            if (correspondentMedicalRecord == null)
                return;

            var doctorId = correspondentMedicalRecord.DoctorId;
            var alarmConfig = alarmService.GetAlarmConfigByEventTypeId(alarmEventType);
            var hasDoctorSMS = alarmConfig.SendDoctorSMS;
            var hasPhysistSMS = alarmConfig.SendPhysictSMS;
            var hasAdminSMS = alarmConfig.SendAdminSMS;
            var hasResidentSMS = alarmConfig.SendResidentSMS;

        }

        public void SendSMSAlarm(int? patientId, Enums.AlarmEventType alarmEventType)
        {
            if (patientId == null)
                return;

            var correspondentMedicalRecord = MedicalRecordService.GetMedicalRecordByPatientId((int)patientId).LastOrDefault();
            if (correspondentMedicalRecord == null)
                return;

            var doctorId = (int)correspondentMedicalRecord.DoctorId;
            var doctor = new DoctorService().GetDoctorById(doctorId);
            var eventTypeObj = alarmService.GetAlarmEventTypeById((int)alarmEventType);

            var alarmConfig = alarmService.GetAlarmConfigByEventTypeId(alarmEventType);

            alarmService.AddAlarm(new Alarm()
            {
                AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                DoctorId = doctorId,
                Title = eventTypeObj.Title,
                IsDelivered =false,
                SendDate = DateTime.Now
                
            });

            var hasDoctorSMS = alarmConfig.SendDoctorSMS;
            if ((bool)hasDoctorSMS)
            {
                var alarmRecord = alarmService.GetDoctorAlarmByDoctorId(doctorId);
                if (CheckUserCanRecieveSMS(doctor))
                {
                    //Send
                    SMSWebService.SendSMS(doctor.Mobile, "بیمار با نام " + correspondentMedicalRecord.PatientFirstName + " " + correspondentMedicalRecord.PatientLastName + " پذیرش شد" + " " + "تاریخ پذیرش " + Common.DateUtility.GetPersianDate(DateTime.Now));
                }
            }


            var hasPhysistSMS = alarmConfig.SendPhysictSMS;
            if ((bool)hasPhysistSMS)
            {
                List<PhysicUserAlarm> alarmRecord = alarmService.GetPhysicUserAlarmList();
                List<PhysicUser> physicUserList = physicUserService.GetPhysicUsers();

                foreach (var user in physicUserList)
                {
                    if (CheckUserCanRecieveSMS(user))
                    {
                        //Send
                        SMSWebService.SendSMS(user.Mobile, "بیمار با نام " + correspondentMedicalRecord.PatientFirstName + " " + correspondentMedicalRecord.PatientLastName + " پذیرش شد" + " " + "تاریخ پذیرش " + Common.DateUtility.GetPersianDate(DateTime.Now));
                    }
                }
            }


            var hasAdminSMS = alarmConfig.SendAdminSMS;
            var hasResidentSMS = alarmConfig.SendResidentSMS;
            if ((bool)hasResidentSMS)
            {
                List<ResidentAlarm> alarmRecord = alarmService.GetResidentAlarmList();
                List<Resident> residentList = residentService.GetResidentsByDoctorId(doctorId);

                foreach (var user in residentList)
                {
                    if (CheckUserCanRecieveSMS(user))
                    {
                        //Send
                        SMSWebService.SendSMS(user.Mobile, "بیمار با نام " + correspondentMedicalRecord.PatientFirstName + " " + correspondentMedicalRecord.PatientLastName + " پذیرش شد" + " " + "تاریخ پذیرش " + Common.DateUtility.GetPersianDate(DateTime.Now));
                    }
                }
            }

        }

        private bool CheckUserCanRecieveSMS(Doctor doctor)
        {
            var alarmRecord = alarmService.GetDoctorAlarmByDoctorId(doctor.Id);
            if (alarmRecord == null)
            {
                return true;
            }
            else
            {
                if (alarmRecord.IsSmsRecieveActive == true)
                {
                    return true;
                }
                return false;
            }
        }

        private bool CheckUserCanRecieveSMS(PhysicUser physicUser)
        {
            var alarmRecord = alarmService.GetPhysicUserAlarmByPhysicUserId(physicUser.Id);
            if (alarmRecord == null)
            {
                return true;
            }
            else
            {
                if (alarmRecord.IsSmsRecieveActive == true)
                {
                    return true;
                }
                return false;
            }
        }

        private bool CheckUserCanRecieveSMS(Resident resident)
        {
            var alarmRecord = alarmService.GetResidentAlarmByResidentId(resident.Id);
            if (alarmRecord == null)
            {
                return true;
            }
            else
            {
                if (alarmRecord.IsSmsRecieveActive == true)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}
