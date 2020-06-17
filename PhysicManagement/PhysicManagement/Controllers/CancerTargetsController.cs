using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class CancerTargetsController : BaseController
    {
        readonly CancerService Service;
        public CancerTargetsController()
        {
            Service = new Logic.Services.CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("list");
        }
        // GET: CancerTargets
        public ActionResult List(int? id)
        {
            List<Model.CancerTarget> CancerTargets = Service.GetCancerTargetList();
            if (id.HasValue)
            {
                CancerTargets = CancerTargets.Where(x => x.CancerId == id).ToList();
            }
            return View(CancerTargets);
        }
        public ActionResult Modify(int? id)
        {

            if (id == null)
            {
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title");
                return View(new Model.CancerTarget());
            }
            else
            {
                var Entity = Service.GetCancerTargetById(id.GetValueOrDefault());
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title", Entity.CancerId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.CancerTarget entity)
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
            return Json(new { location = "../../../CancerTargets/List/" + entity.CancerId });
        }
    }
}