using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PhysicManagement.Logic.Services
{
    public class AlarmService
    {
        #region Alarm Section
        public List<Model.Alarm> GetAlarmList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Alarm.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.Alarm GetAlarmById(long entityId)
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
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                //entity.
                //entity.GenerateUser = AlaramObject2.LastName;

                //Entity.IsActive = entity.IsActive;
                //Entity.IsOnBoard = entity.IsOnBoard;
                //Entity.ReviewDate = entity.ReviewDate;
                //Entity.ReviewText = entity.ReviewText;
                //Entity.ReviewUserName = entity.ReviewUserName;
                //Entity.AlarmTypeId = entity.AlarmTypeId;               
                //Entity.GenerateDate = entity.GenerateDate;
                //Entity.GenerateUser = entity.GenerateUser;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteAlarm(long entityId)
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
        
    }
}
