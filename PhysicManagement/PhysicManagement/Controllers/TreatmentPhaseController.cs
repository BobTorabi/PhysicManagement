using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentPhaseController : BaseController
    {
        Logic.Services.TreatmentService Service;
        public TreatmentPhaseController()
        {
            Service = new Logic.Services.TreatmentService();
        }
        // GET: TreatmentPhase
        public ActionResult List()
        {

            List<Model.TreatmentPhase> TreatmentPhase = Service.GetTreatmentPhasesList();
            return View(TreatmentPhase);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.TreatmentPhase());
            }
            else
            {
                var Entity = Service.GetTreatmentPhaseById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentPhase entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentPhase(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentPhase(entity);
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