using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentController : Controller
    {
        Logic.Services.PhysicTreatmentService Service;
        public PhysicTreatmentController()
        {
            Service = new Logic.Services.PhysicTreatmentService();
        }
        // GET: PhysicTreatment
        public ActionResult List()
        {

            List<Model.PhysicTreatment> PhysicTreatment = Service.GetPhysicTreatmentList();
            return View(PhysicTreatment);
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.PhysicTreatment());
            }
            else
            {
                var Entity = Service.GetPhysicTreatmentById(id.GetValueOrDefault());
                return View(Entity);
            }
        }

        [HttpPost]
        public ActionResult Modify(Model.PhysicTreatment entity)
        {
            return View();
        }

    }


}
