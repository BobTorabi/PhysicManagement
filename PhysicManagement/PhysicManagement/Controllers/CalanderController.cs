using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class CalanderController : Controller
    {
        private readonly Logic.Services.CalendarService CalendarService;
        private readonly Logic.Services.MedicalRecordService medicalRecordService;
        private readonly Logic.Services.TreatmentService TreatmentService;
        public CalanderController()
        {
            CalendarService = new Logic.Services.CalendarService();
            medicalRecordService = new Logic.Services.MedicalRecordService();
            TreatmentService = new Logic.Services.TreatmentService();
        }
        [HttpPost]
        public JsonResult SetCalanderForMedicalRecordAndPhase(long mrId, long phaseId, string fromDate, bool needFreeDays)
        {
            MegaViewModel<bool> Result = new MegaViewModel<bool>(false, MegaStatus.Failed);
            if (string.IsNullOrEmpty(fromDate))
            {
                Result.AddMessage("تاریخ شروع نمی تواند خالی باشد");
            }
            else
            {
                DateTime? _FromDate = Common.DateUtility.TryGetDateTime(fromDate);
                if (_FromDate.HasValue)
                {
                    var MedicalRecordObj = medicalRecordService.GetMedicalRecordById(mrId);
                    if (MedicalRecordObj == null)
                    {
                        Result.AddMessage("پرونده پزشکی مورد نظر پیدا نشد");
                    }
                    else
                    {
                        var PhaseData = TreatmentService.GetTreatmentPhaseById(phaseId);
                        if (PhaseData == null)
                        {
                            Result.AddMessage("فاز درمانی مورد نظر پیدا نشد");
                        }
                        else
                        {
                            var CalandersData = CalendarService.GetCalendarByMedicalRecordIdAndPhaseId(mrId, phaseId);
                            if (CalandersData != null && CalandersData.Count >= 0)
                            {
                                Result.AddMessage("تقدیم قبلا مقدار دهی شده است.ابتدا باید تمام مقادیر قبلی را پاک کنید.");
                            }
                            else
                            {
                                for (int i = 0; i < PhaseData.Fraction; i++)
                                {

                                }
                            }
                        }
                    }
                }
                else
                {
                    Result.AddMessage("تاریخ شروع نمی تواند خالی باشد");
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}