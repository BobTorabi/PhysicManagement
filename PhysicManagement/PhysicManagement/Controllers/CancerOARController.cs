using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class CancerOARController : BaseController
    {
        CancerService Service;
        public CancerOARController()
        {
            Service = new CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: CancerOAR
        public ActionResult List(int? id)
        {
            List<Model.CancerOAR> CancerOAR = Service.GetCancerOARList();
            if (id.HasValue)
            {
                CancerOAR = CancerOAR.Where(x => x.CancerId == id).ToList();
            }
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
            if (entity.Id > 0)
            {
                Service.UpdateCancerOAR(entity);
            }
            else
            {
                Service.AddCancerOAR(entity);
            }
            return Json(new { location = "../../../CancerOAR/list/" + entity.CancerId },JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteCancerOAR(int id)
        {
            var CancerId =  Service.GetCancerOARById(id).CancerId;
            var cancerOAR = Service.DeleteCancerOAR(id);
            return Redirect("/CancerOAR/list/" + CancerId);
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