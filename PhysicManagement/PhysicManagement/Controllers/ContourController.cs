using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;
using PhysicManagement.Model;

namespace PhysicManagement.Controllers
{
    public class ContourController : Controller
    {
        Logic.Services.ContourService Service;
        public ContourController()
        {
            Service = new Logic.Services.ContourService();
        }

        public ActionResult ContourApprove() {
            List<MedicalRecord> Patients = Service.GetContoursToApprove();
            return View(Patients);
        }

      

        [HttpPost]
        public ActionResult ConfirmContour(string actionDate, string acceptDate, string acceptUser, int cancerId, string description)
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetCancerDetailForContour(long medicalRecordId, int cancerOARId)
        {
            var ContourData = Service.GetContourByMedicalRecordId(medicalRecordId);
            var ContourDetail = Service.GetContourDetailByMedicalRecordIdAndCancerOARId(medicalRecordId, cancerOARId, ContourData.Id);
            if (ContourDetail == null)
            {
                Service.AddContourDetails(new Model.ContourDetails
                {
                    CancerOARId = cancerOARId,
                    SelectDate = DateTime.Now,
                    MediacalRecordId = medicalRecordId,
                    ContourId = ContourData.Id,
                    Description = ""
                });
            }
            else {
                Service.DeleteContourDetails(ContourDetail.Id);
            }


            return Json(new MegaViewModel<bool>() { Status = MegaStatus.Successfull }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetApproveContourByDoctor(long countorId,long medicalRecordId) { 
            var ContourData = Service.SetContourAsAcceptedByDoctor(countorId);
            return Json(new { location = "../Patient/SetMedicalRecordPhases?medicalRecordId=" + medicalRecordId },JsonRequestBehavior.AllowGet);
        }
    }
}