using PhysicManagement.Logic.Services;
using PhysicManagement.Model;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PhysicUserController : BaseController
    {
        PhysicUserService Service;
        AlarmService alarmService;
        public PhysicUserController()
        {
            Service = new PhysicUserService();
            alarmService = new AlarmService();
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

        #region Alarm
        public ActionResult PhysicUserAlarm(int? PhysicUserId)
        {
            if (PhysicUserId == null)
                return View(new Model.PhysicUser());

            var PhysicUserEntity = Service.GetPhysicUserById((int)PhysicUserId);
            var PhysicUserAlarmEntity = alarmService.GetPhysicUserAlarmByPhysicUserId(PhysicUserEntity.Id);

            ViewBag.PhysicUserName = PhysicUserEntity.FirstName + " " + PhysicUserEntity.LastName;
            return View(PhysicUserAlarmEntity);
        }


        [HttpPost]
        public ActionResult PhysicUserAlarm(PhysicUserAlarm entity)
        {
            bool result = alarmService.SetPhysicUserAlarm(entity);
            return RedirectToAction("List", "PhysicUser");
        }

        #endregion
    }
}