using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AlarmController : Controller
    {
        Logic.Services.AlarmService Service;
        public AlarmController()
        {
            Service = new Logic.Services.AlarmService();
        }
        // GET: Alarm
        public ActionResult List()
        {
            List<Model.Alarm> Alarms = Service.GetAlarmList();
            return View(Alarms);
        }
    }
}