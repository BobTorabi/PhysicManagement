using System.Web;

namespace PhysicManagement.Common
{
    public class Network
    {
        public static string GetIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }
        public static string BrowserName()
        {
            string BrowserName = HttpContext.Current.Request.Browser.Browser;
            return BrowserName;
        }
    }
}
