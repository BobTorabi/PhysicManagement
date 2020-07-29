using PhysicManagement.Common;
using PhysicManagement.Logic.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (Cookie.IsCookieSet("RoleType"))
            {
                string RoleValue = (Cookie.ReadCookie("RoleType") ?? "").ToString();
                switch (RoleValue)
                {
                    default:
                        return View();
                    case "doctor":
                        {
                            var UserData = DoctorService.IsAuthenticated();
                            if (UserData == null)
                                return View();
                            else
                            {
                                return Redirect("~/Home/Dashboard");
                            }
                        }
                    case "resident":
                        {
                            var UserData = ResidentService.IsAuthenticated();
                            if (UserData == null)
                                return View();
                            else
                            {
                                return Redirect("~/Home/Dashboard");
                            }
                        }
                    case "physicuser":
                        {
                            var UserData = PhysicUserService.IsAuthenticated();
                            if (UserData == null)
                                return View();
                            else
                            {
                                return Redirect("~/Home/Dashboard");
                            }
                        }
                }
            }
            return View();
        }
        public ActionResult TestData(string a, string b)
        {
            return Json(new { a = a, aEnc = DoctorService.EncryptPassword(a, b) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Login(string userName, string password, short roleType)
        {
            //Logic.Services.DoctorService.Register("d", "محمد", "یافتیان", "1", "09123399113", "1", "نامشخص", "", "", "مرد", null);
            //Logic.Services.PhysicUserService.Register("puser", "کاربر", "تستی", "1", "09123399113", "نامشخص", "نامشخص");
            //Logic.Services.ResidentService.Register("resident", "رزیدنت", "تستی", "1", "09123399113", "1", "نامشخص", "مرد", 1);

            switch (roleType)
            {
                //Doctor
                case 1:
                    {
                        var UserData = DoctorService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            DoctorService.SetAuthenticationCookie(userName, password, true);
                            Cookie.SetCookie("RoleType", "doctor", DateTime.Now.AddMonths(1));
                            if (Request["data"] != null)
                            {
                                return Redirect(Request["data"]);
                            }
                            else
                            {
                                return Redirect("~/Home/Dashboard");
                            }

                        }

                    }
                //Resident
                case 2:
                    {
                        var UserData = ResidentService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            ResidentService.SetAuthenticationCookie(userName, password, true);
                            Cookie.SetCookie("RoleType", "resident", DateTime.Now.AddMonths(1));
                            return Redirect("~/Home/Dashboard");
                        }
                    }
                //PhysicUser
                case 3:
                    {
                        var UserData = PhysicUserService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            PhysicUserService.SetAuthenticationCookie(userName, password, true);
                            Cookie.SetCookie("RoleType", "PhysicUser", DateTime.Now.AddMonths(1));
                            return Redirect("~/Home/Dashboard");
                        }
                    }
                //None of them
                default:
                    return Redirect("~/Account/Login");
            }

        }


        public ActionResult LogOut()
        {
            Cookie.ExpireCookie("RoleType");
            Cookie.ExpireCookie("Apa_Co_Auth");
            return Redirect("~/Account/Login");
        }


        [Authorization()]
        public ActionResult ProfileData()
        {
            ViewBag.RoleName = Cookie.ReadCookie("RoleType");
            return View();
        }


        [HttpPost]
        [Authorization()]
        public ActionResult ProfileUpdateForDoctor(Model.Doctor doctor)
        {
            var DoctorData = DoctorService.IsAuthenticated();
            DoctorService.UpdateProfile(DoctorData.Id, doctor.Username, doctor.FirstName, doctor.LastName, doctor.Mobile);
            return Redirect("ProfileData");
        }

        [Authorization()]
        public ActionResult ChangePassword()
        {
            ViewBag.RoleName = Cookie.ReadCookie("RoleType");
            return View();
        }


        [HttpPost]
        [Authorization()]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string NewPasswordRetype)
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                ViewBag.Error = "رمز فعلی خود را وارد کنید";
            }
            else if (string.IsNullOrEmpty(NewPassword))
            {
                ViewBag.Error = "رمز جدید خود را وارد کنید";
            }
            else if (string.IsNullOrEmpty(NewPasswordRetype))
            {
                ViewBag.Error = "تکرار رمز جدید خود را وارد کنید";
            }
            else if (NewPasswordRetype != NewPassword)
            {
                ViewBag.Error = "رمز جدید و تکرار آن شبیه به هم نیست.";
            }
            else
            {
                try
                {
                    AuthenticatedUserService.ChangePassword(NewPassword, OldPassword);
                    ViewBag.Done = "تغییر رمز با موفقیت انجام شد. لطفا دوباره وارد سیستم شوید";
                }
                catch (Exception ex)
                {
                    if (ex is FluentValidation.ValidationException Ex)
                    {
                        ViewBag.Error = Ex.Errors.Select(x => x.ErrorMessage).FirstOrDefault();
                    }
                    else
                    {
                        ViewBag.Error = "خطایی نا معلوم اتفاق افتاد.";
                    }
                }
            }
            return View();

        }

        public ActionResult NoAccess()
        {

            return View();
        }
    }
}