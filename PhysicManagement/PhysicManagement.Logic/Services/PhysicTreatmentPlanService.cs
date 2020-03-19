using PhysicManagement.Logic.Validations;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;

namespace PhysicManagement.Logic.Services
{
    public class PhysicTreatmentPlanService
    {
        #region PhysicTreatment section
        public List<Model.PhysicTreatment> GetPhysicTreatmentList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatment.OrderBy(x => x.PhaseNumber).ToList();
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
                Entity.ActionDate = entity.ActionDate;
                Entity.ActionUser = entity.ActionUser;
                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.MedicalRecordId = entity.MedicalRecordId;
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
        #region PhysicTreatmentPlan section
        public List<Model.PhysicTreatmentPlan> GetPhysicTreatmentPlanList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlan.OrderBy(x => x.Id).ToList();
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
        public bool UpdatePhysicTreatment(Model.PhysicTreatmentPlan entity)
        {
            var validation = new PhysicTreatmentValidation.PhysicTreatmentPlanEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entity.Id);
                Entity.ActionDate = entity.ActionDate;
                Entity.ActionUser = entity.ActionUser;
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
        #region Physic Treatment Plan History section
        public List<Model.PhysicTreatmentPlanHostory> GetPhysicTreatmentPlanHistoryList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlanHostory.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.PhysicTreatmentPlanHostory GetPhysicTreatmentPlanHistoryById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanHostory.Find(entityId);
                return Entity;
            }
        }

        public bool AddPhysicTreatmentPlanHistory(Model.PhysicTreatmentPlanHostory entity)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicTreatmentPlanHostory.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicTreatmentPlanHistory(Model.PhysicTreatmentPlanHostory entity)
        {

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanHostory.Find(entity.Id);
                db.Set<Model.PhysicTreatmentPlanHostory>().AddOrUpdate(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicTreatmentPlanHistory(int entityId)
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
