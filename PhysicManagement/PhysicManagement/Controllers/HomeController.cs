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
        public HomeController()
        {
            PatientService = new Logic.Services.PatientService();
            ContourService = new Logic.Services.ContourService();
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
            return View();
        }
    }
}