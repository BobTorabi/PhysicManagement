using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentPlanController : BaseController
    {
        PhysicTreatmentService Service;
        PhysicTreatmentPlanService planService;
        public PhysicTreatmentPlanController()
        {
            Service = new PhysicTreatmentService();
            planService = new PhysicTreatmentPlanService();
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
            CancerService cancer = new CancerService();
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

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(int id)
        {
            var PhysicTreatmentPlanData = Service.GetPhysicTreatmentById(id);
            int IsPhysicTreatmentPlanUsedBefore = planService.GetTotalPhysicTreatmentPlanHistoryByPhysicTreatmentPlanId(PhysicTreatmentPlanData.Id);
            if (IsPhysicTreatmentPlanUsedBefore > 0)
            {
                TempData["Error"] = "این برنامه درمان فیزیکی در سیستم بیمار دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else
            {
                Service.DeletePhysicTreatmentPlan(PhysicTreatmentPlanData.Id);
                return RedirectToAction("List");
            }
        }

    }
}