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
    public class DoctorService
    {

        #region Doctor Authorization
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
        public static Doctor IsAuthenticated()
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

        public static Doctor GetUserByUserId(long userId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Doctor.Where(x=>x.Id == userId && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }
        }

        public static Doctor GetUserByUserName(string userName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Doctor.Where(x => x.Username.ToLower() == userName.ToLower() && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }
        }

        public static Doctor GetUserByUserNameAndMobile(string userName, string mobile)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Doctor.Where(x => x.Username.ToLower() == userName.ToLower() && x.Mobile == mobile && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }

        }
        public static Doctor GetUserByUserNameAndMobileOrEmail(string userName, string mobile, string email)
        {
            var UserEntity =
                UserRepository
                .GetQuery(x => x.UserName.ToLower() == userName.ToLower() && (x.Email == mobile || x.Email == email) && x.IsActive)
                .FirstOrDefault();

            return UserEntity;
        }

        public static bool IsUserValidByUserName(string userName)
        {
            var UserExists = UserRepository.GetQuery(x => x.UserName.ToLower() == userName.ToLower() && x.IsActive).Count();
            return UserExists == 1;
        }

        public static bool IsUserValidByUserId(long userId)
        {
            var UserExists = UserRepository.GetQuery(x => x.Id == userId && x.IsActive).Count();
            return UserExists == 1;
        }

        public static bool IsUserDataValid(string userName, string passWord)
        {
            string EncryptedPassword = EncryptPassword(userName, passWord);

            var UserExists =
                UserRepository
                .GetQuery(x => x.UserName.ToLower() == userName.ToLower() && x.PasswordHash == EncryptedPassword && x.IsActive)
                .Count();

            return UserExists == 1;
        }

        public static Doctor GetUserData(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            string EncryptedPassword = EncryptPassword(userName, password);

            var UserExists =
                UserRepository
                .GetQuery(x => x.UserName.ToLower() == userName.ToLower() && x.PasswordHash == EncryptedPassword && x.IsActive)
                .FirstOrDefault();
            return UserExists;
        }

        public static bool Register(string userName, string firstName, string lastName, string passWord, string mobileNo, string email, Enums.Sex sex)
        {
            Doctor UserEntity = new Doctor()
            {
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                FirstName = firstName,
                IsActive = true,
                Guid = Guid.NewGuid().ToString(),
                IsLocked = false,
                UserMobile = mobileNo,
                ModifiedBy = 1,
                LastName = lastName,
                ModifiedDate = DateTime.Now,
                Reserve1 = true,
                Reserve2 = true,
                Email = email,
                PasswordHash = EncryptPassword(userName, passWord),
                LastLoginDate = DateTime.Now,
                UserName = userName,
                UserCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
                UserTypeId = 1,
                ClientId = 1 // Speedy
            };

            var validation = new DoctorValidation.DoctorEntityValidate().Validate(UserEntity);
            if (validation.IsValid)
            {
                _ = UserRepository.Add(UserEntity);
                return true;
            }
            throw new ValidationException(validation.Errors);

        }

        public static bool UpdateProfile(int id, string userName, string firstName, string lastName, string mobileNo, string email, Enums.Sex sex)
        {
            var currentUser = GetUserById(id);
            if (currentUser == null)
                throw MegaException.ThrowException("چنین کاربری در سامانه پیدا نشد.");

            if (currentUser.IsLocked)
                throw MegaException.ThrowException("حساب کاربری این کاربر قفل است برای ویرایش ابتدا کاربر را فعال کنید.");

            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.Email = email;
            currentUser.UserMobile = mobileNo;
            var validation = new DoctorValidation.DoctorEntityValidate().Validate(currentUser);
            if (validation.IsValid)
            {
                _ = UserRepository.Update(currentUser);
                return true;
            }
            throw new ValidationException(validation.Errors);
        }

        public static bool LockUser(int id)
        {
            var userEntity = GetUserById(id);
            if (userEntity == null)
                throw MegaException.ThrowException("کاربری با این شناسه در پایگاه داده وجود ندارد");

            userEntity.IsLocked = true;
            UserService.UpdateUser(userEntity);
            return true;
        }

        public static bool UnlockUser(int id)
        {
            var userEntity = GetUserById(id);
            if (userEntity == null)
                throw MegaException.ThrowException("کاربری با این شناسه در پایگاه داده وجود ندارد");

            userEntity.IsLocked = false;
            UserService.UpdateUser(userEntity);
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
            UserRepository.Update(userData);
            return true;
        }

        public static string GetUserPasswordByMobileOrEmail(string userName, string mobile, string email)
        {
            var userPassword =
                UserRepository
                .GetQuery(x => x.UserName.ToLower() == userName.ToLower() && (x.Email == mobile || x.Email == email) && x.IsActive)
                .Select(x => x.PasswordHash)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(userPassword))
                throw Common.MegaException.ThrowException("چنین کاربری پیدا نشد.");

            string decryptedPassword = DecryptPassword(userName, userPassword);
            return decryptedPassword;
        }

        public static string EncryptPassword(string userName, string passWord)
        {
            string userNameKey = userName.GetTypeCode().ToString();
            return Cryptography.EncryptByUV(userNameKey, passWord);
        }

        internal static string DecryptPassword(string userName, string encryptedPassword)
        {
            string userNameKey = userName.GetTypeCode().ToString();
            return Cryptography.Decrypt(encryptedPassword, userNameKey);
        }

        #endregion
        #region Doctor section

        public List<Model.Doctor> GetDoctorList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Doctor.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.Doctor GetDoctorById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entityId);
                return Entity;
            }
        }
        public bool AddDoctor(Model.Doctor entity)
        {
            var validation = new DoctorValidation.DoctorEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Doctor.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateDoctor(Model.Doctor entity)
        {
            var validation = new DoctorValidation.DoctorEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Gender = entity.Gender;
                Entity.Mobile = entity.Mobile;
                Entity.Code = entity.Code;
                Entity.Degree = entity.Degree;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteDoctor(Model.Doctor entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Doctor.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Doctor.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
