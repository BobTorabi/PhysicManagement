using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicTreatmentController : BaseController
    {
        readonly PhysicTreatmentService Service;
        public PhysicTreatmentController()
        {
            Service = new Logic.Services.PhysicTreatmentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
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
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePhysicTreatment(entity);
            }
            else
            {
                IsAffected = Service.AddPhysicTreatment(entity);
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
