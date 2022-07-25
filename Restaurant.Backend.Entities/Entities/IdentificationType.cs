using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Entities.Entities
{
    public class IdentificationType : EntityBase
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
