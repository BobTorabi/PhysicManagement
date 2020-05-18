using PhysicManagement.Logic.Validations;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class CancerService
    {
        #region Cancer section
        public List<Model.Cancer> GetCancerList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Cancer.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.Cancer GetCancerById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Cancer.Find(entityId);
                return Entity;
            }
        }
       
        public Model.Cancer GetCancerByTitle(string title)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Cancer.Where(x=>x.Title == title).FirstOrDefault();
                return Entity;
            }
        }
        public bool AddCancer(Model.Cancer entity)
        {
            var validation = new CancerValidation.
                            CancerEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Cancer.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateCancer(Model.Cancer entity)
        {
            var validation = new CancerValidation.CancerEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Cancer.Find(entity.Id);
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;
                Entity.Title = entity.Title;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteCancer(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Cancer.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Cancer.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region CancerOAR section
        public List<Model.CancerOAR> GetCancerOARList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerOAR.Include("Cancer").OrderBy(x => x.Cancer.Title).ToList();
            }
        }
        public Model.CancerOAR GetCancerOARById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerOAR.Find(entityId);
                return Entity;
            }
        }
        public List<Model.CancerOAR> GetCancerOARListByCancerId(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerOAR.Where(x => x.CancerId == entityId).OrderBy(x => x.OrganTitle).ToList();
            }
        }
        public bool AddCancerOAR(Model.CancerOAR entity)
        {
            var validation = new CancerValidation.CancerOAREntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.CancerOAR.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateCancerOAR(Model.CancerOAR entity)
        {
            var validation = new CancerValidation.CancerOAREntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerOAR.Find(entity.Id);
                Entity.CancerId = entity.CancerId;
                Entity.Description = entity.Description;
                Entity.OrganTitle = entity.OrganTitle;
                Entity.Reserve1 = entity.Reserve1;
                Entity.Reserve2 = entity.Reserve2;
                Entity.Tolerance = entity.Tolerance;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteCancerOAR(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerOAR.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.CancerOAR.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        public int GetCancerOARByCancerId(int cancerId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerOAR.Count(e => e.CancerId == cancerId);
                return Entity;
            }
        }
        #endregion
        #region Cancer Targets section
        public List<Model.CancerTargets> GetCancerTargetList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerTargets.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.CancerTargets GetCancerTargetById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTargets.Find(entityId);
                return Entity;
            }
        }
        public List<Model.CancerTargets> GetCancerTargetListByCancerId(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerTargets.Where(x => x.CancerId == entityId).OrderBy(x => x.Title).ToList();
            }
        }
        public bool AddCancerTarget(Model.CancerTargets entity)
        {
            var validation = new CancerValidation.CancerTargetEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.CancerTargets.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateCancerTarget(Model.CancerTargets entity)
        {
            var validation = new CancerValidation.CancerTargetEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTargets.Find(entity.Id);
                Entity.CancerId = entity.CancerId;
                Entity.Optimum = entity.Optimum;
                Entity.Title = entity.Title;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteCancerTarget(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTargets.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.CancerTargets.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        public int GetCancerTargetsByCancerId(int cancerId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTargets.Count(e => e.CancerId == cancerId);
                return Entity;
            }
        }
        #endregion
    }
}
