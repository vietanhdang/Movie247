using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class ProductionCompany
    {
        public ProductionCompany()
        {
            MovieCompanies = new HashSet<MovieCompany>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<MovieCompany> MovieCompanies { get; set; }
    }
}
