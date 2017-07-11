using backend.auth.api.Contracts;

namespace backend.auth.api.Security
{
    public class LoginHandlerFactory
    {
        public static ILoginHandler GetHandler(string loginType)
        {
            switch (loginType)
            {
                case "facebook":
                    return new FacebookLoginHandler();
                case "google":
                    return new GoogleLoginHandler();
                default:
                    return new AllegroLoginHandler();
            }
        }
    }
}
