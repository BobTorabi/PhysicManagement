using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;

namespace PhysicManagement.Controllers
{
    public class ContourController : Controller
    {
        Logic.Services.ContourService Service;
        public ContourController()
        {
            Service = new Logic.Services.ContourService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Contour
        public ActionResult List()
        {
            List<Model.Contour> Contour = Service.GetContourList();
            return View(Contour);
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Contour());
            }
            else
            {
                var Entity = Service.GetContourById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Contour entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateContour(entity);
            }
            else
            {
                IsAffected = Service.AddContour(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }

        public ActionResult ConfirmContour()
        {
            Logic.Services.CancerService cancer = new CancerService();
            ViewBag.cancerId = new SelectList(cancer.GetCancerList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmContour(string actionDate,string acceptDate,string acceptUser,int cancerId,string description)
        {
            return View();
        }
    }
}