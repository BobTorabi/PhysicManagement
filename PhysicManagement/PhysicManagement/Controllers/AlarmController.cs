using PhysicManagement.Logic.Services;
using PhysicManagement.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AlarmController : BaseController
    {
        AlarmService Service;

        public AlarmController()
        {
            Service = new AlarmService();
        }

        #region BasicOperations
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
            if (id == null)
            {
                //ViewBag.AlarmTypeId = new SelectList(Service.GetAlarmTypeList(), "Id", "Title");
                //ViewBag.GenerateTreatmentPhaseId = new SelectList(ts.GetTreatmentList(), "Id", "PhaseNumber");
                return View(new Model.Alarm());
            }
            else
            {
                var Entity = Service.GetAlarmById(id.GetValueOrDefault());
                //ViewBag.AlarmTypeId = new SelectList(Service.GetAlarmTypeList(), "Id", "Title",Entity.AlarmTypeId);
                //ViewBag.GenerateTreatmentPhaseId = new SelectList(ts.GetTreatmentList(), "Id", "PhaseNumber",Entity.GenerateTreatmentPhaseId);
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
        #endregion
        public ActionResult Inbox()
        {
            var userData = AuthenticatedUserService.GetUserId();
            var userType = AuthenticatedUserService.GetUserType();
            var unreadAlarms = new AlarmService().GetUnreadAlarmListByUserType(userType, (int)userData.UserId);
            return View(unreadAlarms);
        }

        [HttpPost]
        public JsonResult SetAlarmAsRead(int id) {
            var AlarmData = Service.GetAlarmById(id);
            AlarmData.IsDelivered = true;
            bool IsDone = Service.UpdateAlarm(AlarmData);
            var Result = new MegaViewModel<bool>();
            if (IsDone)
            {
                Result.Data = true;
                Result.Status = MegaStatus.Successfull;
            }
            else {
                Result.Data = false;
                Result.Status = MegaStatus.Failed;
                Result.Messages.Add("خطا در خواندن پیام");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}