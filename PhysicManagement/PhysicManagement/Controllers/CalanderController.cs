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
                if (DateTime.TryParse(fromDate, out DateTime _FromDate))
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
                            if (CalandersData==null)
                                CalandersData = new List<Model.Calendar>();

                            if (CalandersData.Count > 0)
                            {
                                Result.AddMessage("تقدیم قبلا مقدار دهی شده است.ابتدا باید تمام مقادیر قبلی را پاک کنید.");
                            }
                            else
                            {
                                for (int i = 0; i < PhaseData.Fraction; i++)
                                {
                                    CalendarService.AddCalendar(new Model.Calendar { 
                                    AttendanceStatusId = null,
                                    Date = _FromDate,
                                    DoctorFullName = MedicalRecordObj.DoctorFirstName + " "+ MedicalRecordObj.DoctorLastName,
                                    MedicalRecordId = mrId,
                                    PatientFullName = MedicalRecordObj.PatientFirstName + " "+ MedicalRecordObj.PatientLastName,
                                    PersianDate = Common.DateUtility.GetPersianDateTime(_FromDate,"/"),
                                    PhysicTreatmentId = int.Parse(phaseId.ToString()),
                                    SessionNumber = (i+1),
                                    TreatmentPhaseText ="",
                                    });
                                    if (needFreeDays)
                                    {
                                        _FromDate = _FromDate.AddDays(1);
                                    }
                                    else {
                                        switch (_FromDate.DayOfWeek)
                                        {
                                            default:
                                            case DayOfWeek.Saturday:
                                            case DayOfWeek.Sunday:
                                            case DayOfWeek.Monday:
                                            case DayOfWeek.Tuesday:
                                            case DayOfWeek.Wednesday:
                                                _FromDate = _FromDate.AddDays(1);
                                                break;
                                            case DayOfWeek.Thursday:
                                                _FromDate = _FromDate.AddDays(2);
                                                break;
                                            case DayOfWeek.Friday:
                                                _FromDate = _FromDate.AddDays(1);
                                                break;
                                        }
                                    }
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