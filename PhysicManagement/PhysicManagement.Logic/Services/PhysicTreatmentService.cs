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
        // PhysicTreatmentPlan CRUD needed
        // PhysicTreatmentPlanHostory CRUD needed

    }
}
