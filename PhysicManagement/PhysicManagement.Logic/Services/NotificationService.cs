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

        private void SendSMSAlarm(long? medicalRecordId, Enums.AlarmEventType alarmEventType)
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

        private void SendSMSAlarm(int? patientId, Enums.AlarmEventType alarmEventType)
        {
            if (patientId == null)
                return;

            var correspondentMedicalRecord = MedicalRecordService.GetMedicalRecordByPatientId((int)patientId).LastOrDefault();
            if (correspondentMedicalRecord == null)
                return;

            var doctorId = (int)correspondentMedicalRecord.DoctorId;
            var doctor = new DoctorService().GetDoctorById(doctorId);


            var alarmConfig = alarmService.GetAlarmConfigByEventTypeId(alarmEventType);

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

        public void AddAlarm(Enums.AlarmEventType alarmEventType, long medicalRecordId, bool NeedSendToDoctor, bool NeedSendToResident, bool NeedSendToPhysist)
        {
            var eventTypeObj = alarmService.GetAlarmEventTypeById((int)alarmEventType);
            var medicalRecord = MedicalRecordService.GetMedicalRecordById(medicalRecordId);
            var alarmTitle = eventTypeObj.Title + " " + medicalRecord.PatientFirstName + " " + medicalRecord.PatientLastName + " " + Common.DateUtility.GetPersianDate(DateTime.Now);
            var alarmBody = alarmTitle + '\n' + " پزشک معالج" + medicalRecord.DoctorLastName + " نوع بیماری" + medicalRecord.CancerTitle;

            if (NeedSendToDoctor)
            {
                alarmService.AddAlarm(new Alarm()
                {
                    AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                    DoctorId = medicalRecord.DoctorId,
                    Title = alarmTitle,
                    Body = alarmBody,
                    IsDelivered = false,
                    SendDate = DateTime.Now
                });
            }
            if (NeedSendToResident)
            {
                var residentList = residentService.GetResidentsByDoctorId((int)medicalRecord.DoctorId);

                foreach (var item in residentList)
                {
                    alarmService.AddAlarm(new Alarm()
                    {
                        AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                        ResidentId = item.Id,
                        Title = alarmTitle,
                        Body = alarmBody,
                        IsDelivered = false,
                        SendDate = DateTime.Now
                    });
                }
            }
            if (NeedSendToPhysist)
            {
                var physictList = physicUserService.GetPhysicUsers();
                foreach (var item in physictList)
                {
                    alarmService.AddAlarm(new Alarm()
                    {
                        AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                        PhysicUserId = item.Id,
                        Title = alarmTitle,
                        Body = alarmBody,
                        IsDelivered = false,
                        SendDate = DateTime.Now
                    });
                }
            }


        }

        /// <summary>
        /// الآن وضعیت ارسال آلارم به این صورت است که وقتی یک پرونده ارائه شود متناسب با مرحله ای که بیمار در آن قرار گرفته است و پارامترهایی که به این متد ارسال می شود آلارم برای پزشکان، کارشناسان بخش فیزیک و رزیدنت های مرتبط با پزشک پرونده ارسال می شود
        /// </summary>
        /// <param name="alarmEventType">از جدول AlarmEventType</param>
        /// <param name="patientId">کد بیمار در بانک اطلاعاتی یا همان آیدی بیمار</param>
        /// <param name="NeedSendToDoctor">لازم است به دکتر پرونده آلارم داده شود؟</param>
        /// <param name="NeedSendToResident">لازم است به رزیدنت های پزشک مربوطه آلارم ارسال شود؟</param>
        /// <param name="NeedSendToPhysist">آیا به کل کارشناسان فیزیک لازم است آلارم ارسال شود؟</param>
        public void AddAlarm(Enums.AlarmEventType alarmEventType, int patientId, bool NeedSendToDoctor, bool NeedSendToResident, bool NeedSendToPhysist)
        {
            var eventTypeObj = alarmService.GetAlarmEventTypeById((int)alarmEventType);
            var medicalRecord = MedicalRecordService.GetMedicalRecordByPatientId(patientId).Last();
            var alarmTitle = eventTypeObj.Title + " - " + medicalRecord.PatientFirstName + " " + medicalRecord.PatientLastName + " " + Common.DateUtility.GetPersianDate(DateTime.Now);
            var alarmBody = alarmTitle + '\n' + " پزشک معالج: " + medicalRecord.DoctorLastName + (string.IsNullOrEmpty(medicalRecord.CancerTitle) ? " " : " نوع بیماری" + medicalRecord.CancerTitle);

            if (NeedSendToDoctor)
            {
                alarmService.AddAlarm(new Alarm()
                {
                    AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                    DoctorId = medicalRecord.DoctorId,
                    Title = alarmTitle,
                    Body = alarmBody,
                    IsDelivered = false,
                    IsSMS = false,
                    PatientFullName = medicalRecord.PatientFirstName + " " + medicalRecord.PatientLastName,
                    TreatmentProcessId = medicalRecord.TreatmentProcessId,
                    SendDate = DateTime.Now
                }); 
            }

            if (NeedSendToResident)
            {
                var residentList = residentService.GetResidentsByDoctorId((int)medicalRecord.DoctorId);

                foreach (var item in residentList)
                {
                    alarmService.AddAlarm(new Alarm()
                    {
                        AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                        ResidentId = item.Id,
                        Title = alarmTitle,
                        Body = alarmBody,
                        IsDelivered = false,
                        IsSMS = false,
                        PatientFullName = medicalRecord.PatientFirstName + " " + medicalRecord.PatientLastName,
                        TreatmentProcessId = medicalRecord.TreatmentProcessId,
                        SendDate = DateTime.Now
                    }); 
                }
            }
            if (NeedSendToPhysist)
            {
                var physictList = physicUserService.GetPhysicUsers();
                foreach (var item in physictList)
                {
                    alarmService.AddAlarm(new Alarm()
                    {
                        AlarmEventTypeId = Convert.ToInt32(alarmEventType),
                        PhysicUserId = item.Id,
                        Title = alarmTitle,
                        Body = alarmBody,
                        IsDelivered = false,
                        IsSMS = false,
                        PatientFullName = medicalRecord.PatientFirstName + " " + medicalRecord.PatientLastName,
                        TreatmentProcessId = medicalRecord.TreatmentProcessId,
                        SendDate = DateTime.Now
                    });
                }
            }


            //SMS Part
            SendSMSAlarm((int)patientId, alarmEventType);

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
