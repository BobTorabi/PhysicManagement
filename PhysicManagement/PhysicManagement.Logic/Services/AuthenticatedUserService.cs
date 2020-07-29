using PhysicManagement.Logic.Enums;

namespace PhysicManagement.Logic.Services
{
    public class AuthenticatedUserService
    {
        public static UserType GetUserType()
        {
            var currentUser = GetUserId();
            switch (currentUser.RoleName.ToLower())
            {
                case "doctor":
                    return UserType.Doctor;
                case "resident":
                    return UserType.Resident;
                case "physicuser":
                    return UserType.Physist;
                default:
                    return UserType.User;
            }
        }
        public static bool ChangePassword(string newPassword, string oldPassword)
        {

            UserType CurrentUserType = GetUserType();
            var (UserId, _, _) = GetUserId();
            switch (CurrentUserType)
            {
                case UserType.Doctor:
                    {
                        return
                           DoctorService
                           .ChangeUserPassword
                           (UserId.GetValueOrDefault(), oldPassword, newPassword);
                    }
                case UserType.Resident:
                    {
                        return
                       ResidentService
                       .ChangeUserPassword
                       (UserId.GetValueOrDefault(), oldPassword, newPassword);
                    }
                case UserType.Physist:
                    {
                        return
                        PhysicUserService
                        .ChangeUserPassword
                        (UserId.GetValueOrDefault(), oldPassword, newPassword);
                    }
                case UserType.User:
                    {
                        return UserService
                          .ChangeUserPassword
                          (UserId.GetValueOrDefault(), oldPassword, newPassword);
                    }
            }
            return false;
        }
        public static (int? UserId, string RoleName, string FullName) GetUserId()
        {
            string CookieName = "RoleType";
            string RoleName = (Common.Cookie.ReadCookie(CookieName) ?? "").ToLower();
            switch (RoleName)
            {
                default:
                    return (UserId: null, RoleName: null, FullName: null);
                case "doctor":
                    {
                        var UserData = DoctorService.IsAuthenticated();
                        if (UserData == null)
                            return (UserId: null, RoleName: null, FullName: null);
                        else
                            return (UserId: UserData.Id, RoleName: RoleName, FullName: UserData.FirstName + " " + UserData.LastName);
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
