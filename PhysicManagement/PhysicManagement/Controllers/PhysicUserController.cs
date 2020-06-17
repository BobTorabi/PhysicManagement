using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicUserController : BaseController
    {
        PhysicUserService Service;
        public PhysicUserController()
        {
            Service = new PhysicUserService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
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
        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteForm(int id)
        {
            var PhysicUserData = Service.GetPhysicUserById(id);
            Service.DeletePhysicUser(PhysicUserData.Id);
            return RedirectToAction("List");
        }
    }
}