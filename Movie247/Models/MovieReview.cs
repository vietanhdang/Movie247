using Movie247.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Movie247.Models
{
    public partial class MovieReview
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public short? Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Movie247User User { get; set; }
    }
}
