using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class PhysicUserService
    {
        #region PhysicUser section

        public List<Model.PhysicUser> GetPhysicUserList()
        { 
            using(var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicUser.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.PhysicUser GetPhysicUserById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUser.Find(entityId);
                return Entity;
            }
        }
        public bool AddPhysicUser(Model.PhysicUser entity)
        {
            var vallidtion = new PhysicUserValidation.PhysicUserEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);
            
            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicUser.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicUser(Model.PhysicUser entity)
        {
            var vallidtion = new PhysicUserValidation.PhysicUserEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUser.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Username = entity.Username;
                Entity.Password = entity.Password;
                Entity.Mobile = entity.Mobile;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicUser(Model.PhysicUser entityId)
        {
            using (var db = new Model.PhysicManagementEntities)
            {
                var Entity = db.PhysicUser.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");
                db.PhysicUser.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
