using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dokuku.Invoice.Models
{
    public static class LogHelper
    {
        public static void LogCookies(this Nancy.NancyModule module, string msg)
        {
            if (module.Request == null || module.Request.Cookies == null)
                return;

            string logMsg = "";
            foreach (string s in module.Request.Cookies.Values)
            {
                logMsg += s + ";";
            }
            Common.Logging.LogManager.GetLogger("dokuku").Debug("Cookies: "+ msg + " => " + logMsg);
        }
    }
}