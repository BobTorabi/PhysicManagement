using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class CancerController : BaseController
    {
        Logic.Services.CancerService Service;
        Logic.Services.MedicalRecordService MedicalService;
        public CancerController()
        {
            Service = new Logic.Services.CancerService();
            MedicalService = new MedicalRecordService();
        }
        
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Cancer
        public ActionResult List()
        {

            List<Model.Cancer> Cancer = Service.GetCancerList();
            return View(Cancer);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Cancer());
            }
            else
            {
                var Entity = Service.GetCancerById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Cancer entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateCancer(entity);
            }
            else
            {
                IsAffected = Service.AddCancer(entity);
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
            var CancerData = Service.GetCancerById(id);
            int IsCancerUsedBefore = MedicalService.GetTotalMedicalRecordsByCancerId(CancerData.Id);
            int IsCancerUsedBeforeInCancerOAR = Service.GetCancerOARByCancerId(CancerData.Id);
            int IsCancerUsedBeforeInCancertargets = Service.GetCancerTargetsByCancerId(CancerData.Id);
            if (IsCancerUsedBefore > 0)
            {
                TempData["Error"] = "این سرطان در سیستم بیمار دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else if (IsCancerUsedBeforeInCancerOAR > 0)
            {
                TempData["Error"] = "این سرطان در سیستم ارگان های هدف وجود دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else if (IsCancerUsedBeforeInCancertargets > 0)
            {
                TempData["Error"] = "این سرطان در سیستم ارگان های در خطر وجود دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else
            {
                Service.DeleteCancer(CancerData.Id);
                return RedirectToAction("List");
            }

        }

        public JsonResult GetCancerOARAndTargets(int cancerId) {
            var CancertOARs = Service.GetCancerOARListByCancerId(cancerId).Select(x=>new {x.Id,x.OrganTitle,x.CancerId,x.Description });
            var CancertTargets = Service.GetCancerTargetListByCancerId(cancerId).Select(x=>new {x.Id,x.Title,x.Optimum,x.Description });
            return Json(new { OARs = CancertOARs, Targets = CancertTargets }, JsonRequestBehavior.AllowGet);
        }
    }
}