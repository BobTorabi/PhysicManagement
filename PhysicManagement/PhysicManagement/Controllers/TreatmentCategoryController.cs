using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                var Entity = Service.GetTreatmentCategoryById(id.GetValueOrDefault());
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
        public JsonResult getTreatmentCategoryServices(int id)
        {
            var Data =  
                Logic.Services
                .TreatmentCategoryService.GetTreatmentCategoryServiceByTreatmentCategoryId(id)
                .Select(x=>new {x.Id,x.Title,x.RelativeValue,x.Code }).ToList();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}