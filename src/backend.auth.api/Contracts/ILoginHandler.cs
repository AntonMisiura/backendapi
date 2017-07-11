using backend.auth.api.Entities;

namespace backend.auth.api.Contracts
{
    public interface ILoginHandler
    {
        /// <summary>
        /// Allegro, Faceebook, Google
        /// </summary>
        string LoginType { get; }
        User GetUser(string loginDataJson);
    }
}
