using System;
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
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
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
                ViewBag.GenderIsMale = new SelectList(Service.GetPatientList());
                return View(new Model.Patient());
            }
            else
            {  
                var Entity = Service.GetPatientById(id.GetValueOrDefault());
                ViewBag.GenderIsMale = new SelectList(Service.GetPatientList(), "Id", "GenderIsMale", Entity.GenderIsMale);
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
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }

        public ActionResult RegisterPatient()
        {
            ViewBag.Id = new SelectList(new Logic.Services.DoctorService().GetDoctorList(), "Id", "LastName");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile,string code)
        {
            Service.RegisterPatient(patientFirstName, patientLastName, nationalCode, doctorId, mobile,code);
            return View();
        }

        public ActionResult PatientSearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientSearch(string info)
        {
            Service.PatientSearch(info);
            return View();
        }
    }
}