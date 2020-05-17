using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class DoctorController : BaseController
    {
        Logic.Services.DoctorService Service;
        Logic.Services.MedicalRecordService MedicalRecordService;
        public DoctorController()
        {
            Service = new Logic.Services.DoctorService();
            MedicalRecordService = new Logic.Services.MedicalRecordService();
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
                Entity.Password = Logic.Services.DoctorService.DecryptPassword(Entity.Username, Entity.Password);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Doctor entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateDoctor(entity);
            }
            else
            {
                IsAffected = Service.AddDoctor(entity);
            }
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