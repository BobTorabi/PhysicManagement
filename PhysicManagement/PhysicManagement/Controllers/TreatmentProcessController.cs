using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentProcessController : Controller
    {
        Logic.Services.TreatmentService Service;
        public TreatmentProcessController()
        {
            Service = new Logic.Services.TreatmentService();
        }
        // GET: TreatmentProcess
        public ActionResult List()
        {

            List<Model.TreatmentProcess> TreatmentProcesses = Service.GetTreatmentProcessList();
            return View(TreatmentProcesses);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.TreatmentProcess());
            }
            else
            {
                var Entity = Service.GetTreatmentProcessById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentProcess entity)
        {
            return View();
        }
    }
}