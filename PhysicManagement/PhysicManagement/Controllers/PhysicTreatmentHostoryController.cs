using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentHostoryController : BaseController
    {
        PhysicTreatmentService Service;
        public PhysicTreatmentHostoryController()
        {
            Service = new PhysicTreatmentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: PhysicTreatmentHostory
        public ActionResult List()
        {

            List<Model.PhysicTreatmentPlanHostory> physicTreatmentPlanHostory = Service.GetPhysicTreatmentPlanHostoryList();
            return View(physicTreatmentPlanHostory);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.PhysicTreatmentPlanHostory());
            }
            else
            {
                var Entity = Service.GetPhysicTreatmentPlanHostoryById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.PhysicTreatmentPlanHostory entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePhysicTreatmentPlanHostory(entity);
            }
            else
            {
                IsAffected = Service.AddPhysicTreatmentPlanHostory(entity);
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