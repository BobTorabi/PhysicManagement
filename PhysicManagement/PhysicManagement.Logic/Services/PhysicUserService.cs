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
    public class PhysicUserService
    {
        #region PhysicUser Authorization

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
        public static PhysicUser IsAuthenticated()
        {
            var cookieValue = ReadAuthCookies();
            if (cookieValue.HasValue)
            {
                return GetUserDate(cookieValue.Value.userName, cookieValue.Value.PassWord);
            }
            return null;
        }
        public static(string userName,string PassWord)? ReadAuthCookies()
        {
            string cookievalue = Cookie.ReadCookie(AuthenticationCookieName);
            if (string.IsNullOrEmpty(cookievalue))
                return null;

            if (Cryptography.TryDecrypt(cookievalue,SecretKey,out string decryptedCookie))
            {
                string[] decryptedArray = System.Text.RegularExpressions.Regex.Split(decryptedCookie, Delimeter);
                string userName = decryptedArray[0];
                string passWord = decryptedArray[1];
            }
            else 
            {
                return null;            
            }
        }
        public static void SetAuthenticationCookie(string userName,string Password, bool rememberMe)
        { }
        public static PhysicUser GetUserByUserId(long userId)
        { }
        public static PhysicUser GetUserByUserName(string userName)
        { }
        public static PhysicUser GetUserByUserNameAndMobile(string userName,string mobile)
        { }
        public static PhysicUser GetUserDate(string userName,string passWord)
        { }
        public static bool IsUserValidByUserName(string userName)
        { }
        public static bool IsUserValidByUserId(long userId)
        { }
        public static bool IsUserDateValid(string userName,string passWord)
        { }
        public static bool Register(string userName, string firstName, string lastName, string passWord, string mobileNo, string degree, string description)
        { }
        public static bool UpdateProfile(int id, string userName, string firstName, string lastName, string mobileNo)
        { }
        public static bool LockUser(int id)
        { }
        public static bool Logout()
        { }
        public static bool ChangeUserPassword(string userName, string oldPassword, string newPassword)
        { }
        public static bool GetUserPasswordByMobile(string userName, string passWord)
        { }
        public static bool EncryptPassword(string userName, string passWord)
        { }
        internal static string DecryptPassword(string userName, string encryptedPassword)
        { }

        #endregion
        #region PhysicUser section

        public List<Model.PhysicUser> GetPhysicUserList()
        { 
            using(var db = new Model.PhysicManagementEntities())
            {
                return db.PhysicUser.OrderBy(x => x.FirstName).ToList();
            }
        }
        public Model.PhysicUser GetPhysicUserById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUser.Find(entityId);
                return Entity;
            }
        }
        public bool AddPhysicUser(Model.PhysicUser entity)
        {
            var vallidtion = new PhysicUserValidation.PhysicUserEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);
            
            using (var db = new Model.PhysicManagementEntities())
            {
                db.PhysicUser.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePhysicUser(Model.PhysicUser entity)
        {
            var vallidtion = new PhysicUserValidation.PhysicUserEntityValidation().Validate(entity);
            if (!vallidtion.IsValid)
                throw new ValidationException(vallidtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUser.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Username = entity.Username;
                Entity.Password = entity.Password;
                Entity.Mobile = entity.Mobile;
                Entity.Description = entity.Description;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePhysicUser(Model.PhysicUser entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.PhysicUser.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");
                db.PhysicUser.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
    }
}
