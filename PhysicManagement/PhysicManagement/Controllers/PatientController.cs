﻿using PhysicManagement.Logic.Services;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class PatientController : BaseController
    {
        readonly PatientService Service;
        readonly MedicalRecordService MedicalService;
        readonly CancerService CancerService;
        readonly ContourService ContourService;
        readonly TreatmentService TreatmentService;
        readonly CalendarService CalendarService;
        readonly DoctorService DoctorService;

        public PatientController()
        {
            Service = new PatientService();
            MedicalService = new MedicalRecordService();
            CancerService = new CancerService();
            ContourService = new ContourService();
            TreatmentService = new TreatmentService();
            CalendarService = new CalendarService();
            DoctorService = new DoctorService();
        }
        public ActionResult Index()
        {
            return View();
        }
        // GET: Patient
        public ActionResult List()
        {
            List<Model.Patient> Patient = Service.GetPatientList();
            return View(Patient);
        }
        /// <summary>
        /// نمایش تقویم برای یک پرونده خاص و فاز درمان خاص
        /// </summary>
        /// <param name="mrId"></param>
        /// <param name="phaseId"></param>
        /// <returns></returns>
        public ActionResult Calander(long? mrId, long? phaseId)
        {
            if (mrId.HasValue == false)
            {
                // در صورتی که پرونده قابل شناسایی نباشد به لیست تائید پلن شده ها بر میگردیم تا انتخاب پرونده انجام شود
                return Redirect("~/treatmentPhase/ListForPhysicist");
            }
            else
            {
                long MedicalRecordId = mrId.GetValueOrDefault();
                var MedicalRecordEntity = MedicalService.GetMedicalRecordById(MedicalRecordId);
                if (!phaseId.HasValue || MedicalRecordEntity == null)
                {
                    return Redirect("/Patient/TreatmentData?mrId=" + MedicalRecordId);
                }
                else
                {
                    ViewBag.MedicalRecord = MedicalRecordEntity;
                    ViewBag.TreatmentPhase = TreatmentService.GetTreatmentPhaseById(phaseId.GetValueOrDefault());
                    ViewBag.TreatmentPhaseId = phaseId;
                    var CalendarData = CalendarService.GetCalendarByMedicalRecordIdAndPhaseId(MedicalRecordId, phaseId.GetValueOrDefault());
                    return View(CalendarData);
                }


            }

        }
        public ActionResult CalanderChoosePhase(int mrId)
        {
            var MedicalRecordEntity = MedicalService.GetMedicalRecordById(mrId);
            var TreatmentPhaseData = TreatmentService.GetTreatmentPhasesByMedicalRecordId(MedicalRecordEntity.Id);
            ViewBag.MedicalRecord = MedicalRecordEntity;
            return View(TreatmentPhaseData);
        }
        public ActionResult SetMedicalRecordPhases(long medicalRecordId)
        {
            var ModelData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var Data = Service.GetPatientById(ModelData.PatientId);
            ViewBag.PatientData = Data;

            return View(ModelData);
        }

        public ActionResult ListOfTreatmentPlans
            (string firstName, string lastName, string mobile, string nationalCode, string caseCode, string code)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            PagedList<Model.Patient> Patient =
                Service
                .GetPatientListWithFilters
                (firstName, lastName, mobile, nationalCode, caseCode, code, null, null, null, null, CurrentPage, ViewBag.PageSize);

            ViewBag.TotalRecords = Patient.TotalRecords;
            return View(Patient);
        }


        /// <summary>
        /// فرم شماره 6 - ثبات
        /// ثبت اطلاعات تعداد فازهای یک پرونده پزشکی
        /// </summary>
        /// <param name="medicalRecordId"></param>
        /// <param name="Phases"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetMedicalRecordPhases(long medicalRecordId, int Phases)
        {
            var UserData = AuthenticatedUserService.GetUserId();
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            //if this user has treatment phase records without acceptance we need to update it instead of inserting it
            for (int i = 1; i <= Phases; i++)
            {
                TreatmentService.AddTreatmentPhase(new Model.TreatmentPhase
                {
                    ApprovedDate = null,
                    ApprovedUserFullName = null,
                    Description = "",
                    Fraction = null,
                    IsApproved = null,
                    MedicalRecordId = medicalRecordId,
                    PatientFirstName = MedicalRecordData.PatientFirstName,
                    PatientLastName = MedicalRecordData.PatientLastName,
                    PhaseNumber = i,
                    PhaseText = "",
                    PrescribeDate = DateTime.Now,
                    PrescribedUserRole = UserData.RoleName,
                    PrescribesdUserFullName = UserData.FullName,
                    PrescribesdUserId = UserData.UserId.ToString(),
                    Reserve1 = null,
                    Reserve2 = null,
                    Reserve3 = "",
                    Target = "",
                    TreatmentDeviceId = null,
                    TreatmentDeviceTitle = null
                });
            }
            MedicalService.UpdateMedicalRecordForPhaseCount(medicalRecordId, Phases);
            return Json(new { location = "../Patient/SetMedicalRecordTreatmentPhase?medicalRecordId=" + medicalRecordId }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// فرم شماره 7
        /// تجویز درمان بر روی تعداد فازهای تعیین شده ی درمان
        /// </summary>
        /// <param name="medicalRecordId">شناسه پرونده پزشکی یک بیمار</param>
        /// <returns></returns>
        public ActionResult SetMedicalRecordTreatmentPhase(long medicalRecordId)
        {
            var MedicalRecordData = MedicalService.GetMedicalRecordById(medicalRecordId);
            var PatientData = Service.GetPatientById(MedicalRecordData.PatientId);
            var ViewData = TreatmentService.GetTreatmentPhasesByMedicalRecordId(medicalRecordId);
            ViewBag.MedicalRecordData = MedicalRecordData;
            ViewBag.PatientData = PatientData;
            ViewBag.CancerOARList = CancerService.GetCancerOARList();
            ViewBag.CancerTargetList = CancerService.GetCancerTargetList();
            ViewBag.ContourDetails = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId).Where(x => x.CancerTargetId != null).ToList();
            ViewBag.TreatmentService = TreatmentService.GetTreatmentDeviceList();
            return View(ViewData);
        }

        public ActionResult DeletePatient(int id)
        {
            Service.DeletePatient(id);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// ثبت اطلاعات فازهای درمانی برای یک پرونده پزشکی
        /// معادل ذخیره سازی فرم شماره 7
        /// </summary>
        /// <param name="medicalRecordId"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetMedicalRecordTreatmentPhase(MedicalRecordTreatmentPhaseVM Data)
        {
            var UserData = AuthenticatedUserService.GetUserId();
            var MedicalRecordData = MedicalService.GetMedicalRecordById(Data.MedicalRecordId);
            var TreatmentPhase = TreatmentService.GetTreatmentPhasesByMedicalRecordId(Data.MedicalRecordId);
            foreach (var item in Data.Phases)
            {
                var Device = TreatmentService.GetTreatmentDeviceById(item.DeviceId);
                switch (item.No)
                {
                    case 1:
                        MedicalRecordData.Phase1TreatmentDeviceId = Device.Id;
                        MedicalRecordData.Phase1TreatmentDeviceTitle = Device.Title;
                        break;
                    case 2:
                        MedicalRecordData.Phase2TreatmentDeviceId = Device.Id;
                        MedicalRecordData.Phase2TreatmentDeviceTitle = Device.Title;
                        break;
                    case 3:
                        MedicalRecordData.Phase3TreatmentDeviceId = Device.Id;
                        MedicalRecordData.Phase3TreatmentDeviceTitle = Device.Title;
                        break;
                    case 4:
                        MedicalRecordData.Phase4TreatmentDeviceId = Device.Id;
                        MedicalRecordData.Phase4TreatmentDeviceTitle = Device.Title;
                        break;
                }
                var CurrentTreatmentPhase = TreatmentPhase.Where(x => x.PhaseNumber == item.No).FirstOrDefault();
                CurrentTreatmentPhase.TreatmentDeviceId = Device.Id;
                CurrentTreatmentPhase.TreatmentDeviceTitle = Device.Title;
                CurrentTreatmentPhase.Fraction = item.Fraction;
                CurrentTreatmentPhase.IsPrescribedByDoctor = true;
                ///  با تعیین تعداد فاز درمان و انتخاب دستگاه و دوز دارو به مرحله ی تائید پلن میریم
                TreatmentService.UpdateTreatmentPhase(CurrentTreatmentPhase);

                foreach (var oar in item.cancerAORs)
                {
                    var CancerOAR = CancerService.GetCancerOARById(oar.Id);
                    TreatmentService.AddTreatmentPhaseDetail(new Model.TreatmentPhaseDetail
                    {
                        CancerOARId = CancerOAR.Id,
                        CancerOARTitle = CancerOAR.OrganTitle,
                        CancerOARTolerance = CancerOAR.Tolerance,
                        CancerTargetOptimum = "",
                        CancerTargetId = null,
                        CancerTargetTitle = "",
                        Description = "",
                        PresciptionHasApproved = null,
                        MedicalRecordId = MedicalRecordData.Id,
                        PatientFirstName = MedicalRecordData.PatientFirstName,
                        PatientLastName = MedicalRecordData.PatientLastName,
                        PrescribedDate = DateTime.Now,
                        PrescribedDose = oar.Dose,
                        TreatmentPhaseId = CurrentTreatmentPhase.Id,
                        AcceptedDoctorDate = DateTime.Now,
                        AcceptedDoctorFullName = UserData.FullName,
                        AcceptedDoctorUserId = UserData.UserId,
                        DoctorDescription = ""
                    });

                }

            }
                MedicalRecordData.TreatmentProcessId = (int)Logic.Enums.TreatmentProcessType.PrescribeTreatment;
            MedicalRecordData.TreatmentDeviceIsQueued = false;
            MedicalService.UpdateMedicalRecord(MedicalRecordData);
            return Redirect("~/TreatmentPhase/List");
        }
        public ActionResult PatientSearch(string firstName, string lastName, string mobile, string nationalCode,
            string caseCode, string code, int? lastDaysReport,
            bool? ContourState, int? DoctorId, string RecieptDate
            )
        {
            DateTime? _RecieptDate = null;
            if (!string.IsNullOrEmpty(RecieptDate))
            {
                _RecieptDate = Common.DateUtility.TryGetDateTime(RecieptDate, Common.DateUtility.TimeFormat.StartOfDay);
            }
            var Doctors = DoctorService.GetIdNameFromDoctorList();
            ViewBag.DoctorId = new SelectList(Doctors, "Id", "Name", DoctorId);
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 25;
            PagedList<Model.Patient> Patient =
                Service.GetPatientListWithFilters
                (firstName, lastName, mobile, nationalCode, caseCode, code, lastDaysReport, ContourState, DoctorId, _RecieptDate, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = Patient.TotalRecords;
            return View(Patient);
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                ViewBag.GenderIsMale = new SelectList(Service.GetPatientList());
                return View(new Model.Patient());
            }
            else
            {
                var Entity = Service.GetPatientById(id.GetValueOrDefault());
                ViewBag.GenderIsMale = new SelectList(Service.GetPatientList(), "Id", "GenderIsMale", Entity.GenderIsMale);
                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.Patient entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdatePatient(entity);
            }
            else
            {
                IsAffected = Service.AddPatient(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }

        public ActionResult RegisterPatient()
        {
            ViewBag.doctorId = new DoctorService().GetIdNameFromDoctorList();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile, string description, bool isIranian)
        {
            var PatientObject = 
                Service.RegisterPatient
                (patientFirstName, patientLastName, nationalCode, doctorId, mobile, description, isIranian);
            //SMS
            //new NotificationService().SendSMSAlarm((int)PatientObject.Id, Logic.Enums.AlarmEventType.MedicalRecordAdded);
            
            //Setting default valuse at the moment
            new NotificationService().AddAlarm(Logic.Enums.AlarmEventType.MedicalRecordAdded, (int)PatientObject.Id, true, true, true);
            return Json(new { location = "PatientInfo?patientId=" + PatientObject.Id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PatientInfo(long patientId)
        {
            var PatientData = Service.GetPatientById(patientId);
            return View(PatientData);
        }
        public ActionResult PatientCTCode()
        {
            return View();
        }
        public JsonResult PatientInfoByMedicalRecordId(int medicalRecordId)
        {
            var Data = new PatientService().GetPatientByMedicalRecordId(medicalRecordId);
            return Json(new MegaViewModel<PatientVMs.MedicalRecordDataWithPatientData>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PatientCTCode(string mriCode, string ctDescription, long medicalRecordId)
        {
            MedicalRecordService medicalRecordService = new Logic.Services.MedicalRecordService();
            var PatientCTCode = medicalRecordService.AddMedicalRecordCTCode(mriCode, ctDescription, medicalRecordId);
            return Json(new { location = "PatientInfo?patientId=" + PatientCTCode.Id }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// لیست بیمارانی که اطلاعات ام ار ای یا سی تی اسکن آنها در سیستم ثبت نشده اشت.
        /// </summary>
        /// <returns></returns>
        public ActionResult PatientWithNoCTScanOrMRI(string firstName, string lastName, string mobile,
            string nationalCode, string systemCode, string code, int? lastDaysReport)
        {
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 5;
            PagedList<Model.MedicalRecord> MedicalRecord =
                Service.GetPatientListDontHaveMriOrCTScan(firstName, lastName, mobile,
                   nationalCode, systemCode, code, lastDaysReport, CurrentPage, ViewBag.PageSize);
            ViewBag.TotalRecords = MedicalRecord.TotalRecords;

            return View(MedicalRecord);
        }
        [HttpPost]
        public ActionResult SetCTAndMIRDataForMedicalRecord(int medicalRecordId, string cTScanCode, string cTScanDescription, string mRICode)
        {
            var Data = Service.SetPatientMediacalRecordCTScanData(medicalRecordId, cTScanCode, cTScanDescription, mRICode);
            new NotificationService().AddAlarm(Logic.Enums.AlarmEventType.CTCodeAdded, (long)medicalRecordId, true, true, true);
            return Json(new MegaViewModel<bool>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientWithNoThreatmentPlan(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code, int? lastDaysReport)
        {
            List<Model.MedicalRecord> Patient =
                Service.
                GetPatientListWithUnsetFusion(firstName, lastName, nationalCode, mobile, systemCode, code, lastDaysReport);
            return View(Patient);
        }
        public ActionResult ListOfUnsetCountorsForCases(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code, bool? hasContour)
        {
            ViewBag.CancerList = new CancerService().GetCancerList();

            List<Model.MedicalRecord> Patient = Service.GetPatientListWithUnsetCountor(firstName, lastName, nationalCode, mobile, systemCode, code, hasContour);
            return View(Patient);
        }
        public ActionResult SetPatientMediacalRecordCPAndFusion(int medicalRecordId, string TPDescription, bool needFusion)
        {
            var Data = Service.SetPatientMediacalRecordCPAndFusion(medicalRecordId, TPDescription, needFusion);
            new NotificationService().AddAlarm(Logic.Enums.AlarmEventType.TPSofwareSelected, (long)medicalRecordId, true, true, true);
            return Json(new MegaViewModel<bool>() { Data = Data }, JsonRequestBehavior.AllowGet);
        }

    }
}