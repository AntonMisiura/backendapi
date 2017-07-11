using System;
using System.ComponentModel.DataAnnotations;
using backend.auth.api.Contracts;
using backend.auth.api.Entities;
using Newtonsoft.Json;

namespace backend.auth.api.Security
{
    public class AllegroLoginHandler : ILoginHandler
    {
        public bool VerifyUserDetails(string email, string password)
        {
            if (String.IsNullOrEmpty(email) && String.IsNullOrEmpty(password.ToString())
                && password.ToString().Length < 6)
            {
                // handle failure..
            }

            var isValid = new EmailAddressAttribute().IsValid(email);

            if (!isValid)
            {
                // handle failure..
            }

            return isValid;
        }

        public string LoginType => "allegro";

        public User GetUser(string loginDataJson)
        {
            var allegroResponse = JsonConvert.DeserializeObject<User>(loginDataJson);

            VerifyUserDetails(allegroResponse.Email, allegroResponse.Password);
            
            return new User
            {
                Id = 1,
                Email = allegroResponse.Email,
                AccessToken = "",
                Password = allegroResponse.Password,
                UserName = allegroResponse.UserName
            };
        }
    }
}
