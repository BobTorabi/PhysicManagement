using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Common;
using System.Data.Entity;

namespace PhysicManagement.Logic.Services
{
    public class TreatmentService
    {
        #region TreatmentPhase Section

        public PagedList<Model.MedicalRecord> 
            GetTreatmentPhasesListForDoctor(string firstName, string lastName, string mobile,
             string nationalCode, string systemCode, string code, int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var MRList = GetMedicalRecordsNotApprovedByDoctor();
                IQueryable<Model.MedicalRecord> QueryableMR =
                    db.MedicalRecord.Where(t => MRList.Contains(t.Id)).Include(x => x.Patient);
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Mobile.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.NationalCode.Contains(nationalCode));
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.SystemCode.Contains(systemCode));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    mobile = code.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Code.Contains(code));
                }
                QueryableMR = QueryableMR.OrderByDescending(x => x.ReceptionDate);
                return new PagedList<Model.MedicalRecord>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryableMR.Count(),
                    Records = QueryableMR.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
            }
        }

        public PagedList<Model.MedicalRecord>
           GetTreatmentPhasesListForPhysist(string firstName, string lastName, string mobile,
            string nationalCode, string systemCode, string code, int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var MRList = GetMedicalRecordsNotApprovedByPhysist();
                IQueryable<Model.MedicalRecord> QueryableMR =
                    db.MedicalRecord.Where(t => MRList.Contains(t.Id)).Include(x => x.Patient);
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Mobile.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.NationalCode.Contains(nationalCode));
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.SystemCode.Contains(systemCode));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    mobile = code.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Code.Contains(code));
                }
                QueryableMR = QueryableMR.OrderByDescending(x => x.ReceptionDate);
                return new PagedList<Model.MedicalRecord>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryableMR.Count(),
                    Records = QueryableMR.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
            }
        }
        public Model.TreatmentPhase GetTreatmentPhaseById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentPhase(Model.TreatmentPhase entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var md = new MedicalRecordService();
                var MedicalRecordObject = md.GetMedicalRecordById(entity.MedicalRecordId.GetValueOrDefault());
                if (MedicalRecordObject == null)
                    throw MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");

                db.TreatmentPhase.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool AddTreatmentPhaseDetail(Model.TreatmentPhaseDetail entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseDetailEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentPhaseDetail.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentPhase(Model.TreatmentPhase entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entity.Id);
                if (Entity == null)
                    throw MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                var md = new MedicalRecordService();
                var MedicalRecordObject = md.GetMedicalRecordById(entity.MedicalRecordId.GetValueOrDefault());
                if (MedicalRecordObject == null)
                    throw MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");
                entity.MedicalRecordId = Convert.ToInt64(MedicalRecordObject.MRICode);

                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.PhaseText = entity.PhaseText;
                Entity.PrescribeDate = entity.PrescribeDate;
                Entity.Target = entity.Target;
                Entity.Description = entity.Description;
                Entity.PatientFirstName = MedicalRecordObject.PatientFirstName;
                Entity.PatientLastName = MedicalRecordObject.PatientLastName;
                Entity.PhaseText = entity.PhaseText;
                Entity.TreatmentDeviceId = entity.TreatmentDeviceId;
                Entity.TreatmentDeviceTitle = entity.TreatmentDeviceTitle;
                Entity.IsApproved = entity.IsApproved;
                Entity.Fraction = entity.Fraction;
                Entity.IsPrescribedByDoctor = entity.IsPrescribedByDoctor;

                return db.SaveChanges() == 1;

            }
        }


        public bool UpdateTreatmentPhaseDetail(Model.TreatmentPhaseDetail entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseDetailEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhaseDetail.Find(entity.Id);
                if (Entity == null)
                    throw MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                var md = new MedicalRecordService();
                var MedicalRecordObject = md.GetMedicalRecordById(entity.MedicalRecordId.GetValueOrDefault());
                if (MedicalRecordObject == null)
                    throw MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");
                entity.MedicalRecordId = Convert.ToInt64(MedicalRecordObject.MRICode);

                Entity.AcceptedDoctorDate = entity.AcceptedDoctorDate;
                Entity.AcceptedDoctorFullName = entity.AcceptedDoctorFullName;
                Entity.AcceptedDoctorUserId = entity.AcceptedDoctorUserId;
                Entity.CancerOARId = entity.CancerOARId;
                Entity.CancerOARTitle = entity.CancerOARTitle;
                Entity.CancerOARTolerance = entity.CancerOARTolerance;
                Entity.CancerTargetId = entity.CancerTargetId;
                Entity.CancerTargetOptimum = entity.CancerTargetOptimum;
                Entity.CancerTargetTitle = entity.CancerTargetTitle;
                Entity.Description = entity.Description;
                Entity.DoctorDescription = entity.DoctorDescription;
                Entity.Evaluation = entity.Evaluation;
                Entity.HadContour = entity.HadContour;
                Entity.MedicalRecordId = entity.MedicalRecordId;
                Entity.PatientFirstName = entity.PatientFirstName;
                Entity.PatientLastName = entity.PatientLastName;
                Entity.PhysicPlanHasAccepted = entity.PhysicPlanHasAccepted;
                Entity.PhysicUserFullName = entity.PhysicUserFullName;
                Entity.PlannedDose = entity.PlannedDose;
                Entity.PresciptionHasApproved = entity.PresciptionHasApproved;
                Entity.PrescribedDate = entity.PrescribedDate;
                Entity.PrescribedDose = entity.PrescribedDose;
                Entity.PrescribedUser = entity.PrescribedUser;
                Entity.Reserve3 = entity.Reserve3;
                Entity.Reserved1 = entity.Reserved1;
                Entity.Reserved2 = entity.Reserved2;
                Entity.TreatmentPhaseId = entity.TreatmentPhaseId;



                return db.SaveChanges() == 1;

            }
        }
        public bool DeleteTreatmentPhase(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentPhase.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        public List<Model.TreatmentPhase> GetTreatmentPhaseList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentPhase.OrderBy(x => x.Id).ToList();
            }
        } 
        /// <summary>
        /// بازگردانی لیست شناسه پرونده پزشکی هایی که فاز درمانی آنها تائید نشده باشد
        /// </summary>
        /// <returns></returns>
        public long[] GetMedicalRecordsNotApprovedByDoctor()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return 
                    db
                    .TreatmentPhase
                    .Where(x => x.IsApproved != true && x.IsPrescribedByDoctor == null)
                    .OrderBy(x => x.Id)
                    .Select(x => x.MedicalRecordId).Distinct().ToList()
                    .Where(x=>x.HasValue).Select(x=>x.Value).ToArray();
            }
        }
        /// <summary>
        /// بازگردانی لیست شناسه پرونده پزشکی هایی که فاز درمانی آنها تائید نشده باشد
        /// </summary>
        /// <returns></returns>
        public long[] GetMedicalRecordsNotApprovedByPhysist()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return
                    db
                    .TreatmentPhase
                    .Where(x => x.IsApproved == null && x.IsPrescribedByDoctor == true)
                    .OrderBy(x => x.Id)
                    .Select(x => x.MedicalRecordId).ToList()
                    .Where(x => x.HasValue).Select(x => x.Value).ToArray();
            }
        }

        public List<Model.TreatmentPhase> GetTreatmentPhasesByMedicalRecordId(long medicalRecordId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentPhase.Where(x=>x.MedicalRecordId == medicalRecordId).OrderBy(x => x.Id).ToList();
            }
        }

        public List<Model.TreatmentPhaseDetail> GetTreatmentPhaseDetatilssByPhaseIds(long[] phaseIds)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return
                    db.TreatmentPhaseDetail
                    .Where(x => x.TreatmentPhaseId != null  && phaseIds.Contains(x.TreatmentPhaseId.Value))
                    .OrderBy(x => x.Id).ToList();
            }
        }
        public List<Model.TreatmentPhaseDetail> GetTreatmentPhaseDetatilssByMedicalRecordId(long medicalRecordId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Phases = GetTreatmentPhasesByMedicalRecordId(medicalRecordId);
                var PhaseIds = Phases.Select(x => x.Id).ToArray();
                return 
                    db.TreatmentPhaseDetail
                    .Where(x => x.TreatmentPhaseId!= null && x.MedicalRecordId == medicalRecordId && PhaseIds.Contains(x.TreatmentPhaseId.Value) )
                    .OrderBy(x => x.Id).ToList();
            }
        }
        public ViewModels.DaysStatisticsVM GetTotalTreatmentPhaseStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    UnsetRecord = db.TreatmentPhase.Count(e => e.IsApproved != true && e.IsPrescribedByDoctor == null),
                    Today = db.TreatmentPhase.Count(e => (e.PrescribeDate != null) && e.PrescribeDate <= TodayEnd && e.PrescribeDate >= TodayStart),
                    LastWeek = db.TreatmentPhase.Count(e => (e. PrescribeDate!= null) && e.PrescribeDate >= LastWeekStart && e.PrescribeDate <= TodayEnd),
                    LastMonth = db.TreatmentPhase.Count(e => (e.PrescribeDate != null) && e.PrescribeDate >= LastMonthStart && e.PrescribeDate <= TodayEnd),
                    TotalRecords = db.TreatmentPhase.Count()
                };
            }
        }
        public ViewModels.DaysStatisticsVM GetTotalConformTreatmentPhaseStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    UnsetRecord = db.TreatmentPhase.Count(e => e.IsApproved == null && e.IsPrescribedByDoctor == true),
                    Today = db.TreatmentPhase.Count(e => (e.ApprovedDate != null) && e.ApprovedDate <= TodayEnd && e.ApprovedDate >= TodayStart),
                    LastWeek = db.TreatmentPhase.Count(e => (e.ApprovedDate != null) && e.ApprovedDate >= LastWeekStart && e.ApprovedDate <= TodayEnd),
                    LastMonth = db.TreatmentPhase.Count(e => (e.ApprovedDate != null) && e.ApprovedDate >= LastMonthStart && e.ApprovedDate <= TodayEnd),
                    TotalRecords = db.TreatmentPhase.Count()
                };
            }
        }
        #endregion
        #region TreatmentProcess Section
        public List<Model.TreatmentProcess> GetTreatmentProcessList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentProcess.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.TreatmentProcess GetTreatmentProcessById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentProcess(Model.TreatmentProcess entity)
        {
            var validation = new TreatmentValidation.TreatmentProcessEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentProcess.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentProcess(Model.TreatmentProcess entity)
        {
            var validation = new TreatmentValidation.TreatmentProcessEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entity.Id);
                Entity.StepNumber = entity.StepNumber;
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentProcess(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentProcess.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region TreatmentDevice Section
        public List<Model.TreatmentDevice> GetTreatmentDeviceList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentDevice.OrderBy(x => x.Title).OrderBy(e => e.Code).ToList();
            }
        }
        public Model.TreatmentDevice GetTreatmentDeviceById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentDevice(Model.TreatmentDevice entity)
        {
            var validation = new TreatmentValidation.TreatmentDeviceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentDevice.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentDevice(Model.TreatmentDevice entity)
        {
            var validation = new TreatmentValidation.TreatmentDeviceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entity.Id);
                Entity.Title = entity.Title;
                Entity.Code = entity.Code;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentDevice(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentDevice.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion


    }
}
