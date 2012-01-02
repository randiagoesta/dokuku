using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Dokuku.Invoice.Models
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public bool Authenticated { get; set; }
        public string Message { get; set; }
    }
}