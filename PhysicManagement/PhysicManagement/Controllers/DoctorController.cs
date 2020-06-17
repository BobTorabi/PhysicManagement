﻿using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class DoctorController : BaseController
    {
        DoctorService Service;
        MedicalRecordService MedicalRecordService;
        public DoctorController()
        {
            Service = new DoctorService();
            MedicalRecordService = new MedicalRecordService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Doctor
        public ActionResult List()
        {

            List<Model.Doctor> Doctors = Service.GetDoctorList();
            return View(Doctors);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Doctor());
            }
            else
            {
                var Entity = Service.GetDoctorById(id.GetValueOrDefault());
                Entity.Password = DoctorService.DecryptPassword(Entity.Username, Entity.Password);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Doctor entity)
        {
            if (entity.Id > 0)
                Service.UpdateDoctor(entity);
            else
                Service.AddDoctor(entity);
            return RedirectToAction("List");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteForm(int id)
        {
            var DoctorData = Service.GetDoctorById(id);
            int IsDoctorUsedBefore = MedicalRecordService.GetTotalMedicalRecordsByDoctorId(DoctorData.Id);
            if (IsDoctorUsedBefore > 0)
            {
                TempData["Error"] = "این دکتر در سیستم بیمار دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else
            {
                 Service.DeleteDoctor(DoctorData.Id);
                return RedirectToAction("List");
            }
        }
    }
}