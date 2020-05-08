using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class MedicalRecordController : BaseController
    {
        Logic.Services.MedicalRecordService service;
        public MedicalRecordController()
        {
            service = new Logic.Services.MedicalRecordService();
        }
        // GET: MedicalRecord
        public ActionResult PatientMedicalRecord(int id)
        {
            var medicalRecordData = service.GetMedicalRecordById(id);
            return View(medicalRecordData);
        }

        [HttpPost]
        public JsonResult SetCancerForMR(int medicalRecordId, int cancerId)
        {
            var medicalRecordData = service.SetCancerForMR(medicalRecordId, cancerId, "");
            return Json(
                new MegaViewModel<bool> { Status = MegaStatus.Successfull }, 
                JsonRequestBehavior.AllowGet);
        }
    }
}