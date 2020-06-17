using PhysicManagement.Logic.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentDeviceController : BaseController
    {
        TreatmentService Service;
        MedicalRecordService MedicalService;
        public TreatmentDeviceController()
        {
            Service = new TreatmentService();
            MedicalService = new MedicalRecordService();
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent("List");
        }
        // GET: TreatmentDevice
        public ActionResult List()
        {

            List<Model.TreatmentDevice> TreatmentDevice = Service.GetTreatmentDeviceList();
            return View(TreatmentDevice);
        }

        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return View(new Model.TreatmentDevice());
            }
            else
            {
                var Entity = Service.GetTreatmentDeviceById(id.GetValueOrDefault());

                return View(Entity);
            }

        }
        [HttpPost]
        public ActionResult Modify(Model.TreatmentDevice entity)
        {
            bool IsAffected;
            if (entity.Id > 0)
            {
                IsAffected = Service.UpdateTreatmentDevice(entity);
            }
            else
            {
                IsAffected = Service.AddTreatmentDevice(entity);
            }
            if (IsAffected)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(int id)
        {
            var TreatmentDeviceData = Service.GetTreatmentDeviceById(id);
            int IsTreatmentDeviceUsedBefore = MedicalService.GetTotalMedicalRecordsByTreatmentDeviceId(TreatmentDeviceData.Id);
            if (IsTreatmentDeviceUsedBefore > 0)
            {
                TempData["Error"] = "این دستگاه درمانی در سیستم بیمار دارد و غیرقابل حذف است.";
                return RedirectToAction("List");
            }
            else
            {
                Service.DeleteTreatmentDevice(TreatmentDeviceData.Id);
                return RedirectToAction("List");
            }

        }
    }
}