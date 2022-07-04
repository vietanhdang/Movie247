﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieCast
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public string Character { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Id { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
