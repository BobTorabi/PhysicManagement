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
                return db.KFactor.Where(x => x.IsActive == true).OrderBy(x => x.Year).ToList();
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

            if (entity.GovernmentalProfessionalFactor == null)
                entity.GovernmentalProfessionalFactor = 0;

            if (entity.GovernmentalTechnicalFactor == null)
                entity.GovernmentalTechnicalFactor = 0;

            if (entity.PrivateProfessionalFactor == null)
                entity.PrivateProfessionalFactor = 0;

            if (entity.PrivateTechnicalFactor== null)
                entity.PrivateTechnicalFactor = 0;

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.KFactor.Find(entity.Id);
                Entity.Year = entity.Year;
                Entity.GovernmentalProfessionalFactor = entity.GovernmentalProfessionalFactor;
                Entity.GovernmentalTechnicalFactor = entity.GovernmentalTechnicalFactor;
                Entity.PrivateTechnicalFactor = entity.PrivateTechnicalFactor;
                Entity.PrivateProfessionalFactor = entity.PrivateProfessionalFactor;
                Entity.PrivateFactor = entity.PrivateTechnicalFactor + entity.PrivateProfessionalFactor;
                Entity.GovernmentalFactor = entity.GovernmentalTechnicalFactor + entity.GovernmentalProfessionalFactor;

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
