using backend.auth.api.Entities;
using System.Linq;
using backend.auth.api.Contracts;
using backend.auth.api.Data;

namespace backend.auth.api.Repo
{
    public class SignUpRepository : ISignUpRepository
    {
        public ApplicationDbContext Context { get; set; }

        public SignUpRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public void Create(User user)
        {
            if (user != null)
            {
                Context.Set<User>().Add(user);
            }
        }

        public User FindByEmail(string email)
        {
            return Context.Set<User>().FirstOrDefault(u => u.Email == email);
        }
    }
}
