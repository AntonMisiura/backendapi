using System.ComponentModel.DataAnnotations;

namespace backend.auth.api.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Password { get; set; }
        public string AccessToken { get; set; }
    }
}
