using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult List()
        {

            List<Model.TreatmentCategoryService> treatmentCategoryService = Service.GetTreatmentCategoryServiceList();
            return View(treatmentCategoryService);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                ViewBag.TreatmentCategoryId = new SelectList(Service.GetTreatmentCategoryList(), "Id", "Title");
                return View(new Model.TreatmentCategoryService());
            }
            else
            {

                ViewBag.TreatmentCategoryId = new SelectList(Service.GetTreatmentCategoryList(), "Id", "Title");
                var Entity = Service.GetTreatmentCategoryServiceById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentCategoryService entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentCategoryService(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentCategoryService(entity);
            }
            if (IsAffected)
                return RedirectToAction("List");
            else
            {
                return View();
            }
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