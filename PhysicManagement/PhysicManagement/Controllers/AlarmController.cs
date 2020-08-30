using PhysicManagement.Logic.Services;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
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
        public JsonResult SetAlarmAsRead(int id)
        {
            var AlarmData = Service.GetAlarmById(id);
            AlarmData.IsDelivered = true;
            bool IsDone = Service.UpdateAlarm(AlarmData);
            var Result = new MegaViewModel<bool>();
            if (IsDone)
            {
                Result.Data = true;
                Result.Status = MegaStatus.Successfull;
            }
            else
            {
                Result.Data = false;
                Result.Status = MegaStatus.Failed;
                Result.Messages.Add("خطا در خواندن پیام");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        #region SMSConfig
        public ActionResult SMSConfig()
        {
            return View(Service.GetAlarmConfigList());
        }

        [HttpPost]
        public ActionResult DoctorSMSConfig(int[] alarmEventTypeIds)
        {
            if (alarmEventTypeIds.Length > 0)
            {
                var allrecords = Service.GetAlarmConfigList();

                foreach (var item in allrecords)
                {
                    if (alarmEventTypeIds.Contains((int)item.AlarmEventTypeId))
                    {
                        item.SendDoctorSMS = true;
                        Service.UpdateAlarmConfig(item);
                    }
                    else
                    {
                        item.SendDoctorSMS = false;
                        Service.UpdateAlarmConfig(item);
                    }
                }
            }
            return View();
        }

        public ActionResult ResidentSMSConfig(int[] alarmEventTypeIds)
        {
            if (alarmEventTypeIds.Length > 0)
            {
                var allrecords = Service.GetAlarmConfigList();

                foreach (var item in allrecords)
                {
                    if (alarmEventTypeIds.Contains((int)item.AlarmEventTypeId))
                    {
                        item.SendResidentSMS = true;
                        Service.UpdateAlarmConfig(item);
                    }
                    else
                    {
                        item.SendResidentSMS = false;
                        Service.UpdateAlarmConfig(item);
                    }
                }
            }
            return View();
        }
        public ActionResult PhysistSMSConfig(int[] alarmEventTypeIds)
        {
            if (alarmEventTypeIds.Length > 0)
            {
                var allrecords = Service.GetAlarmConfigList();

                foreach (var item in allrecords)
                {
                    if (alarmEventTypeIds.Contains((int)item.AlarmEventTypeId))
                    {
                        item.SendPhysictSMS = true;
                        Service.UpdateAlarmConfig(item);
                    }
                    else
                    {
                        item.SendPhysictSMS = false;
                        Service.UpdateAlarmConfig(item);
                    }
                }
            }
            return View();
        }
        #endregion
    }
}