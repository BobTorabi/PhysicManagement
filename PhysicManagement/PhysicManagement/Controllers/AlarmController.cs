using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AlarmController : BaseController
    {
        Logic.Services.AlarmService Service;
        
        public AlarmController()
        {
            Service = new Logic.Services.AlarmService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Alarm
        public ActionResult List()
        {
            List<Model.Alarm> Alarms = Service.GetAlarmList();
            return View(Alarms);
        }
        public ActionResult Modify(int? id)
        {
            Logic.Services.TreatmentService ts = new TreatmentService();
            if(id == null)
            {
                ViewBag.AlarmTypeId = new SelectList(Service.GetAlarmTypeList(), "Id", "Title");
                ViewBag.GenerateTreatmentPhaseId = new SelectList(ts.GetTreatmentList(), "Id", "PhaseNumber");
                return View(new Model.Alarm());
            }
            else
            {
                var Entity = Service.GetAlarmById(id.GetValueOrDefault());
                ViewBag.AlarmTypeId = new SelectList(Service.GetAlarmTypeList(), "Id", "Title",Entity.AlarmTypeId);
                ViewBag.GenerateTreatmentPhaseId = new SelectList(ts.GetTreatmentList(), "Id", "PhaseNumber",Entity.GenerateTreatmentPhaseId);
                return View(Entity);
            }
            
        }
        [HttpPost]
        public ActionResult Modify(Model.Alarm entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateAlarm(entity);
            }
            else
            {
                IsAffected = Service.AddAlarm(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken()]

        public ActionResult DeleteForm(long id)
        {
            var AlarmData = Service.GetAlarmById(id);
            Service.DeleteAlarm(AlarmData.Id);
            return RedirectToAction("List");
        }
    }
}