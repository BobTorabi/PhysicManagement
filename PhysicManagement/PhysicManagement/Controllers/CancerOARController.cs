using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PhysicManagement.Logic.Services;

namespace PhysicManagement.Controllers
{
    public class CancerOARController : BaseController
    {
        Logic.Services.CancerService Service;
        public CancerOARController()
        {
            Service = new Logic.Services.CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: CancerOAR
        public ActionResult List()
        {
            List<Model.CancerOAR> CancerOAR = Service.GetCancerOARList();
            return View(CancerOAR);
        }
        public ActionResult Modify(int? id)
        {

            if (id == null)
            {
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title");
                return View(new Model.CancerOAR());
            }
            else
            {
                var Entity = Service.GetCancerOARById(id.GetValueOrDefault());
                ViewBag.CancerId = new SelectList(Service.GetCancerList(), "Id", "Title", Entity.CancerId);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.CancerOAR entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateCancerOAR(entity);
            }
            else
            {
                IsAffected = Service.AddCancerOAR(entity);
            }
            return RedirectToAction("List",
                new RouteValueDictionary(
                    new { 
                        controller = "CancerOAR", 
                        action = "List", 
                        Id = entity.CancerId }
                    ));

        }

        public ActionResult DeleteCancerOAR(int id)
        {
            var cancerOAR = Service.DeleteCancerOAR(id);
            return RedirectToAction("List");
        }

        public JsonResult GetCancerOARDataByCancerId(int cancerId,long? medicalRecordId) {
            var ORAList = Service.GetCancerOARListByCancerId(cancerId);
            if (medicalRecordId.HasValue)
            {
                var CountorData = new ContourService().GetContourByMedicalRecordId(medicalRecordId.Value);
                return Json(ORAList.Select(x => new {
                    x.OrganTitle,
                    x.Id,
                    x.Tolerance,
                    IsSelected = (
                    (CountorData == null || CountorData.ContourDetails == null || CountorData.ContourDetails.Count == 0)
                    ? false 
                    : CountorData.ContourDetails.Count(t=>t.CancerOARId == x.Id)>0
                    )
                }), JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(ORAList.Select(x => new
                {
                    x.OrganTitle,
                    x.Id,
                    x.Tolerance,
                    IsSelected = false
                }), JsonRequestBehavior.AllowGet); ;
            }
            
        }
    }
}