using System.ComponentModel.DataAnnotations;

namespace Restaurant.Backend.Entities.Entities
{
    public class Country : EntityBase
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public int CallingCode { get; set; }

        [MaxLength(150)]
        public string Capital { get; set; }

        [MaxLength(150)]
        public string Region { get; set; }

        [MaxLength(150)]
        public string SubRegion { get; set; }

        [MaxLength(150)]
        public string Flag { get; set; }

    }
}
