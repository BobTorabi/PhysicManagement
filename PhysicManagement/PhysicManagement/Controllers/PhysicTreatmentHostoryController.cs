using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentHostoryController : BaseController
    {
        Logic.Services.PhysicTreatmentService Service;
        public PhysicTreatmentHostoryController()
        {
            Service = new Logic.Services.PhysicTreatmentService();
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
                return Redirect("Index");
            else
            {
                return View();
            }
        }
    }
}