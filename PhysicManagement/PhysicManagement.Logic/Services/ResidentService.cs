using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class ResidentService
    {
        public List<Model.Resident> GetResidentList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Resident.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.Resident GetResidentById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entityId);
                return Entity;
            }
        }
        public bool AddResident(Model.Resident entity)
        {
            var vallidtion = new ResidentValidation.ResidentEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Resident.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateResident(Model.Resident entity)
        {
            var vallidtion = new ResidentValidation.ResidentEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Username = entity.Username;
                Entity.Password = entity.Password;
                Entity.Mobile = entity.Mobile;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteResident(Model.Resident entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");
                db.Resident.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
    }
}
