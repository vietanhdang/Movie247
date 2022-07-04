using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class Keyword
    {
        public Keyword()
        {
            MovieKeywords = new HashSet<MovieKeyword>();
        }

        public int Id { get; set; }
        public string KeywordName { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<MovieKeyword> MovieKeywords { get; set; }
    }
}
