using backend.auth.api.Entities;

namespace backend.auth.api.Contracts
{
    public interface ISignUpRepository
    {
        void Create(User user);

        User FindByEmail(string email);
    }
}
