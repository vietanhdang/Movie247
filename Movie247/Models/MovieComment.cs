using Movie247.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie247.Models
{
    public class MovieComment
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? ParentMovieId { get; set; }
        public ICollection<MovieComment> MovieCommentChildren { get; set; }

        [ForeignKey("ParentMovieId")]
        public MovieComment ParentMovieComment { set; get; }
        public virtual Movie Movie { get; set; }
        public virtual Movie247User User { get; set; }
    }
}
