using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Backend.Entities.Entities
{
    public class Address : EntityBase
    {
        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        public string AddressLine2 { get; set; }

        [Required]
        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string Region { get; set; }

        [MaxLength(15)]
        public string PostalCode { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        public bool MainAddress { get; set; }

    }
}
