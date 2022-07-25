using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Dto.Entities
{
    public class CustomerDto : EntityBase
    {
        [Required]
        public Guid IdentificationTypeId { get; set; }

        public string IdentificationType { get; set; }

        [Required]
        public long IdentificationNumber { get; set; }

        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(250)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime Birthday { get; set; }

        public short Gender { get; set; }
    }
}
