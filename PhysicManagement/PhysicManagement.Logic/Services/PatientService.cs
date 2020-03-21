using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class PatientService
    {
        #region Patient section
        public List<Model.Patient> GetPatientList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Patient.OrderBy(x=> x.FirstName).ToList();
            }
        }
        public Model.Patient GetPatientById(int entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Find(entityId);
                return Entity;
            }
        }
        public bool AddPatient(Model.Patient entity)
        {
            var validation = new PatientValidation.PatientEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Patient.Add(entity);
                return db.SaveChanges() ==1;
            }
        }
        public bool UpdatePatient (Model.Patient entity)
        {
            var validation = new PatientValidation.PatientEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Mobile = entity.Mobile;
                Entity.NationalCode = entity.NationalCode;
                Entity.Code = entity.Code;
                Entity.Province = entity.Province;
                Entity.City = entity.City;
                Entity.Address = entity.Address;
                Entity.RegisterDate = entity.RegisterDate;
                Entity.MedicalRecord = entity.MedicalRecord;
                Entity.GenderIsMale = entity.GenderIsMale;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePatient(int entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity= db.Patient.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Patient.Remove(Entity);
                return db.SaveChanges() == 1;

            }
        }
        #endregion

    }

}
