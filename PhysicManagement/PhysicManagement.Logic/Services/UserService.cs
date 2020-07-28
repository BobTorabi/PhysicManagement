using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;
using PhysicManagement.Common;
using PhysicManagement.Model;

namespace PhysicManagement.Logic.Services
{
   public class UserService
    {
        #region User Authorization
        protected const string AuthenticationCookieName = "Apa_Co_Auth";
        protected const string Delimeter = "___";
        protected static string SecretKey
        {
            get
            {
                string browserName = Network.BrowserName();
                string IP = Network.GetIP();
                return Cryptography.Encrypt(string.Join(".", IP, browserName));
            }
        }
        public static Model.User IsAuthenticated()
        {
            var cookieValue = ReadAuthCookie();
            if (cookieValue.HasValue)
            {
                return GetUserData(cookieValue.Value.userName, cookieValue.Value.passWord);
            }
            return null;
        }

        public static (string userName, string passWord)? ReadAuthCookie()
        {
            string cookieValue = Cookie.ReadCookie(AuthenticationCookieName);
            if (string.IsNullOrEmpty(cookieValue))
                return null;

            if (Cryptography.TryDecrypt(cookieValue, SecretKey, out string decryptedCookie))
            {
                string[] decryptedArray = System.Text.RegularExpressions.Regex.Split(decryptedCookie, Delimeter);
                string userName = decryptedArray[0];
                string passWord = decryptedArray[1];

                return (userName, passWord);
            }
            else
            {
                return null;
            }
        }

        public static void SetAuthenticationCookie(string userName, string passWord, bool rememberMe)
        {
            if (IsUserDataValid(userName, passWord))
            {
                string cookieValue = Cryptography.EncryptByUV(userName + Delimeter + passWord, SecretKey);
                if (rememberMe)
                    Cookie.SetCookie(AuthenticationCookieName, cookieValue, DateTime.Now.AddDays(30));
                else
                    Cookie.SetCookie(AuthenticationCookieName, cookieValue);
            }
        }

        public static User GetUserByUserId(long userId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.User.Where(x => x.Id == userId && x.IsActive == true).OrderBy(x => x.FirstName ).FirstOrDefault();
            }
        }

        public static User GetUserByUserName(string userName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }
        }

        public static User GetUserByUserNameAndMobile(string userName, string mobile)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.Mobile == mobile && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }

        }

        public static bool IsUserValidByUserName(string userName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.IsActive == true).Count();
                return UserExists != 0;
            }
        }

        public static bool IsUserValidByUserId(long userId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.User.Where(x => x.Id == userId && x.IsActive == true).Count();
                return UserExists == 1;
            }

        }


        public static bool IsUserDataValid(string userName, string passWord)
        {
            string EncryptedPassword = EncryptPassword(userName, passWord);
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.Password == EncryptedPassword && x.IsActive == true).Count();
                return UserExists == 1;
            }

        }

        public static User GetUserData(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            string EncryptedPassword = EncryptPassword(userName, password);
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.Password == EncryptedPassword && x.IsActive == true).FirstOrDefault();
                return UserExists
                    ;
            }
        }

        public static bool Register(string userName, string firstName, string lastName, string passWord, string mobileNo)
        {
            User UserEntity = new User()
            {

                FirstName = firstName,
                Mobile = mobileNo,
                LastName = lastName,
                Password = EncryptPassword(userName, passWord),
                Username = userName,
                IsSupervisor = true,
                IsActive = true
            };

            var validation = new UserValidation.UserEntityValidate().Validate(UserEntity);
            if (validation.IsValid)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    if (IsUserValidByUserName(userName))
                    {
                        throw new ValidationException("این نام کاربری تکراری است");
                    }
                    else
                    {
                        db.User.Add(UserEntity);
                        return db.SaveChanges() == 1;
                    }
                }
            }
            throw new ValidationException(validation.Errors);

        }

        public static bool UpdateProfile(int id, string userName, string firstName, string lastName, string mobileNo)
        {
            var currentUser = GetUserByUserId(id);
            if (currentUser == null)
                throw MegaException.ThrowException("چنین کاربری در سامانه پیدا نشد.");

            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.Mobile = mobileNo;
            var validation = new UserValidation.UserEntityValidate().Validate(currentUser);
            if (validation.IsValid)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    db.User.Add(currentUser);
                    return db.SaveChanges() == 1;
                }
            }
            throw new ValidationException(validation.Errors);
        }

        public static bool LockUser(int id)
        {
            var userEntity = GetUserByUserId(id);
            if (userEntity == null)
                throw MegaException.ThrowException("کاربری با این شناسه در پایگاه داده وجود ندارد");

            UserService.UpdateProfile(userEntity.Id, userEntity.Username, userEntity.FirstName, userEntity.LastName, userEntity.Mobile);
            return true;
        }
        public static bool Logout()
        {
            Cookie.ExpireCookie(AuthenticationCookieName);
            return true;
        }

        public static bool ChangeUserPassword(string userName, string oldPassword, string newPassword)
        {
            string encryptedOldPassword = EncryptPassword(userName, oldPassword);
            var userData = GetUserData(userName, encryptedOldPassword);
            if (userData == null)
                throw MegaException.ThrowException("چنین کاربری پیدا نشد.");

            string encryptedNewPassword = EncryptPassword(userName, newPassword);
            userData.Password = encryptedNewPassword;
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.User.Find(userData.Id);
                Entity.Password = userData.Password;
                return db.SaveChanges() == 1;
            }
        }

        public static string GetUserPasswordByMobileOrEmail(string userName, string mobile)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var userPassword = db.User.Where(x => x.Username.ToLower() == userName.ToLower() && x.Mobile == mobile && x.IsActive == true).Select(x => x.Password).FirstOrDefault();
                if (string.IsNullOrEmpty(userPassword))
                    throw Common.MegaException.ThrowException("چنین کاربری پیدا نشد.");
                string decryptedPassword = DecryptPassword(userName, userPassword);
                return decryptedPassword;
            }

        }

        public static string EncryptPassword(string userName, string passWord)
        {
            return Cryptography.Encrypt(passWord);
        }

        public static string DecryptPassword(string userName, string encryptedPassword)
        {
            return Cryptography.Decrypt(encryptedPassword);
        }

        #endregion
        #region User Section
        public List<Model.User> GetUserList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.User.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.User GetUserById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.User.Find(entityId);
                return Entity;
            }
        }
        public bool AddUser(Model.User entity)
        {
            var validation = new UserValidation.UserEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.User.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateUser(Model.User entity)
        {
            var validation = new UserValidation.UserEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.User.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Username = entity.Username;
                Entity.Mobile = entity.Mobile;
                Entity.Password = entity.Password;
                Entity.IsSupervisor = entity.IsSupervisor;
                Entity.RegisterDate = entity.RegisterDate;
                Entity.Gender = entity.Gender;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteUser(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.User.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.User.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }

}
