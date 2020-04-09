using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class ResidentController : BaseController
    {
        Logic.Services.ResidentService Service;
        public ResidentController()
        {
            Service = new Logic.Services.ResidentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Resident
        public ActionResult List()
        {

            List<Model.Resident> Resident = Service.GetResidentList();
            return View(Resident);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Resident());
            }
            else
            {
                var Entity = Service.GetResidentById(id.GetValueOrDefault());
                Entity.Password = Logic.Services.ResidentService.DecryptPassword(Entity.Username, Entity.Password);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Resident entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateResident(entity);
            }
            else
            {
                IsAffected = Service.AddResident(entity);
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
