using System.Collections.Generic;

namespace Restaurant.Backend.Dto.Entities
{
    public class CountryDto : EntityBase
    {
        public string Name { get; set; }

        public List<string> CallingCodes { get; set; }

        public string Capital { get; set; }

        public string Region { get; set; }

        public string SubRegion { get; set; }

        public string Flag { get; set; }

        public int CallingCode { get; set; }
    }
}
