using PhysicManagement.Logic.Validations;
using PhysicManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
    public class PhysicTreatmentPlanService
    {
        #region PhysicTreatmentPlanSection
        public List<Model.PhysicTreatmentPlan> GetPhysicTreatmentPlanList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlan.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.PhysicTreatmentPlan GetPhysicTreatmentPlanById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entityId);
                return Entity;
            }
        }

        public Model.PhysicTreatmentPlan GetPhysicTreatmentPlanByMedicalRecordIdAndPlanNo(long medicalRecordId, int planNo)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Where(x => x.MedicalRecordId == medicalRecordId && x.PlanNo == planNo).FirstOrDefault();
                return Entity;
            }
        }

        public PhysicTreatmentPlan AddPhysicTreatmentPlan(Model.PhysicTreatmentPlan entity)
        {
            var validation = new PhysicTreatmentPlanValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors.ToString());

            using (var db = new Model.PhysicManagementEntities())
            {
                var result = db.PhysicTreatmentPlan.Add(entity);
                if (result == null)
                    return null;

                db.SaveChanges();
                return result;
                    
            }
        }
        public bool UpdatePhysicTreatmentPlan(Model.PhysicTreatmentPlan entity)
        {
            var validation = new PhysicTreatmentPlanValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors.ToString());

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.PhysicId = entity.PhysicId;
                Entity.DoctorComment = entity.DoctorComment;
                Entity.DoctorFullName = entity.DoctorFullName;
                Entity.DoctorId = entity.DoctorId;
                Entity.Fields = entity.Fields;
                Entity.IsApprovedByDoctor = entity.IsApprovedByDoctor;
                Entity.MedicalRecordId = entity.MedicalRecordId;
                Entity.PhysicApplyDate = entity.PhysicApplyDate;
                Entity.PhysicComment = entity.PhysicComment;
                Entity.PhysicFullName = entity.PhysicFullName;
                Entity.PlanNo = entity.PlanNo;

                return db.SaveChanges() == 1;
            }
        }

        public bool DeletePhysicTreatmentPlan(long entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlan.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicTreatmentPlan.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        #endregion


        #region PhysicTreatmentPlanDetailSection
        public List<Model.PhysicTreatmentPlanDetail> GetPhysicTreatmentPlanDetailList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlanDetail.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.PhysicTreatmentPlanDetail GetPhysicTreatmentPlanDetailById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanDetail.Find(entityId);
                return Entity;
            }
        }

        public List<Model.PhysicTreatmentPlanDetail> GetPhysicTreatmentPlanDetailByPhysicPlanId(int physicPlanId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicTreatmentPlanDetail.Where(x => x.PhysicTreatmentPlanId == physicPlanId).ToList();
            }
        }

        public PhysicTreatmentPlanDetail AddPhysicTreatmentPlanDetail(Model.PhysicTreatmentPlanDetail entity)
        {
            var validation = new PhysicTreatmentPlanDetailValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors.ToString());

            using (var db = new Model.PhysicManagementEntities())
            {
                var result = db.PhysicTreatmentPlanDetail.Add(entity);
                if (result == null)
                    return null;

                db.SaveChanges();
                return result;
            }
        }
        public bool UpdatePhysicTreatmentPlanDetail(Model.PhysicTreatmentPlanDetail entity)
        {
            var validation = new PhysicTreatmentPlanDetailValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors.ToString());

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanDetail.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.MedicalRecordId = entity.MedicalRecordId;
                Entity.CancerOARId = entity.CancerOARId;
                Entity.CancerOARIdValue = entity.CancerOARIdValue;
                Entity.CancerOARTitle = entity.CancerOARTitle;
                Entity.CancerOARTolerance = entity.CancerOARTolerance;
                Entity.CancerTargetId = entity.CancerTargetId;
                Entity.CancerTargetOptimum = entity.CancerTargetOptimum;
                Entity.CancerTargetTitle = entity.CancerTargetTitle;
                Entity.CancerTargetValue = entity.CancerTargetValue;

                return db.SaveChanges() == 1;
            }
        }

        public bool DeletePhysicTreatmentPlanDetail(long entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicTreatmentPlanDetail.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicTreatmentPlanDetail.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        #endregion
    }
}
