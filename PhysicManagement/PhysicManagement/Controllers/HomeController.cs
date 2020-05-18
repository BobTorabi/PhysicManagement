using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class HomeController : BaseController
    {
        Logic.Services.PatientService PatientService;
        Logic.Services.ContourService ContourService;
        Logic.Services.MedicalRecordService MedicalRecordService;
        public HomeController()
        {
            PatientService = new Logic.Services.PatientService();
            ContourService = new Logic.Services.ContourService();
            MedicalRecordService = new Logic.Services.MedicalRecordService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Dashboard");
        }
        public ActionResult BaseInfo()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            ViewBag.PatientsStatistics =  PatientService.GetTotalPatientsStatistics();
            ViewBag.ContoursStatistics = ContourService.GetTotalContoursStatistics();
            ViewBag.CTCodesStatistics = MedicalRecordService.GetTotalCTCodesStatistics();
            ViewBag.TreatmentPlansStatistics = MedicalRecordService.GetTotalTreatmentPlansStatistics();

            return View();
        }
    }
}