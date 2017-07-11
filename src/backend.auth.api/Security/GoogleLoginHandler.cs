using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using backend.auth.api.Contracts;
using backend.auth.api.Entities;
using Newtonsoft.Json;

namespace backend.auth.api.Security
{
    public class GoogleLoginHandler : ILoginHandler
    {
        public string AccessToken { get; set; }
        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
        
        public bool VerifyUserDetails(string email, string accessToken)
        {
            var isValid = new EmailAddressAttribute().IsValid(email);

            if (!isValid && string.IsNullOrEmpty(email))
            {
                //handle failure..
            }

            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, accessToken));

            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                //handle failure..
            }

            if (httpResponseMessage == null && httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                //handle failure..
            }

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;

            return true;

            //var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);

            //if (googleApiTokenInfo == null)
            //{
            //    //hadle failure..
            //}
        }

        public string LoginType => "google";

        public User GetUser(string loginDataJson)
        {
            var googleResponse = JsonConvert.DeserializeObject<User>(loginDataJson);

            VerifyUserDetails(googleResponse.Email, googleResponse.AccessToken);

            return new User
            {
                Id = 1,
                Email = googleResponse.Email,
                UserName = googleResponse.UserName,
                AccessToken = googleResponse.AccessToken,
                Password = googleResponse.Password
            };
        }
    }
}
