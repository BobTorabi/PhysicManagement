using PhysicManagement.Logic.Services;
using PhysicManagement.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class DoctorController : BaseController
    {
        DoctorService Service;
        AlarmService alarmService;
        MedicalRecordService MedicalRecordService;
        public DoctorController()
        {
            Service = new DoctorService();
            MedicalRecordService = new MedicalRecordService();
            alarmService = new AlarmService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: Doctor
        public ActionResult List(string firstName, string lastName, string mobile, string code)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 10;
            Logic.ViewModels.PagedList<Model.Doctor> Doctor =
                Service.GetDoctorList(firstName, lastName, mobile, code, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = Doctor.TotalRecords;
            return View(Doctor);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.Doctor());
            }
            else
            {
                var Entity = Service.GetDoctorById(id.GetValueOrDefault());
                try
                {
                    Entity.Password = DoctorService.DecryptPassword(Entity.Username, Entity.Password);
                }
                catch { }
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Doctor entity)
        {
            if (entity.Id > 0)
                Service.UpdateDoctor(entity);
            else
                Service.AddDoctor(entity);
            return RedirectToAction("List");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteForm(int id)
        {
            var DoctorData = Service.GetDoctorById(id);
            int IsDoctorUsedBefore = MedicalRecordService.GetTotalMedicalRecordsByDoctorId(DoctorData.Id);
            if (IsDoctorUsedBefore > 0)
            {
                TempData["Error"] = "این دکتر در سیستم بیمار دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else
            {
                Service.DeleteDoctor(DoctorData.Id);
                return RedirectToAction("List");
            }
        }

        #region Alarm
        public ActionResult DoctorAlarm(int? doctorId)
        {
            if (doctorId == null)
                return View(new Model.Doctor());
            
            var doctorEntity = Service.GetDoctorById((int)doctorId);

            var doctorAlarmEntity = alarmService.GetDoctorAlarmByDoctorId(doctorEntity.Id);
            var residentList = new ResidentService().GetIdNameFromResidentList();

            if (doctorAlarmEntity == null)
            {
                doctorAlarmEntity = new DoctorAlarm();
                doctorAlarmEntity.IsSmsRecieveActive = true;
                doctorAlarmEntity.DoctorId = doctorEntity.Id;
                ViewBag.ResidentList = new SelectList(new ResidentService().GetResidentList(), "Id", "LastName");
            }
            else
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var item in residentList)
                {
                    SelectListItem i = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = item.Id == doctorAlarmEntity.ReplacementResidentId ? true : false
                    };
                    list.Add(i);
                }

                ViewBag.ResidentList = list;
            }

            
            ViewBag.DoctorName = doctorEntity.FirstName + " " + doctorEntity.LastName;
            return View(doctorAlarmEntity);
        }


        [HttpPost]
        public ActionResult DoctorAlarm(DoctorAlarm entity)
        {
            bool result = alarmService.SetDoctorAlarm(entity);
            return RedirectToAction("List", "Doctor");
        }

        #endregion
    }
}