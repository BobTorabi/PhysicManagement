using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
    public class AuthenticatedUserService
    {
        public static (int? UserId,string RoleName,string FullName) GetUserId()
        {
            string CookieName = "RoleType";
            string RoleName = (Common.Cookie.ReadCookie(CookieName) ?? "").ToLower();
            switch (RoleName)
            {
                default:
                    return (UserId:null , RoleName:null, FullName : null);
                case "doctor":
                    {
                        var UserData = DoctorService.IsAuthenticated();
                        if (UserData == null)
                            return (UserId: null, RoleName: null, FullName: null);
                        else
                            return (UserId: UserData.Id, RoleName: RoleName, FullName: UserData.FirstName + " "+ UserData.LastName);
                    }
                case "resident":
                    {
                        var UserData = ResidentService.IsAuthenticated();
                        if (UserData == null)
                            return (UserId: null, RoleName: null, FullName: null);
                        else
                            return (UserId: UserData.Id, RoleName: RoleName, FullName: UserData.FirstName + " " + UserData.LastName);
                    }
                case "physicuser":
                    {
                        var UserData = Logic.Services.PhysicUserService.IsAuthenticated();
                        if (UserData == null)
                            return (UserId: null, RoleName: null, FullName: null);
                        else
                            return (UserId: UserData.Id, RoleName: RoleName, FullName: UserData.FirstName + " " + UserData.LastName);
                    }

            }
        }
    }
}
