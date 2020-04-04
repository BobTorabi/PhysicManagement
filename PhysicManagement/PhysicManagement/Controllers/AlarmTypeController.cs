using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AlarmTypeController : Controller
    {
        Logic.Services.AlarmService Service;
        public AlarmTypeController()
        {
            Service = new Logic.Services.AlarmService();
        }
        // GET: AlarmType
        public ActionResult List()
        {

            List<Model.AlarmType> AlarmTypes = Service.GetAlarmTypeList();
            return View(AlarmTypes);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.AlarmType());
            }
            else
            {
                var Entity = Service.GetAlarmTypeById(id.GetValueOrDefault());
                return View(Entity);
            }

        }

        [HttpPost]
        public ActionResult Modify(Model.AlarmType entity)
        {
            return View();
        }


    }
}