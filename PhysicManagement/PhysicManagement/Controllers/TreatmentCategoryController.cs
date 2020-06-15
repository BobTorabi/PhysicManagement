using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentCategoryController : Controller
    {
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