using AutoMapper;
using backend.auth.api.Entities;
using backend.auth.api.Model;

namespace backend.auth.api.Service
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("UserProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {
            CreateMap<AccountModel, User>();
        }
    }
}
