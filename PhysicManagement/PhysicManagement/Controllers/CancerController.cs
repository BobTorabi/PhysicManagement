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
        public CancerController()
        {
            Service = new Logic.Services.CancerService();
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
    }
}