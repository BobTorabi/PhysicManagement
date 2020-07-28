using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using PhysicManagement.Model;
using PhysicManagement.Common;
using PhysicManagement.Logic.ViewModels;

namespace PhysicManagement.Logic.Services
{
    public class CancerService
    {
        #region Cancer section
        public List<Model.Cancer> GetCancerList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Cancer.ToList();

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

        public PagedList<Cancer> GetCancerList(string title, string englishTitle,int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Model.Cancer> QueryableCancer = db.Cancer;

                if (!string.IsNullOrEmpty(title))
                {
                    title = title.Trim().ToPersian();
                    QueryableCancer = QueryableCancer.Where(x => x.Title.Contains(title));
                }
                if (!string.IsNullOrEmpty(englishTitle))
                {
                    englishTitle = englishTitle.Trim().ToPersian();
                    QueryableCancer = QueryableCancer.Where(x => x.EnglishTitle.Contains(englishTitle));
                }
                QueryableCancer = QueryableCancer.OrderByDescending(x => x.Id);
                return new ViewModels.PagedList<Model.Cancer>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryableCancer.Count(),
                    Records = QueryableCancer.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
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
        public List<Model.CancerTarget> GetCancerTargetList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerTarget.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.CancerTarget GetCancerTargetById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTarget.Find(entityId);
                return Entity;
            }
        }
        public List<Model.CancerTarget> GetCancerTargetListByCancerId(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.CancerTarget.Where(x => x.CancerId == entityId).OrderBy(x => x.Title).ToList();
            }
        }
        public bool AddCancerTarget(Model.CancerTarget entity)
        {
            var validation = new CancerValidation.CancerTargetEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.CancerTarget.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateCancerTarget(Model.CancerTarget entity)
        {
            var validation = new CancerValidation.CancerTargetEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTarget.Find(entity.Id);
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
                var Entity = db.CancerTarget.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.CancerTarget.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        public int GetCancerTargetsByCancerId(int cancerId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.CancerTarget.Count(e => e.CancerId == cancerId);
                return Entity;
            }
        }
        #endregion
    }
}
