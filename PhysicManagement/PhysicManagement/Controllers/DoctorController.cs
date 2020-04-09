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
        public DoctorController()
        {
            Service = new Logic.Services.DoctorService();
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
            else {
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
            if (IsAffected)
                return RedirectToAction("List");
            else
            {
                return View();
            }
        }
    }
}