using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
   public class ContourService
   {
        #region Contour section

        public List<Model.Contour> GetContourList()
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                return db.Contour.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.Contour GetContourById(int entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Contour.Find(entityId);
                return Entity;
            }
        }
        public bool AddContour(Model.Contour entity)
        {
            var validtion = new ContourValidation.ContourEntityValidate().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using(var db = new Model.PhysicManagementEntities())
            {
                db.Contour.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateContour(Model.Contour entity)
        {
            var validtion = new ContourValidation.ContourEntityValidate().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Contour.Find(entity.Id);
                Entity.Description = entity.Description;
                Entity.ExtraInfo1 = entity.ExtraInfo1;
                Entity.ExtraInfo2 = entity.ExtraInfo2;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteContour(Model.Contour entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Contour.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Contour.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion

        #region ContourDetail section

        public List<Model.ContourDetails> GetContourDetailsList()
        { }
        public Model.ContourDetails GetContourDetailsById(int entityId)
        { }
        public bool AddContourDetails(Model.ContourDetails entity)
        { }
        public bool UpdateContourDetails(Model.ContourDetails entity)
        { }
        public bool DeleteContourDetails(Model.ContourDetails entityId)
        { }
        #endregion


    }
}
