using backend.auth.api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.auth.api
{
    public class AuthContext : DbContext
    {
        //TODO:config to connect DB
        private IConfigurationRoot _config;
        private DbContextOptions _options;

        public AuthContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
            _options = options;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:MY_CONNECTION_STRING"]);
        }
    }
}