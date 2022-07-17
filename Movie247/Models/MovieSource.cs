using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieSource
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string LinkSource { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string Type { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
