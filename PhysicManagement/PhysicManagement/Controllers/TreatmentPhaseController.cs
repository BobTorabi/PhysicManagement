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
        readonly ContourService ContourService;
        public TreatmentPhaseController()
        {
            Service = new TreatmentService();
            MedicalService = new MedicalRecordService();
            CancerService = new CancerService();
            ContourService = new ContourService();
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
                Service.GetTreatmentPhasesListForDoctor
                (firstName, lastName, mobile, nationalCode, systemCode, code, CurrentPage, ViewBag.PageSize);

            ViewBag.TotalRecords = MedicalRecord == null ? 100 : MedicalRecord.TotalRecords;
            return View(MedicalRecord);
        }

        public ActionResult ListForPhysicist(string firstName, string lastName, string mobile, string nationalCode, string systemCode, string code)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 25;
            PagedList<Model.MedicalRecord> MedicalRecord =
                Service.GetTreatmentPhasesListForPhysist
                (firstName, lastName, mobile, nationalCode, systemCode, code, CurrentPage, ViewBag.PageSize);

            ViewBag.TotalRecords = MedicalRecord == null ? 100 : MedicalRecord.TotalRecords;
            return View(MedicalRecord);
        }
        public ActionResult SetPhaseAsPlanned(long medicalRecordId)
        {
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();
            
            ViewBag.MedicalRecordId = medicalRecordId;
            ViewBag.MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            ViewBag.ContourDetailList = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId);
            ViewBag.CancerTargetList = CancerService.GetCancerTargetList();
            ViewBag.CancerOARList = CancerService.GetCancerOARList();

            return View(Phases);
        }

        public ActionResult SetPhysicPlan(long medicalRecordId)
        {
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();

            ViewBag.MedicalRecordId = medicalRecordId;
            ViewBag.MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            ViewBag.ContourDetailList = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId);
            ViewBag.CancerTargetList = CancerService.GetCancerTargetList();
            ViewBag.CancerOARList = CancerService.GetCancerOARList();

            return View(Phases);
        }



        public ActionResult SetPhaseAsApproved(long medicalRecordId)
        {
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();

            ViewBag.MedicalRecordId = medicalRecordId;
            ViewBag.MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            ViewBag.ContourDetailList = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId);
            ViewBag.CancerTargetList = CancerService.GetCancerTargetList();
            ViewBag.CancerOARList = CancerService.GetCancerOARList();

            return View(Phases);
        }

        [HttpPost]
        public JsonResult SetPhaseAsApprovedByPhysicst(long medicalRecordId) {

            var UserData = Logic.Services.AuthenticatedUserService.GetUserId();
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();
            var PhaseIds = Phases.Select(x => x.Id).ToArray();
            var PhaseDetails = Service.GetTreatmentPhaseDetatilssByPhaseIds(PhaseIds);
            foreach (var Phase in Phases)
            {
                Phase.IsApproved = true;
                Service.UpdateTreatmentPhase(Phase);

            }
            foreach (var PHD in PhaseDetails)
            {
                PHD.MedicalRecordId = medicalRecordId;
                PHD.PhysicPlanHasAccepted = true;
                PHD.PhysicUserFullName = UserData.FullName;
                PHD.PresciptionHasApproved = true;
                Service.UpdateTreatmentPhaseDetail(PHD);
            }

            return Json(new { location = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[Authorization(Roles = "doctor")]
        public ActionResult SetPhaseAsPlanned(List<SetPhaseDetail> Data, List<PlanFieldAndComment> FieldCommentData, long medicalRecordId)
        {
            var UserData = Logic.Services.AuthenticatedUserService.GetUserId();
            ViewBag.MedicalRecordId = medicalRecordId;
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Phases = Service.GetTreatmentPhasesByMedicalRecordId(medicalRecordId).OrderByDescending(x => x.PhaseNumber).ToList();
            var PhaseDetails = Service.GetTreatmentPhaseDetatilssByMedicalRecordId(medicalRecordId);
            foreach (var Phase in Phases)
            {
                Phase.IsPrescribedByDoctor = true;
                Phase.PrescribesdUserId = UserData.UserId.ToString();
                Phase.PrescribedUserRole = UserData.RoleName;
                Phase.PrescribesdUserFullName = UserData.FullName;
                Service.UpdateTreatmentPhase(Phase);

            }
            foreach (var item in Data)
            {
                var PHD = PhaseDetails.Where(x => x.Id == item.PhaseDetailId).FirstOrDefault();
                PHD.PlannedDose = item.PlannedDose;
                PHD.Evaluation = item.Evaluation;
                PHD.HadContour = true;
                Service.UpdateTreatmentPhaseDetail(PHD);
            }

            return Json(new { location = ""},JsonRequestBehavior.AllowGet);
        }
    }
}