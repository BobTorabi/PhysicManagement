using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class DoctorService
    {
        #region Doctor section

        public List<Model.Doctor> GetDoctorList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Doctor.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.Doctor GetDoctorById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entityId);
                return Entity;
            }
        }
        public bool AddDoctor(Model.Doctor entity)
        {
            var validation = new DoctorValidation.DoctorEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Doctor.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateDoctor(Model.Doctor entity)
        {
            var validation = new DoctorValidation.DoctorEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Gender = entity.Gender;
                Entity.Mobile = entity.Mobile;
                Entity.Code = entity.Code;
                Entity.Degree = entity.Degree;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteDoctor(Model.Doctor entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Doctor.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
