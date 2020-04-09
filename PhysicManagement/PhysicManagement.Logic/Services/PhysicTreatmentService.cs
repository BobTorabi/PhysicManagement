using FluentValidation;
using PhysicManagement.Logic.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class PhysicTreatmentService
    {
        #region PhysicTreatment Section
        public List<Model.PhysicTreatment> GetPhysicTreatmentList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatment.OrderBy(x => x.ActionDate).ToList();
            }
        }
        public Model.PhysicTreatment GetPhysicTreatmentById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatment.Find(entityId);
                return Entity;
            }
        }
        public bool AddPhysicTreatment(Model.PhysicTreatment entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            Logic.Services.MedicalRecordService md = new MedicalRecordService();
            var PhysicTreatmentObject =md.GetMedicalRecordById(entity.MedicalRecordId.GetValueOrDefault());
            if (PhysicTreatmentObject == null)
                throw Common.MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");
            entity.MedicalRecordId = Convert.ToInt32(PhysicTreatmentObject.MRICode);

            PatientService pa = new PatientService();
            var PhysicTreatmentObject2 = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
            if (PhysicTreatmentObject2 == null)
                throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
            entity.ActionUser = PhysicTreatmentObject2.LastName;

            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicTreatment.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicTreatment(Model.PhysicTreatment entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatment.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Logic.Services.MedicalRecordService md = new MedicalRecordService();
                var PhysicTreatmentObject = md.GetMedicalRecordById(entity.MedicalRecordId.GetValueOrDefault());
                if (PhysicTreatmentObject == null)
                    throw Common.MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");

                PatientService pa = new PatientService();
                var PhysicTreatmentObject2 = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
                if (PhysicTreatmentObject2 == null)
                    throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
                entity.ActionUser = PhysicTreatmentObject2.LastName;

                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.PhysicTreatmentPlan = entity.PhysicTreatmentPlan;
                Entity.ActionDate = entity.ActionDate;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicTreatment(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatment.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicTreatment.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region PhysicTreatmentPlan Section
        public List<Model.PhysicTreatmentPlan> GetPhysicTreatmentPlanList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlan.OrderBy(x => x.PhysicTreatmentId).ToList();
            }

        }
        public Model.PhysicTreatmentPlan GetPhysicTreatmentPlanById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entityId);
                return Entity;
            }
        }
        public bool AddPhysicTreatmentPlan(Model.PhysicTreatmentPlan entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentPlanEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            Logic.Services.CancerService cr = new CancerService();
            var PhysicTreatmentPlanObject = cr.GetCancerOARById(entity.CancerOARId.GetValueOrDefault());
            if (PhysicTreatmentPlanObject == null)
                throw Common.MegaException.ThrowException("ارگان وارد شده در پایگاه داده وجود ندارد.");
           entity.CancerOARId = PhysicTreatmentPlanObject.Id;

            var PhysicTreatmentPlanObject2 = GetPhysicTreatmentById(entity.PhysicTreatmentId.GetValueOrDefault());
            if (PhysicTreatmentPlanObject2 == null)
                throw Common.MegaException.ThrowException("شناسه درمان فیزیکی وارد شده در پایگاه داده وجود ندارد");
            entity.PhysicTreatmentId = PhysicTreatmentPlanObject2.Id;

            PatientService pa = new PatientService();
            var PhysicTreatmenPlantObject3 = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
            if (PhysicTreatmenPlantObject3 == null)
                throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
            entity.ActionUser = PhysicTreatmenPlantObject3.LastName;

            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicTreatmentPlan.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicTreatmentPlan(Model.PhysicTreatmentPlan entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentPlanEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Logic.Services.CancerService cr = new CancerService();
                var PhysicTreatmentPlanObject = cr.GetCancerOARById(entity.CancerOARId.GetValueOrDefault());
                if (PhysicTreatmentPlanObject == null)
                    throw Common.MegaException.ThrowException("ارگان وارد شده در پایگاه داده وجود ندارد.");
                entity.CancerOARId = PhysicTreatmentPlanObject.Id;

                var PhysicTreatmentPlanObject2 = GetPhysicTreatmentById(entity.PhysicTreatmentId.GetValueOrDefault());
                if (PhysicTreatmentPlanObject2 == null)
                    throw Common.MegaException.ThrowException("شناسه درمان فیزیکی وارد شده در پایگاه داده وجود ندارد");
                entity.PhysicTreatmentId = PhysicTreatmentPlanObject2.Id;

                PatientService pa = new PatientService();
                var PhysicTreatmenPlantObject3 = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
                if (PhysicTreatmenPlantObject3 == null)
                    throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
                entity.ActionUser = PhysicTreatmenPlantObject3.LastName;

                Entity.HadContour = entity.HadContour;
                Entity.IsAcceptedByDoctor = entity.IsAcceptedByDoctor;
                Entity.IsAcceptedByPhysic = entity.IsAcceptedByPhysic;
                Entity.PlannedDose = entity.PlannedDose;
                Entity.Reserve1 = entity.Reserve1;
                Entity.Reserve2 = entity.Reserve2;
                Entity.AcceptedDoctorUser = entity.AcceptedDoctorUser;
                Entity.ActionDate = entity.ActionDate;
                Entity.Evaluation = entity.Evaluation;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicTreatmentPlan(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicTreatmentPlan.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region PhysicTreatmentPlanHostory
        public List<Model.PhysicTreatmentPlanHostory> GetPhysicTreatmentPlanHostoryList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlanHostory.OrderBy(x => x.PhysicTreatmentPlanId).ToList();
            }
        }
        public Model.PhysicTreatmentPlanHostory GetPhysicTreatmentPlanHostoryById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanHostory.Find(entityId);
                return Entity;
            }
        }
        public bool AddPhysicTreatmentPlanHostory(Model.PhysicTreatmentPlanHostory entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentPlanHostoryEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            PatientService pa = new PatientService();
            var PhysicTreatmentHostoryObject = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
            if (PhysicTreatmentHostoryObject == null)
                throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
            entity.ActionUser = PhysicTreatmentHostoryObject.LastName;

            var PhysicTreatmentHostoryObject2 = GetPhysicTreatmentPlanById(entity.PhysicTreatmentPlanId.GetValueOrDefault());
            if (PhysicTreatmentHostoryObject2 == null)
                throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
            entity.PhysicTreatmentPlanId = PhysicTreatmentHostoryObject2.Id;

            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicTreatmentPlanHostory.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicTreatmentPlanHostory(Model.PhysicTreatmentPlanHostory entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentPlanHostoryEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanHostory.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                PatientService pa = new PatientService();
                var PhysicTreatmentHostoryObject = pa.GetPatientById(Convert.ToInt32(entity.ActionUser));
                if (PhysicTreatmentHostoryObject == null)
                    throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
                entity.ActionUser = PhysicTreatmentHostoryObject.LastName;

                var PhysicTreatmentHostoryObject2 = GetPhysicTreatmentPlanById(entity.PhysicTreatmentPlanId.GetValueOrDefault());
                if (PhysicTreatmentHostoryObject2 == null)
                    throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
                entity.PhysicTreatmentPlanId = PhysicTreatmentHostoryObject2.Id;

                Entity.HasAlarm = entity.HasAlarm;
                Entity.IsDoctor = entity.IsDoctor;
                Entity.Note = entity.Note;
                Entity.ChangeDate = entity.ChangeDate;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicTreatmentPlanHostory(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanHostory.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicTreatmentPlanHostory.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion

    }
}
