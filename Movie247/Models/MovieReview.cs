using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieReview
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Review { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public short? Vote { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
    }
}
