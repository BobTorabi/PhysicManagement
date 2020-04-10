using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentPlanController : BaseController
    {
        Logic.Services.PhysicTreatmentService Service;
        public PhysicTreatmentPlanController()
        {
            Service = new Logic.Services.PhysicTreatmentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: PhysicTreatmentPlan
        public ActionResult List()
        {

            List<Model.PhysicTreatmentPlan> PhysicTreatmentPlan = Service.GetPhysicTreatmentPlanList();
            return View(PhysicTreatmentPlan);
        }

        public ActionResult Modify(int? id)
        {
            Logic.Services.CancerService cancer = new Logic.Services.CancerService();
            if (id == null)
            {
                ViewBag.PhysicTreatmentId = new SelectList(Service.GetPhysicTreatmentList(), "Id", "PhaseNumber");
                ViewBag.CancerOARId = new SelectList(cancer.GetCancerOARList(), "Id", "OrganTitle");
                return View(new Model.PhysicTreatmentPlan());
            }
            else
            {
                var Entity = Service.GetPhysicTreatmentPlanById(id.GetValueOrDefault());
                ViewBag.PhysicTreatmentId = new SelectList(Service.GetPhysicTreatmentList(), "Id", "PhaseNumber", Entity.PhysicTreatmentId);
                ViewBag.CancerOARId = new SelectList(cancer.GetCancerOARList(), "Id", "OrganTitle", Entity.CancerOARId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.PhysicTreatmentPlan entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePhysicTreatmentPlan(entity);
            }
            else
            {
                IsAffected = Service.AddPhysicTreatmentPlan(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View(entity);
            }
        }

    }
}