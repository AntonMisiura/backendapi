using backend.auth.api.Contracts;

namespace backend.auth.api.Security
{
    public class LoginHandlerFactory
    {
        public static ILoginHandler GetHandler(string loginType)
        {
            switch (loginType)
            {
                case "Login":
                    return new LoginHandler();

                case "FacebookLogin":
                    return new FacebookLoginHandler();

                case "GoogleLogin":
                    return new GoogleLoginHandler();

                default: return new LoginHandler();
            }
        }
    }
}
