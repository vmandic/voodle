using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Voodle.Utility
{
    public static class Logger
    {
        public static void Log(Exception exc, HttpRequestBase request = null, int iD = -1, String userName = "")
        {

            StreamWriter objSw = null;
            String requestMsg = "";
            String iDStr = "";

            if (iD > -1)
                iDStr = iD.ToString();

            //Extracting from request.Headers into string
            if (request != null)
            {
                int loop1, loop2;
                NameValueCollection coll;

                // Load Header collection into NameValueCollection object.
                coll = request.Headers;

                // Put the names of all keys into a string array.
                String[] arr1 = coll.AllKeys;
                for (loop1 = 0; loop1 < arr1.Length; loop1++)
                {
                    requestMsg += arr1[loop1] + ": ";
                    // Get all values under this key.
                    String[] arr2 = coll.GetValues(arr1[loop1]);
                    for (loop2 = 0; loop2 < arr2.Length; loop2++)
                    {
                        requestMsg += arr2[loop2] + Environment.NewLine;
                    }
                }
                //attaching URL
                requestMsg += "URL: " + request.Url.AbsoluteUri + Environment.NewLine;
            }

            String message = GetExceptionMessage(exc, false, "", requestMsg, iDStr, userName);

            string sFolderName = @"C:\AppErrors\";

            if (!Directory.Exists(sFolderName))
                Directory.CreateDirectory(sFolderName);

            string sFilePath = sFolderName + "Error.log";

            objSw = new StreamWriter(sFilePath, true);
            objSw.WriteLine(message + Environment.NewLine);

            if (objSw != null)
            {
                objSw.Flush();
                objSw.Dispose();
            }

            //if (sendMail)
            //    SendErrorMail(message);

        }

        public static string GetExceptionMessage(Exception ex, bool isInnerException = false, string err = "", String requestMsg = "", String iD = "", String userName = "")
        {
            String lines = "----------------------------------------------------------------------------------";
            var message = FormatException(ex);
            err += isInnerException ? "[INNER EXCEPTION]" : "[EXCEPTION]";
            if (requestMsg != "" && iD != "")
                err += String.Format("[{0}]" + Environment.NewLine +
                    "USER(ID): {1}({2})" + Environment.NewLine +
                    "{3}" + Environment.NewLine +
                    "{4}" +
                    "{5}" + Environment.NewLine +
                    "HTTP REQUEST:" + Environment.NewLine +
                    "{6}" +
                    "{7}" + Environment.NewLine +
                    Environment.NewLine + Environment.NewLine,
                    DateTime.Now.ToString(), userName, iD, lines, message, lines, requestMsg, lines);
            else if (requestMsg != "")
            {
                err += String.Format("[{0}]" + Environment.NewLine +
                    "{1}" + Environment.NewLine +
                    "{2}" +
                    "{3}" + Environment.NewLine +
                    "HTTP REQUEST:" + Environment.NewLine +
                    "{4}" +
                    "{5}" + Environment.NewLine +
                    Environment.NewLine + Environment.NewLine,
                    DateTime.Now.ToString(), lines, message, lines, requestMsg, lines);
            }
            else
            {
                err += String.Format("[{0}]" + Environment.NewLine +
                    "{1}" + Environment.NewLine +
                    "{2}" +
                    "{3}" + Environment.NewLine +
                    Environment.NewLine + Environment.NewLine,
                    DateTime.Now.ToString(), lines, message, lines);
            }

            if (ex.InnerException != null)
                return GetExceptionMessage(ex.InnerException, true, err, requestMsg, iD, userName);
            else
                return err;
        }

        private static string FormatException(Exception ex)
        {
            return "MESSAGE: " + ex.Message + Environment.NewLine + "STACKTRACE: " + ex.StackTrace + Environment.NewLine;
        }
    }
}
