using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class AlarmService
    {
        #region Alarm Section
        public List<Model.Alarm> GetAlarmList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Alarm.OrderBy(x => x.ReviewUserName).ToList();
            }
        }
        public Model.Alarm GetAlarmById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Alarm.Find(entityId);
                return Entity;
            }
        }
        public bool AddAlarm(Model.Alarm entity)
        {
            var validation = new AlarmValidation.AlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            using (var db = new Model.PhysicManagementEntities())
            {
                db.Alarm.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateAlarm(Model.Alarm entity)
        {
            var validation = new AlarmValidation.AlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Alarm.Find(entity.Id);
                Entity.IsActive = entity.IsActive;
                Entity.IsOnBoard = entity.IsOnBoard;
                Entity.ReviewDate = entity.ReviewDate;
                Entity.ReviewText = entity.ReviewText;
                Entity.ReviewUserName = entity.ReviewUserName;
                Entity.AlarmTypeId = entity.AlarmTypeId;
                Entity.AlarmTypeTitle = entity.AlarmTypeTitle;
                Entity.GenerateDate = entity.GenerateDate;
                Entity.GenerateTreatmentPhaseId = entity.GenerateTreatmentPhaseId;
                Entity.GenerateTreatmentPhaseTitle = entity.GenerateTreatmentPhaseTitle;
                Entity.GenerateUser = entity.GenerateUser;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteAlarm(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Alarm.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Alarm.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region AlarmType section
        public List<Model.AlarmType> GetAlarmTypeList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.AlarmType.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.AlarmType GetAlarmTypeById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmType.Find(entityId);
                return Entity;
            }
        }
        public bool AddAlarmType(Model.AlarmType entity)
        {
            var validation = new AlarmValidation.AlarmTypeEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.AlarmType.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateAlarmType(Model.AlarmType entity)
        {
            var validation = new AlarmValidation.AlarmTypeEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmType.Find(entity.Id);
                Entity.IsActive = entity.IsActive;
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;

            }
        }
        public bool DeleteAlarmType(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmType.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.AlarmType.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        #endregion

    }
}
