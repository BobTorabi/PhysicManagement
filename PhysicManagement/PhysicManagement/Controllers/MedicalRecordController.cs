using PhysicManagement.Logic.Services;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class MedicalRecordController : BaseController
    {
        MedicalRecordService Service;
        ContourService ContourService;
        public MedicalRecordController()
        {
            Service = new MedicalRecordService();
            ContourService = new ContourService();
        }

        // GET: MedicalRecord
        public ActionResult PatientMedicalRecord(int id)
        {
            var medicalRecordData = Service.GetMedicalRecordById(id);
            return View(medicalRecordData);
        }

        //[HttpPost]
        //public JsonResult SetCancerForMR(int medicalRecordId, int cancerId)
        //{
        //    var UserData = Logic.Services.AuthenticatedUserService.GetUserId();
        //    var medicalRecordData = service.SetCancerForMR(medicalRecordId, cancerId, UserData.UserId.ToString());
        //    return Json(
        //        new MegaViewModel<bool> { Status = MegaStatus.Successfull },
        //        JsonRequestBehavior.AllowGet);
        //}
        public ActionResult SetNoDataForMedicalRecord(long mrId)
        {
            Service.SetNoCTScanDataForMedicalRecord(mrId);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetMedicalRecordData(int medicalRecordId)
        {
            var medicalRecordData = Service.GetMedicalRecordById(medicalRecordId);
            var Contour = ContourService.GetContourByMedicalRecordId(medicalRecordId);
            return Json(
                new
                {
                    medicalRecordData.PatientFirstName,
                    medicalRecordData.PatientLastName,
                    medicalRecordData.SystemCode,
                    medicalRecordData.CancerId,
                    medicalRecordData.CancerTitle,
                    DoctorName = medicalRecordData.DoctorFirstName + " " + medicalRecordData.DoctorLastName,
                    ReceptionDate = Common.DateUtility.GetPersianDateTime(medicalRecordData.ReceptionDate),
                    CTDate = Common.DateUtility.GetPersianDateTime(medicalRecordData.CTEnterDate),
                    Contour = new
                    {
                        IsAccepted = Contour?.IsAccepted,
                        Description = Contour?.Description,
                        ActionDate = Common.DateUtility.GetPersianDateTime(Contour?.ActionDate)
                    },
                    ContourDetail = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId)
                                    .Select(x => new { x.Id, x.CancerTargetId, x.CancerOARId, x.Description })
                                    .ToList()
                }, JsonRequestBehavior.AllowGet);
        }
    }
}