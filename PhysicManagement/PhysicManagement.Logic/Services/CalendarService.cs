using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class CalendarService
    {
        #region Calendar section

        public List<Model.Calendar> GetCalendarList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Calendar.OrderBy(x => x.PatientFullName).ToList();
            }
        }
        public Model.Calendar GetCalendarById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Calendar.Find(entityId);
                return Entity;
            }
        }
        public bool AddCalendar(Model.Calendar entity)
        {
            var validtion = new CalendarValidation.CalendarEntityValidate().Validate(entity);
                if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

                using(var db = new Model.PhysicManagementEntities())
                {
                    db.Calendar.Add(entity);
                    return db.SaveChanges() == 1;
                }
        }
        public bool UpdateCalendar(Model.Calendar entity)
        {
            var validtion = new CalendarValidation.CalendarEntityValidate().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Calendar.Find(entity.Id);
                Entity.PatientFullName = entity.PatientFullName;
                Entity.DoctorFullName = entity.DoctorFullName;
                Entity.SessionNumber = entity.SessionNumber;
                Entity.PhysicTreatmentId = entity.PhysicTreatmentId;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteCalendar(Model.Calendar entityId)
        { 
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Calendar.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Calendar.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion

    }
}
