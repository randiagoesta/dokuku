using Nancy;
using Nancy.Security;
using Nancy.ViewEngines.Razor;
using FirstNancyApp.Models;
using System.Dynamic;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using System;
using Common.Logging;
using Dokuku.Invoice.Models;
namespace FirstNancyApp.Modules
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            this.RequiresAuthentication();

            Get["/"] = p =>
            {
                return View["index"];
            };
        }
    }
}