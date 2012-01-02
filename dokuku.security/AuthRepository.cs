namespace dokuku.security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Nancy.Authentication.Forms;
    using Nancy.Security;

    using LoveSeat;
    using LoveSeat.Interfaces;

    public class AuthRepository : IUserMapper
    {
        static CouchClient couchClient;
        static AuthRepository()
        {
            couchClient = new CouchClient("tcloud1.bonastoco.com", 5984, "admin", "S31panas", false, AuthenticationType.Basic);
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            CouchDatabase db = couchClient.GetDatabase("dokuku");
            
            ViewResult<User> users = db.View<User>("all_organizations", "organization_views");
            var userRecord = users.Items.Where(usr => usr._id == identifier).FirstOrDefault();

            return userRecord == null
                       ? null
                       : new UserIdentity { UserName = userRecord.Email, Claims = userRecord.Roles };
        }

        public static Guid? ValidateUser(string email, string password)
        {
            CouchDatabase db = couchClient.GetDatabase("dokuku");
            ViewResult<User> users = db.View<User>("all_organizations", "organization_views");
            var userRecord = users.Items.Where(usr => usr.Email == email && usr.Password == password).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord._id;
        }

        public class User
        {
            public Guid _id { get; set; }
            public string _rev { get; set; }
            public string Email {get;set;}
            public string Password { get; set; }
            public string[] Roles { get; set; }
        }
    }
}