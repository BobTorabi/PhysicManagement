using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string password, short roleType)
        {
            //Logic.Services.DoctorService.Register("doctor", "دکتر", "تستی", "1", "09123399113", "1", "نامشخص", "", "", "مرد", null);
            //Logic.Services.PhysicUserService.Register("puser", "کاربر", "تستی", "1", "09123399113", "نامشخص", "نامشخص");
            //Logic.Services.ResidentService.Register("resident", "رزیدنت", "تستی", "1", "09123399113", "1", "نامشخص", "مرد", 1);

            switch (roleType)
            {
                //Doctor
                case 1:
                    {
                        var UserData = Logic.Services.DoctorService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            Logic.Services.DoctorService.SetAuthenticationCookie(userName, password, true);
                            Common.Cookie.SetCookie("RoleType", "doctor");
                            return Redirect("~/Home/Dashboard");
                        }

                    }
                //Resident
                case 2:
                    {
                        var UserData = Logic.Services.ResidentService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            Logic.Services.ResidentService.SetAuthenticationCookie(userName, password, true);
                            Common.Cookie.SetCookie("RoleType", "resident");
                            return Redirect("~/Home/Dashboard");
                        }
                    }
                //PhysicUser
                case 3:
                    {
                        var UserData = Logic.Services.PhysicUserService.GetUserData(userName, password);
                        if (UserData == null)
                        {
                            TempData["Error"] = "اطلاعات وارد شده اشتباه است";
                            return Redirect("~/Account/Login?result=failed");
                        }
                        else
                        {
                            Logic.Services.PhysicUserService.SetAuthenticationCookie(userName, password, true);
                            Common.Cookie.SetCookie("RoleType", "PhysicUser");
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
            Common.Cookie.ExpireCookie("RoleType");
            Common.Cookie.ExpireCookie("Apa_Co_Auth");
            return Redirect("~/Account/Login");
        }
        [Authorization()]
        public ActionResult ProfileData()
        {
            ViewBag.RoleName = Common.Cookie.ReadCookie("RoleType");
            return View();
        }

        [HttpPost]
        [Authorization()]
        public ActionResult ProfileUpdateForDoctor(Model.Doctor doctor)
        {
            var DoctorData = Logic.Services.DoctorService.IsAuthenticated();
            Logic.Services.DoctorService.UpdateProfile(DoctorData.Id, doctor.Username, doctor.FirstName, doctor.LastName, doctor.Mobile);
            return Redirect("ProfileData");
        }

        [Authorization()]
        public ActionResult ChangePassword()
        {
            ViewBag.RoleName = Common.Cookie.ReadCookie("RoleType");
            return View();
        }
        [Authorization()]
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword,string NewPassword,string NewPasswordRetype)
        {
            return View();
        }
    }
}