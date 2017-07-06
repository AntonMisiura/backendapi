using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using backend.auth.api.Contracts;
using backend.auth.api.Entities;
using Newtonsoft.Json;

namespace backend.auth.api.Security
{
    public class FacebookLoginHandler : ILoginHandler
    {
        public async Task<FacebookMeResponse> VerifyAccessToken(string email, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ValidationException("Invalid Facebook token");
            }

            var facebookGraphUrl = "https://graph.facebook.com/me?fields=cover,age_range,first_name,location,last_name,hometown,gender,birthday,email&access_token=" + accessToken;
            var request = WebRequest.Create(facebookGraphUrl);
            request.Credentials = CredentialCache.DefaultCredentials;

            using (var response = await request.GetResponseAsync())
            {
                var status = ((HttpWebResponse)response).StatusCode;

                var dataStream = response.GetResponseStream();

                var reader = new StreamReader(dataStream);
                var responseFromServer = reader.ReadToEnd();
                var facebookUser = JsonConvert.DeserializeObject<FacebookMeResponse>(responseFromServer);

                var valid = facebookUser != null && !string.IsNullOrWhiteSpace(facebookUser.Email) && facebookUser.Email.ToLower() == email.ToLower();
                facebookUser.PublicProfilePhotoUrl = "http://graph.facebook.com/" + facebookUser.Id + "/picture";

                if (!valid)
                {
                    throw new ValidationException("Invalid Facebook token");
                }

                return facebookUser;
            }
        }
    }
}
