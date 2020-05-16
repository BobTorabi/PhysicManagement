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
        Logic.Services.MedicalRecordService MedicalService;
        Logic.Services.PhysicTreatmentService PhysicTreatmentService;

        public PatientController()
        {
            Service = new Logic.Services.PatientService();
            MedicalService = new Logic.Services.MedicalRecordService();
            PhysicTreatmentService = new Logic.Services.PhysicTreatmentService();
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
        public ActionResult SetMedicalRecordPhases(long medicalRecordId)
        {
            var ModelData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Data = Service.GetPatientById(ModelData.PatientId);
            ViewBag.PatientData = Data;

            return View(ModelData);
        }
        /// <summary>
        /// فرم شماره 6 - ثبات
        /// ثبت اطلاعات تعداد فازهای یک پرونده پزشکی
        /// </summary>
        /// <param name="medicalRecordId"></param>
        /// <param name="Phases"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetMedicalRecordPhases(long medicalRecordId, int Phases)
        {
            var UserData = Logic.Services.AuthenticatedUserService.GetUserId();

            MedicalService.UpdateMedicalRecordForPhaseCount(medicalRecordId, Phases);
            for (int i = 1; i <= Phases; i++)
            {
                PhysicTreatmentService.AddPhysicTreatment(new Model.PhysicTreatment
                {
                    ActionDate = null,
                    ActionUser = UserData.UserId.ToString(),
                    MedicalRecordId = medicalRecordId,
                    PhaseNumber = i
                });

            }
            return Json(new { location = "../PhysicTreatment/" },JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// فرم شماره 7
        /// تجویز درمان بر روی تعداد فازهای تعیین شده ی درمان
        /// </summary>
        /// <param name="medicalRecordId">شناسه پرونده پزشکی یک بیمار</param>
        /// <returns></returns>
        public ActionResult SetMedicalRecordTreatmentPhase(long medicalRecordId) {
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var PatientData = Service.GetPatientById(MedicalRecordData.PatientId);
            var ViewData = PhysicTreatmentService.GetPhysicTreatmentByMedicalRecordId(medicalRecordId);
            ViewBag.MedicalRecordData = MedicalRecordData;
            ViewBag.PatientData = PatientData;
            return View(ViewData);
        }

        [HttpPost]
        public ActionResult SetMedicalRecordTreatmentPhase(long medicalRecordId,string Data) {

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
        public ActionResult RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile,string description)
        {
            var PatientObject = Service.RegisterPatient(patientFirstName, patientLastName, nationalCode, doctorId, mobile,description);
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
        public ActionResult ListOfUnsetCountorsForCases(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code, bool? hasContour)
        {
            ViewBag.CancerList = new Logic.Services.CancerService().GetCancerList();

            List<Model.Patient> Patient = Service.GetPatientListWithUnsetCountor(firstName, lastName, mobile, nationalCode, systemCode, code, hasContour);
            return View(Patient);
        }
        public ActionResult SetPatientMediacalRecordCPAndFusion(int medicalRecordId, string TPDescription, bool needFusion)
        {
            var Data = Service.SetPatientMediacalRecordCPAndFusion(medicalRecordId, TPDescription, needFusion);
            return Json(new MegaViewModel<bool>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }

    }
}