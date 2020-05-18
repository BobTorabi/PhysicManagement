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
        public HomeController()
        {
            PatientService = new Logic.Services.PatientService();
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
            return View();
        }
    }
}