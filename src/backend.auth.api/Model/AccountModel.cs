using System.ComponentModel.DataAnnotations;

namespace backend.auth.api.Model
{
    public class AccountModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool AgreedToTerms { get; set; }

    }
}
