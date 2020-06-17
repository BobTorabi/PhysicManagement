using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Model;
using System;


namespace PhysicManagement.Logic.Services
{
    public class TreatmentCategoryService
    {
        public static List<Model.KFactor> GetAllKFaktors()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.KFactor.ToList();
            }
        }
        public static Model.KFactor GetKFaktorByYear(string year)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.KFactor.Where(x => x.Year == year).FirstOrDefault();
            }
        }

        public static List<Model.TreatmentCategory> GetAllTreatmentCategory()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategory.ToList();
            }
        }
        public static List<Model.TreatmentCategoryService>
            GetTreatmentCategoryServiceByTreatmentCategoryId(int TreatmentCategoryId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return
                    db.TreatmentCategoryService
                    .Where(x => x.TreatmentCategoryId == TreatmentCategoryId)
                    .ToList();
            }
        }
        #region TreatmentCategory
        public List<Model.TreatmentCategory> GetTreatmentCategoryList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategory.Include("TreatmentCategoryService").OrderBy(x => x.Title).ToList();
            }
        }
        public Model.TreatmentCategory GetTreatmentCategoryById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategory.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentCategory(Model.TreatmentCategory entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentCategory.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentCategory(Model.TreatmentCategory entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategory.Find(entity.Id);
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentCategory(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategory.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentCategory.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region TreatmentCategoryService
        public List<Model.TreatmentCategoryService> GetTreatmentCategoryServiceList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategoryService.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.TreatmentCategoryService GetTreatmentCategoryServiceById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategoryService.Find(entityId);
                return Entity;
            }
        }
        public List<Model.TreatmentCategoryService> GetTreatmentCategoriesServiceByTreatmentCategoryId(int treatmentCategoryId)
        {
            using (var db = new PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategoryService.Where(e=>e.TreatmentCategoryId == treatmentCategoryId).ToList();
                return Entity;
            }
        }
        public bool AddTreatmentCategoryService(Model.TreatmentCategoryService entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryServiceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentCategoryService.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentCategoryService(Model.TreatmentCategoryService entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryServiceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategoryService.Find(entity.Id);
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentCategoryService(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategoryService.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentCategoryService.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }

}
