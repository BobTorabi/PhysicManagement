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
        public ActionResult List(string firstName, string lastName, string mobile, string degree)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            Logic.ViewModels.PagedList<Model.PhysicUser> PhysicUser =
                Service.GetPhysicUserList(firstName, lastName, mobile, degree, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = PhysicUser.TotalRecords;
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
                try
                {
                    Entity.Password = PhysicUserService.DecryptPassword(Entity.Username, Entity.Password);
                }
                catch { }
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