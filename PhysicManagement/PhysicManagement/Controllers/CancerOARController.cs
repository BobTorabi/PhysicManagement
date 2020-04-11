using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;

namespace PhysicManagement.Controllers
{
    public class CancerOARController : BaseController
    {
        Logic.Services.CancerService Service;
        public CancerOARController()
        {
            Service = new Logic.Services.CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: CancerOAR
        public ActionResult List()
        {
            List<Model.CancerOAR> CancerOAR = Service.GetCancerOARList();
            return View(CancerOAR);
        }
        public ActionResult Modify(int? id)
        {
            
            if (id == null)
            {
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title");
                return View(new Model.CancerOAR());
            }
            else
            {
                var Entity = Service.GetCancerOARById(id.GetValueOrDefault());
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title",Entity.CancerId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.CancerOAR entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateCancerOAR(entity);
            }
            else
            {
                IsAffected = Service.AddCancerOAR(entity);
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