using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieGenre
    {
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Id { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
