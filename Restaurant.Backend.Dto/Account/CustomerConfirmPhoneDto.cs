using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Dto.Account
{
    public class CustomerConfirmPhoneDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string UniquePhoneKey { get; set; }
        public int Code { get; set; }
    }
}
