using PhysicManagement.Logic.Services;
using PhysicManagement.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class ResidentController : BaseController
    {
        ResidentService Service;
        DoctorService DoctorService;
        AlarmService alarmService;
        public ResidentController()
        {
            Service = new ResidentService();
            DoctorService = new DoctorService();
            alarmService = new AlarmService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Resident
        public ActionResult List(string firstName, string lastName, string mobile)
        {

            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 10;
            Logic.ViewModels.PagedList<Model.Resident> Resident =
                Service.GetResidentList(firstName, lastName, mobile, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = Resident.TotalRecords;
            return View(Resident);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                ViewBag.DoctorId = new SelectList(DoctorService.GetIdNameFromDoctorList(), "Id", "Name");
                return View(new Model.Resident());
            }
            else
            {
                var Entity = Service.GetResidentById(id.GetValueOrDefault());
                ViewBag.DoctorId = new SelectList(DoctorService.GetIdNameFromDoctorList(), "Id", "Name", Entity.DoctorId);
                try
                {
                    Entity.Password = ResidentService.DecryptPassword(Entity.Username, Entity.Password);
                }
                catch { }
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Resident entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = ResidentService.UpdateResident(entity);
            }
            else
            {
                IsAffected = ResidentService.Register(entity);
            }
            if (IsAffected)
                return RedirectToAction("List");
            else
            {
                return View(entity);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteForm(int id)
        {
            var ResidentData = Service.GetResidentById(id);
            Service.DeleteResident(ResidentData.Id);
            return RedirectToAction("List");
        }

        #region Alarm
        public ActionResult ResidentAlarm(int? ResidentId)
        {
            if (ResidentId == null)
                return View(new Model.Resident());

            var ResidentEntity = Service.GetResidentById((int)ResidentId);
            var ResidentAlarmEntity = alarmService.GetResidentAlarmByResidentId(ResidentEntity.Id);

            ViewBag.ResidentName = ResidentEntity.FirstName + " " + ResidentEntity.LastName;
            return View(ResidentAlarmEntity);
        }


        [HttpPost]
        public ActionResult ResidentAlarm(ResidentAlarm entity)
        {
            bool result = alarmService.SetResidentAlarm(entity);
            return RedirectToAction("List", "Resident");
        }

        #endregion
    }
}
