using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentProcessController : BaseController
    {
        Logic.Services.TreatmentService Service;
        public TreatmentProcessController()
        {
            Service = new Logic.Services.TreatmentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
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
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentProcess(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentProcess(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else {
                return View();
            }
        }
    }
}