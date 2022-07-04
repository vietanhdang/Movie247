using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class ProductionCountry
    {
        public ProductionCountry()
        {
            MovieCountries = new HashSet<MovieCountry>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<MovieCountry> MovieCountries { get; set; }
    }
}
