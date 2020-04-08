using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentPlanController : BaseController
    {
        Logic.Services.PhysicTreatmentPlanService Service;
        public PhysicTreatmentPlanController()
        {
            Service = new Logic.Services.PhysicTreatmentPlanService();
        }
        // GET: PhysicTreatmentPlan
        public ActionResult List()
        {

            List<Model.PhysicTreatmentPlan> PhysicTreatmentPlan = Service.GetPhysicTreatmentPlanList();
            return View(PhysicTreatmentPlan);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.PhysicTreatmentPlan());
            }
            else
            {
                var Entity = Service.GetPhysicTreatmentPlanById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.PhysicTreatmentPlan entity)
        {
            return View();
        }

    }
}