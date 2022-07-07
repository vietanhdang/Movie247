using System;
using System.Collections.Generic;
using Movie247.Areas.Identity.Data;

namespace Movie247.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Movie247User User { get; set; }


    }
}
