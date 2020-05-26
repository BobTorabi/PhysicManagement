using System.Collections.Generic;
using System.Web.Mvc;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Model;

namespace PhysicManagement.Controllers
{
    public class TreatmentPhaseController : BaseController
    {
        Logic.Services.TreatmentService Service;
        public TreatmentPhaseController()
        {
            Service = new Logic.Services.TreatmentService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: TreatmentPhase
        public ActionResult List(string firstName, string lastName, string mobile,
            string nationalCode, string systemCode, string code)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            PagedList<Model.TreatmentPhase> MedicalRecord = Service.GetTreatmentPhasesList(firstName, lastName, mobile,
                nationalCode, systemCode, code, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = MedicalRecord.TotalRecords;
            return View(MedicalRecord);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.TreatmentPhase());
            }
            else
            {
                var Entity = Service.GetTreatmentPhaseById(id.GetValueOrDefault());
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentPhase entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentPhase(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentPhase(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }
    }
}