using FluentValidation;
using PhysicManagement.Common;
using PhysicManagement.Logic.Validations;
using PhysicManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhysicManagement.Logic.Services
{
    public class ResidentService
    {
        #region Resident Authorization
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
        public static Resident IsAuthenticated()
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

        public static Resident GetUserByUserId(long userId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Resident.Where(x => x.Id == userId && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }
        }

        public static Resident GetUserByUserName(string userName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }
        }

        public static Resident GetUserByUserNameAndMobile(string userName, string mobile)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.Mobile == mobile && x.IsActive == true).OrderBy(x => x.FirstName).FirstOrDefault();
            }

        }

        public static bool IsUserValidByUserName(string userName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.IsActive == true).Count();
                return UserExists != 0;
            }
        }

        public static bool IsUserValidByUserId(long userId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.Resident.Where(x => x.Id == userId && x.IsActive == true).Count();
                return UserExists == 1;
            }

        }


        public static bool IsUserDataValid(string userName, string passWord)
        {
            string EncryptedPassword = EncryptPassword(userName, passWord);
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.Password == EncryptedPassword && x.IsActive == true).Count();
                return UserExists == 1;
            }

        }

        public static Resident GetUserData(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            string EncryptedPassword = EncryptPassword(userName, password);
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserExists = db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.Password == EncryptedPassword && x.IsActive == true).FirstOrDefault();
                return UserExists;
            }
        }

        public static bool Register(string userName, string firstName, string lastName, string passWord, string mobileNo, string code, string description, string gender, int doctorId)
        {
            Resident UserEntity = new Resident()
            {

                FirstName = firstName,
                IsActive = true,
                Mobile = mobileNo,
                LastName = lastName,
                Password = EncryptPassword(userName, passWord),
                Username = userName,
                Code = code,
                Gender = gender,
                Description = description,
                DoctorId = doctorId
            };

            var validation = new ResidentValidation.ResidentEntityValidation().Validate(UserEntity);
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
                        db.Resident.Add(UserEntity);
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
            var validation = new ResidentValidation.ResidentEntityValidation().Validate(currentUser);
            if (validation.IsValid)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    db.Resident.Add(currentUser);
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

            ResidentService.UpdateProfile(userEntity.Id, userEntity.Username, userEntity.FirstName, userEntity.LastName, userEntity.Mobile);
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
                var Entity = db.Resident.Find(userData.Id);
                Entity.Password = userData.Password;
                return db.SaveChanges() == 1;
            }
        }

        public static string GetUserPasswordByMobileOrEmail(string userName, string mobile)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var userPassword = db.Resident.Where(x => x.Username.ToLower() == userName.ToLower() && x.Mobile == mobile && x.IsActive == true).Select(x => x.Password).FirstOrDefault();
                if (string.IsNullOrEmpty(userPassword))
                    throw Common.MegaException.ThrowException("چنین کاربری پیدا نشد.");
                string decryptedPassword = DecryptPassword(userName, userPassword);
                return decryptedPassword;
            }

        }

        public static string EncryptPassword(string userName, string passWord)
        {
            string userNameKey = userName.GetTypeCode().ToString();
            return Cryptography.EncryptByUV(userNameKey, passWord);
        }

        public static string DecryptPassword(string userName, string encryptedPassword)
        {
            string userNameKey = userName.GetTypeCode().ToString();
            return Cryptography.Decrypt(encryptedPassword, userNameKey);
        }

        #endregion
        #region Resident Section
        public List<Model.Resident> GetResidentList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Resident.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.Resident GetResidentById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entityId);
                return Entity;
            }
        }
        public bool AddResident(Model.Resident entity)
        {
            var vallidtion = new ResidentValidation.ResidentEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Resident.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateResident(Model.Resident entity)
        {
            var vallidtion = new ResidentValidation.ResidentEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Username = entity.Username;
                Entity.Password = entity.Password;
                Entity.Mobile = entity.Mobile;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteResident(Model.Resident entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Resident.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");
                db.Resident.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion

    }
}
