using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class ContourDetailsController : BaseController
    {
        Logic.Services.ContourService Service;

        public ContourDetailsController()
        {
            Service = new ContourService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: ContourDetails
        public ActionResult List()
        {
            List<Model.ContourDetails> ContourDetails = Service.GetContourDetailsList();
            return View(ContourDetails);
        }
        public ActionResult Modify(int? id)
        {
            CancerService ts = new CancerService();
            if (id == null)
            {
                ViewBag.CancerOARId = new SelectList(ts.GetCancerOARList(), "Id", "OrganTitle");
                return View(new Model.ContourDetails());
            }
            else
            {
                var Entity = Service.GetContourDetailsById(id.GetValueOrDefault());
                ViewBag.CancerOARId = new SelectList(ts.GetCancerOARList(), "Id", "OrganTitle", Entity.CancerOARId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.ContourDetails entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateContourDetails(entity);
            }
            else
            {
                IsAffected = Service.AddContourDetails(entity);
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