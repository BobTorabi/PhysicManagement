using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using PhysicManagement.Logic.Enums;
using PhysicManagement.Model;

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

        public Model.DoctorAlarm GetDoctorAlarmByDoctorId(int entityId)
        {
            DoctorAlarm doctorAlarm;
            using (var db = new Model.PhysicManagementEntities())
            {
                var doctor = db.Doctor.Find(entityId);
                if (doctor != null)
                    doctorAlarm = db.DoctorAlarms.Where(x => x.DoctorId == doctor.Id).FirstOrDefault();
                else
                    return null;
                return doctorAlarm;
            }
        }

        public bool AddDoctorAlarm(Model.DoctorAlarm entity)
        {
            var validation = new DoctorAlarmValidation.DoctorAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                entity.ChangeDate = DateTime.Now;
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

        public bool SetDoctorAlarm(DoctorAlarm doctorAlarm)
        {
            var retrievedDoctorAlarm = GetDoctorAlarmByDoctorId((int)doctorAlarm.DoctorId);
            if (retrievedDoctorAlarm == null)
                return AddDoctorAlarm(doctorAlarm);
            else
                doctorAlarm.Id = retrievedDoctorAlarm.Id;
                return UpdateDoctorAlarm(doctorAlarm);
        }

        #endregion

        #region ResidentAlarm

        public List<Model.ResidentAlarm> GetResidentAlarmList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.ResidentAlarms.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.ResidentAlarm GetResidentAlarmById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ResidentAlarms.Find(entityId);
                return Entity;
            }
        }

        public Model.ResidentAlarm GetResidentAlarmByResidentId(int entityId)
        {
            ResidentAlarm ResidentAlarm;
            using (var db = new Model.PhysicManagementEntities())
            {
                var Resident = db.Resident.Find(entityId);
                if (Resident != null)
                    ResidentAlarm = db.ResidentAlarms.Where(x => x.ResidentId == Resident.Id).FirstOrDefault();
                else
                    return null;
                return ResidentAlarm;
            }
        }

        public bool AddResidentAlarm(Model.ResidentAlarm entity)
        {
            var validation = new ResidentAlarmValidation.ResidentAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                entity.ChangeDate = DateTime.Now;
                db.ResidentAlarms.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateResidentAlarm(Model.ResidentAlarm entity)
        {
            var validation = new ResidentAlarmValidation.ResidentAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ResidentAlarms.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.ResidentId = entity.ResidentId;
                Entity.IsSmsRecieveActive = entity.IsSmsRecieveActive;
                Entity.ChangeDate = DateTime.Now;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteResidentAlarm(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.ResidentAlarms.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.ResidentAlarms.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        public bool SetResidentAlarm(ResidentAlarm ResidentAlarm)
        {
            var retrievedResidentAlarm = GetResidentAlarmByResidentId((int)ResidentAlarm.ResidentId);
            if (retrievedResidentAlarm == null)
                return AddResidentAlarm(ResidentAlarm);
            else
                ResidentAlarm.Id = retrievedResidentAlarm.Id;
            return UpdateResidentAlarm(ResidentAlarm);
        }

        #endregion

        #region PhysicUserAlarm

        public List<Model.PhysicUserAlarm> GetPhysicUserAlarmList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicUserAlarms.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.PhysicUserAlarm GetPhysicUserAlarmById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUserAlarms.Find(entityId);
                return Entity;
            }
        }

        public Model.PhysicUserAlarm GetPhysicUserAlarmByPhysicUserId(int entityId)
        {
            PhysicUserAlarm PhysicUserAlarm;
            using (var db = new Model.PhysicManagementEntities())
            {
                var PhysicUser = db.PhysicUser.Find(entityId);
                if (PhysicUser != null)
                    PhysicUserAlarm = db.PhysicUserAlarms.Where(x => x.PhysicUserId == PhysicUser.Id).FirstOrDefault();
                else
                    return null;
                return PhysicUserAlarm;
            }
        }

        public bool AddPhysicUserAlarm(Model.PhysicUserAlarm entity)
        {
            var validation = new PhysicUserAlarmValidation.PhysicUserAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                entity.ChangeDate = DateTime.Now;
                db.PhysicUserAlarms.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicUserAlarm(Model.PhysicUserAlarm entity)
        {
            var validation = new PhysicUserAlarmValidation.PhysicUserAlarmEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUserAlarms.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.PhysicUserId = entity.PhysicUserId;
                Entity.IsSmsRecieveActive = entity.IsSmsRecieveActive;
                Entity.ChangeDate = DateTime.Now;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicUserAlarm(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUserAlarms.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.PhysicUserAlarms.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        public bool SetPhysicUserAlarm(PhysicUserAlarm PhysicUserAlarm)
        {
            var retrievedPhysicUserAlarm = GetPhysicUserAlarmByPhysicUserId((int)PhysicUserAlarm.PhysicUserId);
            if (retrievedPhysicUserAlarm == null)
                return AddPhysicUserAlarm(PhysicUserAlarm);
            else
                PhysicUserAlarm.Id = retrievedPhysicUserAlarm.Id;
            return UpdatePhysicUserAlarm(PhysicUserAlarm);
        }

        #endregion


        #region AlarmConfig

        public List<Model.AlarmConfig> GetAlarmConfigList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.AlarmConfig.OrderBy(x => x.Id).ToList();
            }
        }

        public Model.AlarmConfig GetAlarmConfigById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmConfig.Find(entityId);
                return Entity;
            }
        }

        public bool AddAlarmConfig(Model.AlarmConfig entity)
        {
            var validation = new AlarmConfigValidation.AlarmConfigEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.AlarmConfig.Add(entity);
                return db.SaveChanges() == 1;
            }
        }

        public bool UpdateAlarmConfig(Model.AlarmConfig entity)
        {
            var validation = new AlarmConfigValidation.AlarmConfigEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmConfig.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                Entity.AlarmEventTypeId = entity.AlarmEventTypeId;
                Entity.SendDoctorSMS = entity.SendDoctorSMS;
                Entity.SendPhysictSMS = entity.SendPhysictSMS;
                Entity.SendAdminSMS = entity.SendAdminSMS;
                Entity.SendAggregateSMS = entity.SendAggregateSMS;
                Entity.SendResidentSMS = entity.SendResidentSMS;
                Entity.LastModifiedDate = DateTime.Now;
                
                return db.SaveChanges() == 1;
            }
        }

        public bool DeleteAlarmConfig(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmConfig.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.AlarmConfig.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        public Model.AlarmConfig GetAlarmConfigByEventTypeId(Enums.AlarmEventType alarmEventType)
        {
            int EventTypeId = (int)alarmEventType;
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmConfig.Where(x=>x.AlarmEventTypeId == EventTypeId).FirstOrDefault();
                return Entity;
            }
        }
        #endregion 

        #region AlarmEventType

        public List<Model.AlarmEventType> GetAlarmEventTypeList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.AlarmEventType.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.AlarmEventType GetAlarmEventTypeById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmEventType.Find(entityId);
                return Entity;
            }
        }

        #endregion
    }
}
