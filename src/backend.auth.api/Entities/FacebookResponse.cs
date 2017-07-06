using backend.auth.api.Contracts;

namespace backend.auth.api.Entities
{
    public class FacebookResponse
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }

        public FacebookResponse(string message, bool ok = true, string jwtToken = "")
        {
            this.Message = message;
            this.Ok = ok;
            this.JwtToken = jwtToken;
        }
    }
}