using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Dto.Entities
{
    public class IdentificationTypeDto : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
