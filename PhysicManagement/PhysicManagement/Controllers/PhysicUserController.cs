using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{



    public class PhysicUserController : BaseController
    {
        Logic.Services.PhysicUserService Service;
        public PhysicUserController()
        {
            Service = new Logic.Services.PhysicUserService();
        }
        // GET: PhysicUser
        public ActionResult List()
        {

            List<Model.PhysicUser> PhysicUser = Service.GetPhysicUserList();
            return View(PhysicUser);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.PhysicUser());
            }
            else
            {
                var Entity = Service.GetPhysicUserById(id.GetValueOrDefault());
                Entity.Password = Logic.Services.PhysicUserService.DecryptPassword(Entity.Username, Entity.Password);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.PhysicUser entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePhysicUser(entity);
            }
            else
            {
                IsAffected = Service.AddPhysicUser(entity);
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