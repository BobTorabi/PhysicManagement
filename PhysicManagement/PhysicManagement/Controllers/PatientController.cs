using PhysicManagement.Logic.ViewModels;
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
        public ActionResult SetMedicalRecordPhases(long medicalRecordId) {
            return View();
        }
        public ActionResult PatientSearch(string firstName, string lastName, string mobile, string nationalCode, string caseCode)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            Logic.ViewModels.PagedList<Model.Patient> Patient =
                Service.GetPatientListWithFilters(firstName, lastName, mobile, nationalCode, caseCode, null, CurrentPage, ViewBag.PageSize);
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
            ViewBag.doctorId = new Logic.Services.DoctorService().GetIdNameFromDoctorList();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile)
        {
            var PatientObject = Service.RegisterPatient(patientFirstName, patientLastName, nationalCode, doctorId, mobile);
            return Json(new { location = "PatientInfo?patientId=" + PatientObject.Id }, JsonRequestBehavior.AllowGet);
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
        public JsonResult PatientInfoByMedicalRecordId(int medicalRecordId)
        {
            var Data = new Logic.Services.PatientService().GetPatientByMedicalRecordId(medicalRecordId);
            return Json(new MegaViewModel<PatientVMs.MedicalRecordDataWithPatientData>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PatientCTCode(string mriCode, string ctDescription, long medicalRecordId)
        {
            Logic.Services.MedicalRecordService medicalRecordService = new Logic.Services.MedicalRecordService();
            var PatientCTCode = medicalRecordService.AddMedicalRecordCTCode(mriCode, ctDescription, medicalRecordId);
            return Json(new { location = "PatientInfo?patientId=" + PatientCTCode.Id }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// لیست بیمارانی که اطلاعات ام ار ای یا سی تی اسکن آنها در سیستم ثبت نشده اشت.
        /// </summary>
        /// <returns></returns>
        public ActionResult PatientWithNoCTScanOrMRI()
        {
            List<Model.Patient> Patient = Service.GetPatientListDontHaveMriOrCTScan();
            return View(Patient);
        }
        [HttpPost]
        public ActionResult SetCTAndMIRDataForMedicalRecord(int medicalRecordId, string cTScanCode, string cTScanDescription, string mRICode)
        {
            var Data = Service.SetPatientMediacalRecordCTScanData(medicalRecordId, cTScanCode, cTScanDescription, mRICode);
            return Json(new MegaViewModel<bool>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientWithNoThreatmentPlan(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code)
        {
            List<Model.Patient> Patient = Service.GetPatientListWithUnsetFusion(firstName, lastName, mobile, nationalCode, systemCode, code);
            return View(Patient);
        }
        public ActionResult ListOfUnsetCountorsForCases(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code,bool? hasContour) {
            ViewBag.CancerList = new Logic.Services.CancerService().GetCancerList();

            List<Model.Patient> Patient = Service.GetPatientListWithUnsetCountor(firstName, lastName, mobile, nationalCode, systemCode, code, hasContour);
            return View(Patient);
        }
         public ActionResult SetPatientMediacalRecordCPAndFusion(int medicalRecordId, string TPDescription, bool needFusion)
        {
            var Data = Service.SetPatientMediacalRecordCPAndFusion(medicalRecordId,TPDescription,needFusion);
            return Json(new MegaViewModel<bool>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }
        
    }
}