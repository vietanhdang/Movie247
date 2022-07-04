using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieKeyword
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int KeywordId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
