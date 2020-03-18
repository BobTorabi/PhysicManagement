using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PhysicManagement.Common
{
    public class JSON
    {
        public static string Serialize<T>(T entity)
        {
            var seri = new JavaScriptSerializer();
            return seri.Serialize(entity);
        }
        public static T Desrialize<T>(string json)
        {
            var seri = new JavaScriptSerializer();
            return seri.Deserialize<T>(json);
        }
    }
}
