using FluentValidation;
using PhysicManagement.Logic.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
    public class AlarmEventTypeService
    {
        #region AlarmEventType
        public List<Model.AlarmEventType> GetAlarmEventTypeList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.AlarmEventType.OrderBy(x => x.Title).ToList();
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

        public bool AddAlarmEventType(Model.AlarmEventType entity)
        {
            var validation = new AlarmEventTypeValidation.
                             AlarmEventTypeEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.AlarmEventType.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateAlarmEventType(Model.AlarmEventType entity)
        {
            var validation = new AlarmEventTypeValidation.AlarmEventTypeEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmEventType.Find(entity.Id);
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;
                Entity.Title = entity.Title;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteAlarmEventType(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmEventType.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.AlarmEventType.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
