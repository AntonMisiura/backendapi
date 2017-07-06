using System;
using System.Net;
using System.Net.Http;
using backend.auth.api.Contracts;
using backend.auth.api.Entities;
using Newtonsoft.Json;

namespace backend.auth.api.Security
{
    public class GoogleLoginHandler : ILoginHandler
    {
        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        public User GetUserDetails(string accessToken)
        {
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, accessToken));

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                return null;
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);

            if (!SupportedClientsIds.Contains(googleApiTokenInfo.Aud))
            {
                //Logger:("Google API Token Info aud field ({0}) not containing the required client id", googleApiTokenInfo.aud);
                return null;
            }

            return new User
            {
                Id = googleApiTokenInfo.Sub,
                Email = googleApiTokenInfo.Email,
                UserName = googleApiTokenInfo.Name
            };
        }
    }
}
