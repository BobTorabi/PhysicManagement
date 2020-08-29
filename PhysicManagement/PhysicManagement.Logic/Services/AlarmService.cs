using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using PhysicManagement.Logic.Enums;

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

                Entity.AlarmEventTypeId = entity.AlarmEventTypeId;
                Entity.Body = entity.Body;
                Entity.DeliverDate = entity.DeliverDate;
                Entity.DoctorId = entity.DoctorId;
                Entity.IsArchived = entity.IsArchived;
                Entity.IsDelivered = entity.IsDelivered;
                Entity.IsSMS = entity.IsSMS;
                Entity.IsSystemAlarm = entity.IsSystemAlarm;
                Entity.SendDate = entity.SendDate;

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
        public List<Model.Alarm> GetUnreadAlarmListByUserType(UserType userType, int entityId)
        {
            if (entityId == 0)
                throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

            using (var db = new Model.PhysicManagementEntities())
            {
                switch (userType)
                {
                    case UserType.Doctor:
                        return db.Alarm.Where(x => x.DoctorId == entityId && x.IsDelivered == false).OrderBy(x => x.Id).ToList();
                    case UserType.Resident:
                        return db.Alarm.Where(x => x.ResidentId == entityId && x.IsDelivered == false).OrderBy(x => x.Id).ToList();
                    case UserType.Physist:
                        return db.Alarm.Where(x => x.PhysicUserId == entityId && x.IsDelivered == false).OrderBy(x => x.Id).ToList();
                    case UserType.User:
                        return db.Alarm.Where(x => x.UserId == entityId && x.IsDelivered == false).OrderBy(x => x.Id).ToList();
                    default:
                        return null;
                }
            }
        }
        #endregion

        #region DoctorAlarm

        public List<Model.DoctorAlarm> GetDoctorAlarmList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.DoctorAlarms.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.DoctorAlarm GetDoctorAlarmById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.DoctorAlarms.Find(entityId);
                return Entity;
            }
        }
        public bool AddDoctorAlarm(Model.DoctorAlarm entity)
        {
            var validation = new DoctorAlarmValidation.DoctorAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.DoctorAlarms.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateDoctorAlarm(Model.DoctorAlarm entity)
        {
            var validation = new DoctorAlarmValidation.DoctorAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.DoctorAlarms.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.DoctorId = entity.DoctorId;
                Entity.IsSmsRecieveActive = entity.IsSmsRecieveActive;
                Entity.ReplacementResidentId = entity.ReplacementResidentId;
                Entity.ChangeDate = DateTime.Now;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteDoctorAlarm(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.DoctorAlarms.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.DoctorAlarms.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        #endregion
    }
}
