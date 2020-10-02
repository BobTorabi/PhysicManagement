using PhysicManagement.Logic.Services;
using System;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class HomeController : BaseController
    {
        PatientService PatientService;
        ContourService ContourService;
        MedicalRecordService MedicalRecordService;
        TreatmentService TreatmentService;
        TreatmentCategoryService TreatmentCategoryService;
        public HomeController()
        {
            //SMSWebService.SendSMS("09122863082", "نمونه پیامک تستی");
            PatientService = new PatientService();
            ContourService = new ContourService();
            MedicalRecordService = new MedicalRecordService();
            TreatmentService = new TreatmentService();
            TreatmentCategoryService = new TreatmentCategoryService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Dashboard");
        }
        public ActionResult BaseInfo()
        {
            return View();
        }
        public ActionResult Calculate()
        {
            string JYear = Common.DateUtility.GetPersianYear(DateTime.Now);

            ViewBag.kFactor = TreatmentCategoryService.GetKFaktorByYear(JYear);
            ViewBag.TreatmentCategory = TreatmentCategoryService.GetAllTreatmentCategory();

            return View();
        }
        public ActionResult CalculatePrint(int techKFactor, int profKFactor, int tcId, string name, int percent)
        {
            ViewBag.TreatmentCategoryTitle = TreatmentCategoryService.GetTreatmentCategoryById(tcId).Title;
            ViewBag.TechnicalKfactor = techKFactor;
            ViewBag.ProfessionalKFactor = profKFactor;
            ViewBag.Name = name;
            ViewBag.Percent = percent;
            return View();
        }
        public ActionResult Dashboard()
        {
            ViewBag.PatientsStatistics = PatientService.GetTotalPatientsStatistics();
            ViewBag.ContoursStatistics = MedicalRecordService.GetTotalContoursStatistics();
            ViewBag.CTCodesStatistics = MedicalRecordService.GetTotalCTCodesStatistics();
            ViewBag.TreatmentPlansStatistics = MedicalRecordService.GetTotalTreatmentPlansStatistics();
            ViewBag.PatientReceptionStatistics = PatientService.GetPatientsReceptionStatistics();
            ViewBag.ConformContourStatistics = MedicalRecordService.GetTotalConformContoursStatistics();
            ViewBag.TreatmentPhaseStatistics = TreatmentService.GetTotalTreatmentPhaseStatistics();
            ViewBag.ConformTreatmentPhaseStatistics = TreatmentService.GetTotalConformTreatmentPhaseStatistics();
            return View();
        }
    }
}