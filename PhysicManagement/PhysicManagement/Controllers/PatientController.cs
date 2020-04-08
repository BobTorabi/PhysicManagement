﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PatientController : BaseController
    {
        Logic.Services.PatientService Service;
        public PatientController()
        {
            Service = new Logic.Services.PatientService();
        }
        // GET: Patient
        public ActionResult List()
        {

            List<Model.Patient> Patient = Service.GetPatientList();
            return View(Patient);
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Patient());
            }
            else
            {
                var Entity = Service.GetPatientById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Patient entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePatient(entity);
            }
            else
            {
                IsAffected = Service.AddPatient(entity);
            }
            if (IsAffected)
                return Redirect("Index");
            else
            {
                return View();
            }
        }


    }
}