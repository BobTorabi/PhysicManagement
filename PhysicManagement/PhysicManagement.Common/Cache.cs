using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PhysicManagement.Common
{
    public class Cache
    {
        public static void Set(string name, object value, int inMinutes)
        {
            HttpContext.Current.Cache.Add(
                name,
                value,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, inMinutes, 0),
                System.Web.Caching.CacheItemPriority.Default,
                null);
        }
        public static object Get(string name)
        {
            return HttpContext.Current.Cache[name];
        }
        public static void Dispose(string name)
        {
            HttpContext.Current.Cache.Remove(name);
        }

    }
}
