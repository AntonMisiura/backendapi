using System;
using System.ComponentModel.DataAnnotations;
using backend.auth.api.Contracts;
using backend.auth.api.Entities;

namespace backend.auth.api.Security
{
    public class LoginHandler : ILoginHandler
    {
        public bool ValidateUserData(User user)
        {
            if (String.IsNullOrEmpty(user.Email) && String.IsNullOrEmpty(user.Password.ToString())
                && user.Password.ToString().Length < 6)
            {
                return false;
            }

            var isValid = new EmailAddressAttribute().IsValid(user.Email);
            return isValid;
        }
    }
}
