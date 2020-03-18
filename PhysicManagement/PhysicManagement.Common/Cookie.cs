using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PhysicManagement.Common
{
    public class Cookie
    {
        public static bool SetCookie(string cookieName, string value)
        {

            HttpCookie aCookie = new HttpCookie(cookieName) { Value = value, HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(aCookie);
            return true;
        }
        public static bool SetCookie(string cookieName, string value, DateTime expireDate)
        {

            HttpCookie aCookie = new HttpCookie(cookieName) { Expires = expireDate, Value = value, HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(aCookie);
            return true;
        }
        // ReSharper disable MethodOverloadWithOptionalParameter
        public static bool SetCookie(string cookieName, string doamin, params string[] param)
        // ReSharper restore MethodOverloadWithOptionalParameter
        {

            HttpCookie aCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(+7) };
            string values = string.Join("$$", param);
            aCookie.Value = values;
            if (!string.IsNullOrEmpty(doamin))
                aCookie.Domain = doamin;
            HttpContext.Current.Response.Cookies.Add(aCookie);
            return true;
        }
        public static bool SetCookie(string cookieName, DateTime expireDate, params string[] param)
        {

            HttpCookie aCookie = new HttpCookie(cookieName) { Expires = expireDate };
            string values = string.Join("$$", param);
            aCookie.Value = values;
            HttpContext.Current.Response.Cookies.Add(aCookie);
            return true;
        }
        public static bool IsCookieSet(string cookieName)
        {
            return (HttpContext.Current.Request.Cookies[cookieName] != null);
        }
        public static bool ExpireCookie(string cookiName)
        {
            HttpCookie aCookie = new HttpCookie(cookiName) { Expires = DateTime.Now.AddDays(-10) };
            HttpContext.Current.Response.Cookies.Add(aCookie);
            return true;
        }
        public static string ReadCookie(string cookiName)
        {
            HttpCookie aCookie = HttpContext.Current.Request.Cookies[cookiName];
            if (aCookie ==null)
            {
                return null;
            }
            return aCookie.Value;
        }
      
    }

}
