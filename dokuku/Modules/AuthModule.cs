﻿using System.Dynamic;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using System;
using Nancy;
using Nancy.Responses;
using Dokuku.Invoice.Models;
using dokuku.security;
namespace FirstNancyApp.Modules
{
    public class AuthModule : Nancy.NancyModule
    {
        public AuthModule()
        {
            Get["/Scripts/{file}"] = p =>
            {
                string filename = p.file.ToString();
                return Response.AsJs("Scripts/" + filename);
            };

            Get["/Styles/{file}"] = p =>
            {
                string filename = p.file.ToString();
                return Response.AsCss("Styles/" + filename);
            };

            Get["/Contents/images/{name}"] = x =>
            {
                return Response.AsImage(string.Format("Contents/images/{0}", (string)x.name));
            };

            Get["/login"] = p =>
            {
                return View["login"];
            };

            Post["/login"] = x =>
            {
                var userGuid = AuthRepository.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Post["/signup"] = x =>
            {
                try
                {
                    var userGuid = AuthRepository.SignUp((string)this.Request.Form.Username, (string)this.Request.Form.Password);
                    return this.LoginAndRedirect(userGuid.Value, null, "~/");
                }
                catch (Exception ex)
                {
                    return Context.GetRedirect("~/signup?error=true&message=" + ex.Message);
                }
            };

            Get["/signup"] = p =>
            {
                return View["signup"];
            };

            Get["/signup/validate/{username}"] = p =>
            {
                try
                {
                    var user = AuthRepository.GetUserFromUsername(p.username);
                    if (user != null)
                        return Response.AsJson(new {Registered = true, Error = false });
                    else
                        return Response.AsJson(new { Registered = false, Error = false });                   
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { Message = ex.Message, Error = true });
                }
            };

            Get["/logout"] = x =>
            {
                return this.LogoutAndRedirect("~/");
            };
        }
    }
}