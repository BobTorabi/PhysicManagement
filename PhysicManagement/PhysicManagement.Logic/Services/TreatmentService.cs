using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class TreatmentService
    {
        #region TreatmentPhase Section
        public List<Model.TreatmentPhase> GetTreatmentPhasesList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentPhase.OrderBy(x => x.Target).ToList();
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
                db.TreatmentPhase.Add(entity);
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
                Entity.MedicalRecordId = entity.MedicalRecordId;
                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.PhaseText = entity.PhaseText;
                Entity.PrescribeDate = entity.PrescribeDate;
                Entity.PrescribesdUser = entity.PrescribesdUser;
                Entity.Target = entity.Target;
                Entity.Description = entity.Description;
                Entity.Dose = entity.Dose;
                Entity.Fraction = entity.Fraction;

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

        // Add TreatmentDevice CRUD section 
    }
}
