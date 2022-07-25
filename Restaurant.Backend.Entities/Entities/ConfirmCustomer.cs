using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Backend.Entities.Entities
{
    public class ConfirmCustomer : EntityBase
    {
        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [MaxLength(500)]
        public string UniqueEmailKey { get; set; }

        public DateTimeOffset ExpirationEmail { get; set; }

        [MaxLength(150)]
        public string UniquePhoneKey { get; set; }


    }
}
