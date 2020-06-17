using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class HomeController : BaseController
    {
        PatientService PatientService;
        ContourService ContourService;
        MedicalRecordService MedicalRecordService;
        public HomeController()
        {
            PatientService = new PatientService();
            ContourService = new ContourService();
            MedicalRecordService = new MedicalRecordService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Dashboard");
        }
        public ActionResult BaseInfo()
        {
            return View();
        }
        public ActionResult Calculate() {
            string JYear = Common.DateUtility.GetPersianYear(DateTime.Now);
            
            ViewBag.kFactor = TreatmentCategoryService.GetKFaktorByYear(JYear);
            ViewBag.TreatmentCategory = TreatmentCategoryService.GetAllTreatmentCategory();

            return View();
        }
       
        public ActionResult Dashboard()
        {
            ViewBag.PatientsStatistics =  PatientService.GetTotalPatientsStatistics();
            ViewBag.ContoursStatistics = MedicalRecordService.GetTotalContoursStatistics();
            ViewBag.CTCodesStatistics = MedicalRecordService.GetTotalCTCodesStatistics();
            ViewBag.TreatmentPlansStatistics = MedicalRecordService.GetTotalTreatmentPlansStatistics();
            ViewBag.PatientReceptionStatistics = PatientService.GetPatientsReceptionStatistics();
            ViewBag.ConformContourStatistics = MedicalRecordService.GetTotalConformContoursStatistics();
            return View();
        }
    }
}