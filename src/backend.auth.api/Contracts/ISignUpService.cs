using backend.auth.api.Entities;

namespace backend.auth.api.Contracts
{
    public interface ISignUpService
    {
        User GetOrCreate(User user);
    }
}
