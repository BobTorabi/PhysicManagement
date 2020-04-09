using System.Collections.Generic;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class TreatmentDeviceController : BaseController
    {
        Logic.Services.TreatmentService Service;
        public TreatmentDeviceController()
        {
            Service = new Logic.Services.TreatmentService();
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
    }
}