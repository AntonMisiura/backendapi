using System.ComponentModel.DataAnnotations;

namespace backend.auth.api.Model
{
    public class ExternalLoginModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
