using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Dto.Account
{
    public class CustomerConfirmEmailDto
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required]
        public string EmailKey { get; set; }
    }
}
