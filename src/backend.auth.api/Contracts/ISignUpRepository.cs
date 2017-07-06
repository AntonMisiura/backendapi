using backend.auth.api.Entities;

namespace backend.auth.api.Contracts
{
    public interface ISignUpRepository
    {
        void Save(User user);

        User FindByEmail(string email);
    }
}
