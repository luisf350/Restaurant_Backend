using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Backend.Entities.Entities
{
    public class Customer : EntityBase
    {
        [Required]
        public Guid IdentificationTypeId { get; set; }

        [ForeignKey("IdentificationTypeId")]
        public IdentificationType IdentificationType { get; set; }

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

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public bool VerifiedPhoneNumber { get; set; }

        public bool VerifiedEmail { get; set; }

        public DateTime Birthday { get; set; }

        public short Gender { get; set; }

        public List<Address> Addresses { get; set; }

        public bool Active { get; set; }
    }
}
