using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class UserController : BaseController
    {
        readonly UserService Service;
        public UserController()
        {
            Service = new UserService();
        }
        
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }

        // GET: User
        public ActionResult List()
        {

            List<Model.User> Users = Service.GetUserList();
            return View(Users);
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.User());
            }
            else
            {
                var Entity = Service.GetUserById(id.GetValueOrDefault());
                Entity.Password = UserService.DecryptPassword(Entity.Username, Entity.Password);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.User entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateUser(entity);
            }
            else
            {
                IsAffected = Service.AddUser(entity);
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
            var UserData = Service.GetUserById(id);
            Service.DeleteUser(UserData.Id);
            return RedirectToAction("List");
        }
    }
}