using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using bfnexchange.Services.Services;
using System.ServiceModel;

namespace bfnexchange.Services
{
    public class ValidateUser : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new SecurityTokenException("Username and password required");
            var repository = new UserServices();
            if (!repository.IsValidUser(userName, password))
                throw new FaultException(string.Format("Wrong username ({0}) or password ", userName));
        }
    }
}