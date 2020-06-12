using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class MedicalRecordController : BaseController
    {
        Logic.Services.MedicalRecordService Service;
        Logic.Services.ContourService ContourService;
        public MedicalRecordController()
        {
            Service = new Logic.Services.MedicalRecordService();
            ContourService = new Logic.Services.ContourService();
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

        public JsonResult GetMedicalRecordData(int medicalRecordId)
        {
            var medicalRecordData = Service.GetMedicalRecordById(medicalRecordId);
            return Json(
                new
                {
                    medicalRecordData.PatientFirstName,
                    medicalRecordData.PatientLastName,
                    medicalRecordData.SystemCode,
                    medicalRecordData.CancerId,
                    medicalRecordData.CancerTitle,
                    DoctorName = medicalRecordData.DoctorFirstName +" " + medicalRecordData.DoctorLastName,
                    ReceptionDate = Common.DateUtility.GetPersianDateTime(medicalRecordData.ReceptionDate),
                    CTDate = Common.DateUtility.GetPersianDateTime(medicalRecordData.CTEnterDate),
                    ContourDetail = ContourService.GetContourDetailsByMedicalRecordId(medicalRecordId)
                                    .Select(x=>new {x.Id,x.CancerTargetId,x.CancerOARId,x.Description })
                                    .ToList()
                }, JsonRequestBehavior.AllowGet);
        }
    }
}