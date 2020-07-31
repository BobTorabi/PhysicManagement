using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;


namespace PhysicManagement.Controllers
{
    public class TreatmentCategoryController : BaseController
    {
        Logic.Services.TreatmentCategoryService Service;
        public TreatmentCategoryController()
        {
            Service = new Logic.Services.TreatmentCategoryService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: TreatmentCategory
        public ActionResult List()
        {

            List<Model.TreatmentCategory> treatmentCategory = Service.GetTreatmentCategoryList();
            return View(treatmentCategory);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.TreatmentCategory());
            }
            else
            {
                Model.TreatmentCategory Entity = Service.GetTreatmentCategoryById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentCategory entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentCategory(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentCategory(entity);
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
            var TreatmentCategoryServices = Service.GetTreatmentCategoriesServiceByTreatmentCategoryId(id);
            if (TreatmentCategoryServices.Count == 0)
            {
                Service.DeleteTreatmentCategory(id);
                return RedirectToAction("List");
            }
            else {
                TempData["Error"] = "این دسته بندی زیرمجموعه دارد. ابتدا دسته بندی های زیرمجموعه را حذف کنید..";
                return RedirectToAction("List");
            }
        }

        public JsonResult getTreatmentCategoryServices(int id)
        {
            var Data =
                Service.GetTreatmentCategoryServiceByTreatmentCategoryId(id)
                .Select(x=>new {x.Id,x.Title,x.RelativeValue,x.Code }).ToList();

            return Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}