using backend.auth.api.Contracts;
using backend.auth.api.Entities;

namespace backend.auth.api.Service
{
    public class SignUpService : ISignUpService
    {
        private ISignUpRepository _repository;

        public SignUpService(ISignUpRepository repository)
        {
            _repository = repository;
        }

        public User GetOrCreate(User user)
        {
            var dbUser = _repository.FindByEmail(user.Email);

            if (dbUser == null)
            {
                _repository.Create(user);
                return user;
            }

            return dbUser;
        }
    }
}
