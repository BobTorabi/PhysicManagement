using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;

namespace PhysicManagement.Controllers
{
    public class CancerTargetsController : BaseController
    {
        Logic.Services.CancerService Service;
        public CancerTargetsController()
        {
            Service = new Logic.Services.CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("list");
        }
        // GET: CancerTargets
        public ActionResult List()
        {
            List<Model.CancerTargets> CancerTargets = Service.GetCancerTargetList();
            return View(CancerTargets);
        }
        public ActionResult Modify(int? id)
        {

            if (id == null)
            {
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title");
                return View(new Model.CancerTargets());
            }
            else
            {
                var Entity = Service.GetCancerTargetById(id.GetValueOrDefault());
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title", Entity.CancerId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.CancerTargets entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateCancerTarget(entity);
            }
            else
            {
                IsAffected = Service.AddCancerTarget(entity);
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