using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Voodle.Web.Filters
{
    public class WebServiceCompressGZIP : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            string acceptEncoding = actContext.Request.Headers.AcceptEncoding.ToString();
            if (acceptEncoding == "GZIP")
            {
                var content = actContext.Response.Content;
                var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
                var compressed = bytes == null ? new byte[0] : CompressionHelper.GZIPBytes(bytes);

                actContext.Response.Content = new ByteArrayContent(compressed);
                actContext.Response.Content.Headers.Remove("Content-Type");
                actContext.Response.Content.Headers.Add("Content-encoding", "GZIP");
                actContext.Response.Content.Headers.Add("Content-Type", "application/json");
            }
            base.OnActionExecuted(actContext);
        }
    }

    class CompressionHelper
    {
        //public static byte[] DeflateBytes(byte[] str)
        //{
        //    if (str == null)
        //        return null;

        //    using (var output = new MemoryStream())
        //    {
        //        using (var compressor = new DeflateStream(output, CompressionMode.Compress))
        //            compressor.Write(str, 0, str.Length);

        //        return output.ToArray();
        //    }
        //}

        public static byte[] GZIPBytes(byte[] str)
        {
            if (str == null)
                return null;

            using (var output = new MemoryStream())
            {
                using (var compressor = new GZipStream(output, CompressionMode.Compress))
                    compressor.Write(str, 0, str.Length);

                return output.ToArray();
            }
        }
    }
}
