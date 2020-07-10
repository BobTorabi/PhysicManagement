using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class ResidentController : BaseController
    {
        ResidentService Service;
        DoctorService DoctorService;
        public ResidentController()
        {
            Service = new ResidentService();
            DoctorService = new DoctorService();
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
                Entity.Password = ResidentService.DecryptPassword(Entity.Username, Entity.Password);
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
                return View();
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
    }
}
