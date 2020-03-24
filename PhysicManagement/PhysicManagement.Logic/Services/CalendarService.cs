using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class CalendarService
    {
        #region Calendar section

        public List<Model.Calendar> GetCalendarList()
        { 
            using(var db = new Model.PhysicManagementEntities())
            {
                return db.Calendar.OrderBy(x => x.PatientFullName).ToList();
            }
        }
        public Model.Calendar GetCalendarById(int entityId)
        {
            using(var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Calendar.Find(entityId);
                return Entity;
            }
        }
        public bool AddCalendar(Model.Calendar entity)
        {
              
        }
        public bool UpdateCalendar(Model.Calendar entity)
        { }
        public bool DeleteCalendar(Model.Calendar entityId)
        { }
        #endregion

    }
}
