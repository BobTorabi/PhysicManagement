using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;
using System.Data.Entity;
using PhysicManagement.Model;

namespace PhysicManagement.Logic.Services
{
    public class ContourService
    {
        #region Contour section

        public List<Model.Contour> GetContourList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Contour.OrderBy(x => x.Id).ToList();
            }
        }

        public List<MedicalRecord> GetContoursToApprove()
        {
            using (var db = new PhysicManagementEntities())
            {
                return db
                    .MedicalRecord
                    .Where(x =>  x.ContourAcceptDate  == null)
                    .Include("Contour").Include("Patient").ToList();
            }
        }

        public Model.Contour GetContourByMedicalRecordId(long medicalRecordId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Contour.Where(e=>e.MedicalRecordId == medicalRecordId).Include(x=>x.ContourDetails).FirstOrDefault();
                return Entity;
            }
        }
        public Model.Contour GetContourById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
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

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Contour.Add(entity);
                return db.SaveChanges() == 1;
            }
        }

        public object SetContourAsAcceptedByDoctor(long countorId)
        {
            var UserData = Logic.Services.AuthenticatedUserService.GetUserId();

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Contour.Find(countorId);
                Entity.AcceptDate = DateTime.Now;
                Entity.AcceptUser = UserData.UserId.ToString();

                return db.SaveChanges() == 1;
            }
        }

        public bool UpdateContour(Model.Contour entity)
        {
            var validtion = new ContourValidation.ContourEntityValidate().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
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
            using (var db = new Model.PhysicManagementEntities())
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
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                return db.ContourDetails.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.ContourDetails GetContourDetailsById(int entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ContourDetails.Find(entityId);
                return Entity;
            }
        }
        public bool AddContourDetails(Model.ContourDetails entity)
        {
            var validation = new ContourValidation.ContourDetailsEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.ContourDetails.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public Model.ContourDetails GetContourDetailByMedicalRecordIdAndCancerOARId(long medicalRecordId,int cancerOARId, long contourId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
               return db.ContourDetails.FirstOrDefault(e=>e.CancerOARId == cancerOARId && e.ContourId == contourId && e.MediacalRecordId == medicalRecordId);
            }
        }
        public bool UpdateContourDetails(Model.ContourDetails entity)
        {
            var validtion = new ContourValidation.ContourDetailsEntityValidate().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ContourDetails.Find(entity.Id);
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteContourDetails(long entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ContourDetails.Find(entityId);
                if (Entity == null)
                    throw new ValidationException(" این رکورد در پایگاه داده وجود ندارد");
                db.ContourDetails.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion


    }
}
