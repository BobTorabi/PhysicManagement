using FluentValidation;
using PhysicManagement.Logic.Validations;
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
                Entity.MedicalRecordId = entity.MedicalRecordId;
                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.PhysicTreatmentPlan = entity.PhysicTreatmentPlan;
                Entity.ActionDate = entity.ActionDate;
                Entity.ActionUser = entity.ActionUser;

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
                return db.PhysicTreatmentPlan.OrderBy(x => x.PhysicTreatment).ToList();
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
                Entity.HadContour = entity.HadContour;
                Entity.IsAcceptedByDoctor = entity.IsAcceptedByDoctor;
                Entity.IsAcceptedByPhysic = entity.IsAcceptedByPhysic;
                Entity.PhysicTreatment = entity.PhysicTreatment;
                Entity.PhysicTreatmentId = entity.PhysicTreatmentId;
                Entity.PhysicTreatmentPlanHostory = entity.PhysicTreatmentPlanHostory;
                Entity.PlannedDose = entity.PlannedDose;
                Entity.Reserve1 = entity.Reserve1;
                Entity.Reserve2 = entity.Reserve2;
                Entity.AcceptedDoctorUser = entity.AcceptedDoctorUser;
                Entity.ActionDate = entity.ActionDate;
                Entity.ActionUser = entity.ActionUser;
                Entity.CancerOARId = entity.CancerOARId;
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
                Entity.HasAlarm = entity.HasAlarm;
                Entity.IsDoctor = entity.IsDoctor;
                Entity.Note = entity.Note;
                Entity.PhysicTreatmentPlan = entity.PhysicTreatmentPlan;
                Entity.PhysicTreatmentPlanId = entity.PhysicTreatmentPlanId;
                Entity.ActionUser = entity.ActionUser;
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
