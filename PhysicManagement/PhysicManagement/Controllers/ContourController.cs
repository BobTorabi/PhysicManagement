using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}