using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieCountry
    {
        public string CountryId { get; set; }
        public int MovieId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Id { get; set; }

        public virtual ProductionCountry Country { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
