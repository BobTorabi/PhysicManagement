using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class KFactorController : BaseController
    {
        Logic.Services.kFactorService Service;
        public KFactorController()
        {
            Service = new Logic.Services.kFactorService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: kFactor
        public ActionResult List()
        {

            List<Model.KFactor> kFactor = Service.GetkFactorList();
            return View(kFactor);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.KFactor());
            }
            else
            {
                var Entity = Service.GetkFactorById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.KFactor entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatekFactor(entity);
            }
            else
            {
                IsAffected = Service.AddkFactor(entity);
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