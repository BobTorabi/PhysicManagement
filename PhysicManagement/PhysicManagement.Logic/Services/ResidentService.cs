using FluentValidation;
using PhysicManagement.Common;
using PhysicManagement.Logic.Validations;
using PhysicManagement.Logic.ViewModels;
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

        public static bool Register(Resident entity)
        {
            Resident UserEntity = new Resident()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Mobile = entity.Mobile,
                Password = EncryptPassword(entity.Username, entity.Password),
                Username = entity.Username,
                Code = entity.Code,
                Gender = entity.Gender,
                Description = entity.Description,
                DoctorId = entity.DoctorId,

                IsActive = true
            };

            var validation = new ResidentValidation.ResidentEntityValidation().Validate(UserEntity);
            if (validation.IsValid)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    if (IsUserValidByUserName(UserEntity.Username))
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

        public static bool UpdateResident(Resident entity)
        {
            var validation = new ResidentValidation.ResidentEntityValidation().Validate(entity);
            if (validation.IsValid)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var currentUser = db.Resident.Find(entity.Id);
                    currentUser.FirstName = entity.FirstName;
                    currentUser.LastName = entity.LastName;
                    currentUser.Mobile = entity.Mobile;
                    currentUser.DoctorId = entity.DoctorId;
                    currentUser.Password = EncryptPassword(entity.Username, entity.Password);

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

            ResidentService.UpdateResident(userEntity);
            return true;
        }
        public static bool Logout()
        {
            Cookie.ExpireCookie(AuthenticationCookieName);
            return true;
        }

        public static bool ChangeUserPassword(int userId, string oldPassword, string newPassword)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var UserData = GetUserByUserId(userId);
                string encryptedOldPassword = EncryptPassword(UserData.Username, oldPassword);
                var userData = GetUserData(UserData.Username, oldPassword);
                if (userData == null)
                    throw MegaException.ThrowException("رمز وارد شده اشتباه است.");

                string encryptedNewPassword = EncryptPassword(userData.Username, newPassword);
                UserData.Password = encryptedNewPassword;

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
            return Cryptography.Encrypt(passWord);
        }

        public static string DecryptPassword(string userName, string encryptedPassword)
        {
            return Cryptography.Decrypt(encryptedPassword);
        }

        #endregion
        #region Resident Section
        public PagedList<Model.Resident> GetResidentList(string firstName, string lastName, string mobile, int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Model.Resident> QueryableResident = db.Resident;

                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    QueryableResident = QueryableResident.Where(x => x.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    QueryableResident = QueryableResident.Where(x => x.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    QueryableResident = QueryableResident.Where(x => x.Mobile.Contains(mobile));
                }
                QueryableResident = QueryableResident.OrderByDescending(x => x.IsActive);
                return new ViewModels.PagedList<Model.Resident>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryableResident.Count(),
                    Records = QueryableResident.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
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
        //public bool UpdateResident(Model.Resident entity)
        //{
        //    var vallidtion = new ResidentValidation.ResidentEntityValidation().Validate(entity);
        //    if (!vallidtion.IsValid)
        //        throw new ValidationException(vallidtion.Errors);

        //    using (var db = new Model.PhysicManagementEntities())
        //    {
        //        var Entity = db.Resident.Find(entity.Id);
        //        Entity.FirstName = entity.FirstName;
        //        Entity.LastName = entity.LastName;
        //        Entity.Username = entity.Username;
        //        Entity.Password = entity.Password;
        //        Entity.Mobile = entity.Mobile;
        //        Entity.Description = entity.Description;

        //        return db.SaveChanges() == 1;
        //    }
        //}
        public bool DeleteResident(int entityId)
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
