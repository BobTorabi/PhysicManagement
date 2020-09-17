using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentCategoryServiceController : BaseController
    {
        Logic.Services.TreatmentCategoryService Service;
        public TreatmentCategoryServiceController()
        {
            Service = new Logic.Services.TreatmentCategoryService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: TreatmentCategoryService
        public ActionResult List(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "TreatmentCategory");
            }
            ViewBag.TreatmentCategory = Service.GetTreatmentCategoryById((int)id);
            List<Model.TreatmentCategoryService> treatmentCategoryService = Service.GetTreatmentCategoryServiceByTreatmentCategoryId((int)id);
            return View(treatmentCategoryService);
        }

        public ActionResult Modify(int? id, int? serviceId)
        {
            if (serviceId == null)
                return RedirectToAction("Index", "TreatmentCategory");
            
            var serviceObject = Service.GetTreatmentCategoryById((int)serviceId);
            ViewBag.TreatmentCategoryId = serviceObject.Id;

            if (id == null)
            {
                return View(new Model.TreatmentCategoryService());
            }

            var Entity = Service.GetTreatmentCategoryServiceById(id.GetValueOrDefault());

            return View(Entity);


        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentCategoryService entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
                IsAffected = Service.UpdateTreatmentCategoryService(entity);
            else
                IsAffected = Service.AddTreatmentCategoryService(entity);

            if (IsAffected)
                return RedirectToAction("List", "TreatmentCategoryService", new { @id = entity.TreatmentCategoryId });
            else
                return View();
            
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteForm(int id)
        {
            Service.DeleteTreatmentCategoryService(id);
            return RedirectToAction("List");
        }
    }
}