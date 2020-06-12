using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicManagement.Logic.Services;
using PhysicManagement.Model;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Models;
using System.Data.Entity.Infrastructure;

namespace PhysicManagement.Controllers
{
    public class ContourController : Controller
    {
        Logic.Services.ContourService Service;
        Logic.Services.MedicalRecordService MedicalRecordService;
        public ContourController()
        {
            Service = new ContourService();
            MedicalRecordService = new MedicalRecordService();
        }
        /// <summary>
        /// فرم شماره 5
        /// تائید کانتور
        /// اطلاعات توسط کانتور ثبت شده و برای تائید توسط دکتر برای او ارسال گردید است.
        /// دکتر یا رزیدنت می توانند از این فرم برای تائید یا عدم تائید کانتور استفاده نمایند
        /// </summary>
        /// <param name="firstName">نام بیمار</param>
        /// <param name="lastName">نام خانوادگی بیمار</param>
        /// <param name="mobile">شماره موبایل بیمار</param>
        /// <param name="nationalCode">کد ملی بیمار</param>
        /// <param name="systemCode">شماره پرونده</param>
        /// <param name="code">کد بیمار</param>
        /// <returns></returns>
        public ActionResult ContourApprove(string firstName, string lastName, string mobile, string nationalCode, string systemCode, string code)
        {
            ViewBag.CancerList = new CancerService().GetCancerList();
            int CurrentPage = int.Parse(Request["p"] ?? "1");
            ViewBag.PageSize = 25;
            PagedList<MedicalRecord> MedicalRecords =
                Service.GetContoursToApprove
                (firstName, lastName, mobile, nationalCode, systemCode, code, CurrentPage, ViewBag.PageSize);

            ViewBag.TotalRecords = MedicalRecords.TotalRecords;
            return View(MedicalRecords);
        }

        public JsonResult UnacceptContourByDoctor(long medicalRecordId, string Description)
        {
            var UserData = Logic.Services.AuthenticatedUserService.GetUserId();
            var CountourData = Service.GetContourByMedicalRecordId(medicalRecordId);
            CountourData.DoctorFullName = UserData.FullName;
            CountourData.DoctorUserId = UserData.UserId.GetValueOrDefault().ToString();
            CountourData.Description = Description;
            CountourData.IsAccepted = false;
            CountourData.ModifyDate = DateTime.Now;
            Service.UpdateContour(CountourData);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptContourByDoctor(long medicalRecordId, string Description)
        {
            var UserData = AuthenticatedUserService.GetUserId();
            var CountourData = Service.GetContourByMedicalRecordId(medicalRecordId);
            CountourData.DoctorFullName = UserData.FullName;
            CountourData.DoctorUserId = UserData.UserId.GetValueOrDefault().ToString();
            CountourData.Description = Description;
            CountourData.IsAccepted = true;
            CountourData.ModifyDate = DateTime.Now;
            Service.UpdateContour(CountourData);
            return Json(new { location = "../Patient/SetMedicalRecordPhases?medicalRecordId=" + medicalRecordId }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SetCountorForMediacalRecord(CountorDataForMedicalRecordVM data)
        {
            var ContourSet = MedicalRecordService.SetCancerForMR(data.MedicalRecordId, data.CancerId);
            var ContourData = Service.GetContourByMedicalRecordId(data.MedicalRecordId);

            if (data.OARs != null && data.OARs.Count != 0)
            {

                foreach (var item in data.OARs)
                {
                    if (item.Value)
                    {
                        Service.AddContourDetails(new ContourDetails
                        {
                            CancerOARId = item.Id,
                            CancerTargetId = null,
                            ContourId = ContourData.Id,
                            MediacalRecordId = data.MedicalRecordId,
                            Description = item.Note,
                            SelectDate = DateTime.Now
                        });
                    }

                }
            }
            if (data.Targets != null && data.Targets.Count != 0)
            {
                foreach (var item in data.Targets)
                {
                    if (item.Value)
                    {
                        Service.AddContourDetails(new ContourDetails
                        {
                            CancerOARId = null,
                            CancerTargetId = item.Id,
                            ContourId = ContourData.Id,
                            MediacalRecordId = data.MedicalRecordId,
                            Description = item.Note,
                            SelectDate = DateTime.Now
                        });
                    }

                }
            }

            return Json(new { location = "" }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult SetCancerDetailForContour(long medicalRecordId, int cancerOARId)
        //{
        //    var ContourData = Service.GetContourByMedicalRecordId(medicalRecordId);
        //    var ContourDetail = Service.GetContourDetailByMedicalRecordIdAndCancerOARId(medicalRecordId, cancerOARId, ContourData.Id);
        //    if (ContourDetail == null)
        //    {
        //        Service.AddContourDetails(new Model.ContourDetails
        //        {
        //            CancerOARId = cancerOARId,
        //            SelectDate = DateTime.Now,
        //            MediacalRecordId = medicalRecordId,
        //            ContourId = ContourData.Id,
        //            Description = ""
        //        });
        //    }
        //    else {
        //        Service.DeleteContourDetails(ContourDetail.Id);
        //    }


        //    return Json(new MegaViewModel<bool>() { Status = MegaStatus.Successfull }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult SetApproveContourByDoctor(long countorId, long medicalRecordId)
        //{
        //    var ContourData = Service.SetContourAsAcceptedByDoctor(countorId);
        //    return Json(new { location = "../Patient/SetMedicalRecordPhases?medicalRecordId=" + medicalRecordId }, JsonRequestBehavior.AllowGet);
        //}
    }
}