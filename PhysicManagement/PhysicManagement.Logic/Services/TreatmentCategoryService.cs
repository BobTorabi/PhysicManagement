﻿using FluentValidation;
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
        public TreatmentCategory GetTreatmentCategoryById(int id)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategory.Find(id);
            }
        }
        public static List<Model.TreatmentCategory> GetAllTreatmentCategory()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategory.ToList();
            }
        }

        public List<Model.TreatmentCategoryService>
            GetTreatmentCategoryServiceByTreatmentCategoryId(int TreatmentCategoryId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return
                    db.TreatmentCategoryService
                    .Where(x => x.TreatmentCategoryId == TreatmentCategoryId && x.IsActive == true)
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
       
        public bool AddTreatmentCategory(Model.TreatmentCategory entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                entity.IsActive = true;
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
                var Entity = db.TreatmentCategoryService.Where(e=>e.TreatmentCategoryId == treatmentCategoryId && e.IsActive == true).ToList();
                return Entity;
            }
        }
        public bool AddTreatmentCategoryService(Model.TreatmentCategoryService entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryServiceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            if (string.IsNullOrEmpty(entity.Code))
                entity.Code = new Random().Next(1100, 9909).ToString();
            

            using (var db = new Model.PhysicManagementEntities())
            {
                entity.IsActive = true;
                db.TreatmentCategoryService.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentCategoryService(Model.TreatmentCategoryService entity)
        {
            var validation = new TreatmentCategoryValidation.TreatmentCategoryServiceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            if (entity.RelativeProfessionalValue == null)
                entity.RelativeProfessionalValue = 0;

            //if (entity.RelativeTechnicalValue == null)
            //    entity.RelativeTechnicalValue = 0;

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentCategoryService.Find(entity.Id);
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                Entity.RelativeTechnicalValue = entity.RelativeTechnicalValue;
                Entity.RelativeProfessionalValue = entity.RelativeProfessionalValue;
                Entity.RelativeValue = entity.RelativeProfessionalValue + entity.RelativeTechnicalValue;
                Entity.TreatmentCategoryId = entity.TreatmentCategoryId;
                Entity.Code = entity.Code;
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
