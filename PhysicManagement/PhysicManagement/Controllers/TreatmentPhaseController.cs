using PhysicManagement.Logic.Services;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Models;
using System;
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
        readonly PhysicTreatmentPlanService physicTreatmentPlanService;
        public TreatmentPhaseController()
        {
            Service = new TreatmentService();
            MedicalService = new MedicalRecordService();
            CancerService = new CancerService();
            ContourService = new ContourService();
            physicTreatmentPlanService = new PhysicTreatmentPlanService();
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
            ViewBag.PhysicTreatmentPlanList = physicTreatmentPlanService.GetPhysicTreatmentPlanList();
            ViewBag.PhysicTreatmentPlanDetailList = physicTreatmentPlanService.GetPhysicTreatmentPlanDetailList();

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
        public JsonResult SetPhaseAsApprovedByPhysicst(long medicalRecordId)
        {

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

            if (UserData.RoleName == "doctor")
            {
                //No need to insert or update database
            }

            if (Data == null)
                Json(new { location = "" }, JsonRequestBehavior.AllowGet);

            List<int> plansCount = new List<int>();
            foreach (var item in Data)
            {
                if (!plansCount.Contains(item.PhaseId))
                {
                    plansCount.Add(item.PhaseId);
                }
            }

            var physicTreatmentPlanList = physicTreatmentPlanService.GetPhysicTreatmentPlanList();
            var physicTreatmentPlanDetails = physicTreatmentPlanService.GetPhysicTreatmentPlanDetailList();
            foreach (var planId in plansCount)
            {
                var eachPlanList = Data.Where(x => x.PhaseId == planId).ToList();
                var physicTreatmentPlanInDb = physicTreatmentPlanList.Where(x => x.PlanNo == planId && x.MedicalRecordId == medicalRecordId).FirstOrDefault();
                if (physicTreatmentPlanInDb == null || physicTreatmentPlanInDb.Id == 0)
                {
                    var filedId = FieldCommentData.Where(x => x.Plan == planId).FirstOrDefault().Field;
                    var comment = FieldCommentData.Where(x => x.Plan == planId).FirstOrDefault().Comment;
                    //Insert into physicTreatmentPlan
                    physicTreatmentPlanInDb = physicTreatmentPlanService.AddPhysicTreatmentPlan(new Model.PhysicTreatmentPlan() { MedicalRecordId = medicalRecordId, PhysicId = (int)UserData.UserId, PhysicFullName = UserData.FullName, PlanNo = planId, Fields = filedId, PhysicComment = comment, PhysicApplyDate = System.DateTime.Now });

                }

                foreach (var item in eachPlanList)
                {
                    if (!string.IsNullOrEmpty(item.CancerOARId) && Convert.ToInt32(item.CancerOARId) != 0)
                    {
                        var inserted = physicTreatmentPlanService.AddPhysicTreatmentPlanDetail(new Model.PhysicTreatmentPlanDetail()
                        {
                            MedicalRecordId = medicalRecordId,
                            PhysicTreatmentPlanId = physicTreatmentPlanInDb.Id,
                            CancerOARId = Convert.ToInt32(item.CancerOARId),
                            CancerOARIdValue = item.PlannedDose,
                            PlanNo = planId
                        }) ;
                    }
                    else if (!string.IsNullOrEmpty(item.TargetOARId) || Convert.ToInt32(item.TargetOARId) != 0)
                    {
                        var inserted = physicTreatmentPlanService.AddPhysicTreatmentPlanDetail(new Model.PhysicTreatmentPlanDetail()
                        {
                            MedicalRecordId = medicalRecordId,
                            PhysicTreatmentPlanId = physicTreatmentPlanInDb.Id,
                            CancerTargetId = Convert.ToInt32(item.TargetOARId),
                            CancerTargetValue = item.PlannedDose,
                            PlanNo = planId
                        });

                    }
                    

                }
            }

            return Json(new { location = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}