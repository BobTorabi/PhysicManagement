﻿using System;
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
        // GET: Alarm
        public ActionResult List()
        {
            List<Model.Alarm> Alarms = Service.GetAlarmList();
            return View(Alarms);
        }
        public ActionResult Modify(int? id)
        {
            if(id == null)
            {
                return View(new Model.Alarm());
            }
            else
            {
                var Entity = Service.GetAlarmById(id.GetValueOrDefault());
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
    }
}