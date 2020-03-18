using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace PhysicManagement.Common
{
    public class GZip
    {
        private static void EnableGzip(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            Stream uncompressedStream = response.Filter;

            if (!(context.CurrentHandler is Page || context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") || request["HTTP_X_MICROSOFTAJAX"] != null)
                return;
            if (string.IsNullOrEmpty(acceptEncoding))
                return;

            //else
            acceptEncoding = acceptEncoding.ToLower();
            if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            {
                response.Filter = new System.IO.Compression.DeflateStream(uncompressedStream, System.IO.Compression.CompressionMode.Compress);
                response.AppendHeader("Content-Encoding", "deflate");
            }
            else if (acceptEncoding.Contains("gzip"))
            {
                response.Filter = new System.IO.Compression.GZipStream(uncompressedStream, System.IO.Compression.CompressionMode.Compress);
                response.AppendHeader("Content-Encoding", "gzip");
            }
        }
        public static void CompressCurrentPage()
        {
            EnableGzip(HttpContext.Current);
        }
    }
}
