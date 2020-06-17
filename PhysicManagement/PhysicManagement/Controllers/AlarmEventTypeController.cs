using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AlarmEventTypeController : BaseController
    {
        readonly AlarmEventTypeService Service;
        public AlarmEventTypeController()
        {
            Service = new AlarmEventTypeService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: AlarmEventType
        public ActionResult List()
        {

            List<Model.AlarmEventType> alarmEventType = Service.GetAlarmEventTypeList();
            return View(alarmEventType);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.AlarmEventType());
            }
            else
            {
                var Entity = Service.GetAlarmEventTypeById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.AlarmEventType entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateAlarmEventType(entity);
            }
            else
            {
                IsAffected = Service.AddAlarmEventType(entity);
            }
            if (IsAffected)
                return RedirectToAction("List");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteForm(int id)
        {
            var AlarmEventTypeData = Service.GetAlarmEventTypeById(id);
            Service.DeleteAlarmEventType(AlarmEventTypeData.Id);
            return RedirectToAction("List");
        }

    }
}