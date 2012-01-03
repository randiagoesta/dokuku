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
        const string ADMIN_ROLE = "admin";
        const string OWNER_ROLE = "owner";
        const string ACCOUNT_TYPE = "account";
        static CouchClient couchClient;
        static AuthRepository()
        {
            couchClient = new CouchClient("tcloud1.bonastoco.com", 5984, "admin", "S31panas", false, AuthenticationType.Basic);
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            CouchDatabase db = couchClient.GetDatabase("dokuku");
            ViewResult<Account> users = db.View<Account>("all_accounts", "view_accounts");
            var userRecord = users.Items.Where(acc => acc._id == identifier).FirstOrDefault();

            return userRecord == null
                       ? null
                       : new UserIdentity { UserName = userRecord.Email, Claims = userRecord.Roles };
        }

        public static Guid? ValidateUser(string email, string password)
        {
            CouchDatabase db = couchClient.GetDatabase("dokuku");
            ViewResult<Account> users = db.View<Account>("all_accounts", "view_accounts");
            var userRecord = users.Items.Where(usr => usr.Email == email && usr.Password == password).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord._id;
        }

        public static Guid? SignUp(string email, string password)
        {
            CouchDatabase db = couchClient.GetDatabase("dokuku");
            ViewResult<Account> users = db.View<Account>("all_accounts", "view_accounts");
            var userRecord = users.Items.Where(usr => usr.Email == email).FirstOrDefault();
            if(userRecord!=null)
                throw new ApplicationException(String.Format("{0} sudah terdaftar", userRecord.Email));

            Document<Account> account = new Document<Account>(new Account
            {
                Email = email,
                Password = password,
                Roles = new string[2] { OWNER_ROLE, ADMIN_ROLE },
                Type = ACCOUNT_TYPE
            });

            db.CreateDocument(account);

            return ValidateUser(email, password);
        }

        private class Account
        {
            public Guid _id { get; set; }
            public string _rev { get; set; }
            public string Email {get;set;}
            public string Password { get; set; }
            public string[] Roles { get; set; }
            public string Type { get; set; }
        }
    }
}