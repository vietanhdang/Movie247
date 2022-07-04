using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class Person
    {
        public Person()
        {
            MovieCasts = new HashSet<MovieCast>();
            MovieCrews = new HashSet<MovieCrew>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public double? Popularity { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
        public virtual ICollection<MovieCrew> MovieCrews { get; set; }
    }
}
