using PhysicManagement.Logic.Services;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentPhaseController : BaseController
    {
        readonly TreatmentService Service;
        readonly MedicalRecordService MedicalService;
        readonly CancerService CancerService;
        public TreatmentPhaseController()
        {
            Service = new TreatmentService();
            MedicalService = new MedicalRecordService();
            CancerService = new CancerService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: TreatmentPhase
        public ActionResult List(string firstName, string lastName, string mobile, string nationalCode, string systemCode, string code)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 25;
            PagedList<Model.MedicalRecord> MedicalRecord = 
                Service.GetTreatmentPhasesList
                (firstName, lastName, mobile, nationalCode, systemCode, code, CurrentPage, ViewBag.PageSize);

            ViewBag.TotalRecords = MedicalRecord == null ? 100 : MedicalRecord.TotalRecords;
            return View(MedicalRecord);
        }
        public ActionResult SetPhaseAsPlanned(long medicalRecordId)
        {
            ViewBag.MedicalRecordId = medicalRecordId;
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x=>x.PhaseNumber).ToList();
            var PhaseDetails = Service.GetTreatmentPhaseDetatilssByMedicalRecordId(medicalRecordId);
            ViewBag.CancerOARList = CancerService.GetCancerOARListByCancerId(MedicalRecordData.CancerId.GetValueOrDefault());
            ViewBag.CancerOARData = PhaseDetails.Where(t => t.CancerOARId != null).Select(t => t.CancerOARId.Value).ToArray();
            ViewBag.PhaseDetails = PhaseDetails;
            return View(Phases);
        }

        [HttpPost]
        [Authorization(Roles = "doctor")]
        public ActionResult SetPhaseAsPlanned(List<SetPhaseDetail> Data, long medicalRecordId) {
            ViewBag.MedicalRecordId = medicalRecordId;
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();
            var PhaseDetails = Service.GetTreatmentPhaseDetatilssByMedicalRecordId(medicalRecordId);
            foreach (var PHD in PhaseDetails)
            {

            }
            return View(Phases);
        }
    }
}