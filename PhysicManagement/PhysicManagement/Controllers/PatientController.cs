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
            return View();
        }
        // GET: Patient
        public ActionResult List()
        {

            List<Model.Patient> Patient = Service.GetPatientList();
            return View(Patient);
        }
        public ActionResult PatientSearch(string firstName, string lastName, string mobile, string nationalCode, string caseCode)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            Logic.ViewModels.PagedList<Model.Patient> Patient = Service.GetPatientListWithFilters(firstName, lastName, mobile, nationalCode, caseCode, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = Patient.TotalRecords;
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
            ViewBag.doctorId = new SelectList(new Logic.Services.DoctorService().GetIdNameFromDoctorList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile)
        {
            string PatientId = Service.RegisterPatient(patientFirstName, patientLastName, nationalCode, doctorId, mobile);
            return Json(new { location = "PatientInfo?patientId=" + PatientId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PatientInfo(long patientId)
        {
            var PatientData = Service.GetPatientById(patientId);
            return View(PatientData);
        }
        public ActionResult PatientCTCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientCTCode(string mricode, string ctdescription,int patientid)
        {
            var PatientCTCode = Service.AddPatientCTCode(mricode, ctdescription,patientid);
            return Json(new { location = "PatientInfo?patientId=" + PatientCTCode }, JsonRequestBehavior.AllowGet);
        }
    }
}