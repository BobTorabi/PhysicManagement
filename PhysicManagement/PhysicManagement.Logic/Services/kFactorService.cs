using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PhysicManagement.Common;
using PhysicManagement.Logic.Validations;
using PhysicManagement.Model;

namespace PhysicManagement.Logic.Services
{
   public class kFactorService
    {
        #region PhysicUser section

        public List<Model.KFactor> GetkFactorList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.KFactor.OrderBy(x => x.Year).ToList();
            }
        }
        public Model.KFactor GetkFactorById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.KFactor.Find(entityId);
                return Entity;
            }
        }
        public bool AddkFactor(Model.KFactor entity)
        {
            var vallidtion = new KFactorValidation.KFactorEntityValidate().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.KFactor.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatekFactor(Model.KFactor entity)
        {
            var vallidtion = new KFactorValidation.KFactorEntityValidate().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.KFactor.Find(entity.Id);
                Entity.Year = entity.Year;
                Entity.GovernmentalFactor = entity.GovernmentalFactor;
                Entity.PrivateFactor = entity.PrivateFactor;
              

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletekFactor(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.KFactor.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");
                db.KFactor.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
