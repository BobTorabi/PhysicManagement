using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class MedicalRecordService
    {
        #region MedicalRecord section

        public List<Model.MedicalRecord> GetMedicalRecordList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.MedicalRecord.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.MedicalRecord GetMedicalRecordById(long entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entityId);
                return Entity;
            }
        }
        public int GetTotalMedicalRecordsByDoctorId(int doctorId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Count(e => e.DoctorId == doctorId);
                return Entity;
            }
        }
        public int GetTotalMedicalRecordsByCancerId(int cancerId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Count(e => e.CancerId == cancerId);
                return Entity;
            }
        }
        public int GetTotalMedicalRecordsByTreatmentDeviceId(int treatmentDeviceId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Count(e => e.TreatmentDeviceId == treatmentDeviceId);
                return Entity;
            }
        }
        public List<Model.MedicalRecord> GetMedicalRecordByPatientId(int patientId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Where(x => x.PatientId == patientId).ToList();
                return Entity;
            }
        }
        public bool AddMedicalRecord(Model.MedicalRecord entity)
        {
            var valition = new MedicalRecordValidation.MedicalRecordEntityValidation().Validate(entity);
            if (!valition.IsValid)
                throw new ValidationException(valition.Errors);


            using (var db = new Model.PhysicManagementEntities())
            {
                entity.SystemCode = GetSystemCodeToRegister();
                entity.ReceptionDate = DateTime.Now;
                db.MedicalRecord.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateMedicalRecord(Model.MedicalRecord entity)
        {
            var validtion = new MedicalRecordValidation.MedicalRecordEntityValidation().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entity.Id);
                Entity.PatientFirstName = entity.PatientFirstName;
                Entity.PatientLastName = entity.PatientLastName;
                Entity.DoctorFirstName = entity.DoctorFirstName;
                Entity.DoctorLastName = entity.DoctorLastName;
                Entity.CTCode = entity.CTCode;
                Entity.CTDescription = entity.CTDescription;
                Entity.TPCode = entity.TPCode;
                Entity.TPDescription = entity.TPDescription;
                Entity.MRICode = entity.MRICode;
                Entity.CancerTitle = entity.CancerTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteMedicalRecord(Model.MedicalRecord entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه دادهوجود ندارد");
                db.MedicalRecord.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        public bool UpdateMedicalRecordForDoctorChange(long medicalRecordId, int doctorId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(medicalRecordId);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("");

                Model.Doctor DoctorObject = new DoctorService().GetDoctorById(doctorId);
                if (DoctorObject == null)
                    throw Common.MegaException.ThrowException("");

                Entity.DoctorFirstName = DoctorObject.FirstName;
                Entity.DoctorLastName = DoctorObject.LastName;
                Entity.DoctorId = DoctorObject.Id;
                return db.SaveChanges() == 1;
            }
        }

        public bool UpdateMedicalRecordForPhaseCount(long medicalRecordId, int phaseNo)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(medicalRecordId);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("");

                Entity.PhasesCount = phaseNo;
                Entity.PhasesPrescribedDate = DateTime.Now;
                Entity.TreatmentProcessId = 3; // تجویز درمان
                Entity.LastTreatmentProcessChangeDate = DateTime.Now;
                return db.SaveChanges() == 1;
            }
        }

        public bool SetCancerForMR(long medicalRecordId, int cancerId, string UserId)
        {
            try
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var Entity = db.MedicalRecord.Find(medicalRecordId);
                    if (Entity == null)
                        throw Common.MegaException.ThrowException("");

                    Model.Cancer CancerObject = new CancerService().GetCancerById(cancerId);
                    if (CancerObject == null)
                        throw Common.MegaException.ThrowException("");

                    Entity.CancerTitle = CancerObject.Title;
                    Entity.CancerId = CancerObject.Id;
                    Entity.ContourAcceptDate = DateTime.Now;
                    Entity.ContourAcceptUser = UserId;
                    Entity.TreatmentProcessId = 2; //کانتورینگ
                    Entity.LastTreatmentProcessChangeDate = DateTime.Now;
                    var MedicalRecordContourObject = new ContourService().GetContourByMedicalRecordId(medicalRecordId);
                    if (MedicalRecordContourObject == null)
                    {
                        Entity.Contour.Add(new Model.Contour()
                        {
                            AcceptDate = null,
                            AcceptUser = null,
                            ActionDate = DateTime.Now,
                            Description = "",
                            ExtraInfo1 = "",
                            ExtraInfo2 = "",
                            MedicalRecordId = medicalRecordId
                        });
                    }

                    int RowsAffected = db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private Object thisLock = new Object();

        /// <summary>
        /// این متد برای به دست آوردن شماره ی بعدی شناسه سیستمی پرونده استفاده می گردد
        /// </summary>
        /// <returns></returns>
        public string GetSystemCodeToRegister()
        {
            lock (thisLock)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var SystemCode = db.MedicalRecord.Max(x => x.SystemCode);
                    if (string.IsNullOrEmpty(SystemCode))
                    {
                        return "1000";
                    }
                    else
                    {
                        return (int.Parse(SystemCode) + 1).ToString();
                    }
                }
            }
        }
        public string GetSystemCodeToCTCode()
        {
            lock (thisLock)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var SystemCode = db.MedicalRecord.Max(x => x.CTCode);
                    if (string.IsNullOrEmpty(SystemCode))
                    {
                        return "1000";
                    }
                    else
                    {
                        return (int.Parse(SystemCode) + 1).ToString();
                    }
                }
            }
        }
        #endregion

        public Model.MedicalRecord AddMedicalRecordCTCode(string mriCode, string ctDescription, long medicalRecordId)
        {
            MedicalRecordService medicalRecordService = new MedicalRecordService();
            var medicalRecordEntity = medicalRecordService.GetMedicalRecordById(medicalRecordId);
            medicalRecordEntity.CTCode = GetSystemCodeToCTCode();
            medicalRecordEntity.CTDescription = ctDescription;
            medicalRecordEntity.CTEnterDate = DateTime.Now;
            medicalRecordEntity.MRICode = mriCode;
            medicalRecordEntity.MRIEnterDate = DateTime.Now;
            medicalRecordEntity.IsOnGoing = true;
            medicalRecordEntity.IsOnCalendar = false;
            var IsMedicalrecoedInsert = medicalRecordService.UpdateMedicalRecord(medicalRecordEntity);
            if (!IsMedicalrecoedInsert)
                throw Common.MegaException.ThrowException("امکان ثبت این اطلاعات وجود ندارد.لطفا با واحد فنی تماس بگیرید.");

            return medicalRecordEntity;
        }

        public ViewModels.DaysStatisticsVM GetTotalCTCodesStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    UnsetRecord = db.MedicalRecord.Count(e => e.CTEnterDate == null || e.MRIEnterDate == null),
                    Today = db.MedicalRecord.Count(e => (e.CTEnterDate != null && e.MRIEnterDate != null) && e.CTEnterDate <= TodayEnd && e.CTEnterDate >= TodayStart),
                    LastWeek = db.MedicalRecord.Count(e => (e.CTEnterDate != null && e.MRIEnterDate != null) && e.CTEnterDate >= LastWeekStart && e.CTEnterDate <= TodayEnd),
                    LastMonth = db.MedicalRecord.Count(e => (e.CTEnterDate != null && e.MRIEnterDate != null) && e.CTEnterDate >= LastMonthStart && e.CTEnterDate <= TodayEnd),
                    TotalRecords = db.MedicalRecord.Count()
                };
            }
        }
        public ViewModels.DaysStatisticsVM GetTotalTreatmentPlansStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    UnsetRecord = db.MedicalRecord.Count(e => e.NeedsFusion == null || e.TPEnterDate == null),
                    Today = db.MedicalRecord.Count(e => e.NeedsFusion != null && e.TPEnterDate >= TodayStart && e.TPEnterDate <= TodayEnd),
                    LastWeek = db.MedicalRecord.Count(e => e.NeedsFusion != null && e.TPEnterDate >= LastWeekStart && e.TPEnterDate <= TodayEnd),
                    LastMonth = db.MedicalRecord.Count(e => e.NeedsFusion != null && e.TPEnterDate >= LastMonthStart && e.TPEnterDate <= TodayEnd),
                    TotalRecords = db.MedicalRecord.Count()
                };
            }
        }
        public ViewModels.DaysStatisticsVM GetTotalContoursStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    UnsetRecord = db.MedicalRecord.Count(x => x.Contour.Count == 0),
                    Today = db.MedicalRecord.Count(x => x.Contour.Count != 0 && x.ContourAcceptDate >= TodayStart && x.ContourAcceptDate <= TodayEnd),
                    LastWeek = db.MedicalRecord.Count(x => x.Contour.Count != 0 && x.ContourAcceptDate >= LastWeekStart && x.ContourAcceptDate <= TodayEnd),
                    LastMonth = db.MedicalRecord.Count(x => x.Contour.Count != 0 && x.ContourAcceptDate >= LastMonthStart && x.ContourAcceptDate <= TodayEnd),
                    TotalRecords = db.MedicalRecord.Count()
                };
            }
        }
    }
}
