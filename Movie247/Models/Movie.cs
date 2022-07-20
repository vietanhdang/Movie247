using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Movie247.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieCasts = new HashSet<MovieCast>();
            MovieCompanies = new HashSet<MovieCompany>();
            MovieCountries = new HashSet<MovieCountry>();
            MovieCrews = new HashSet<MovieCrew>();
            MovieFavourites = new HashSet<MovieFavourite>();
            MovieGenres = new HashSet<MovieGenre>();
            MovieReviews = new HashSet<MovieReview>();
            MovieSources = new HashSet<MovieSource>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
        [Range(0, 10)]
        public double? ImdbAverage { get; set; }
        public long? Views { get; set; }
        public string Duration { get; set; }
        public string Trailer { get; set; }
        public bool? MovieStatus { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
        public virtual ICollection<MovieCompany> MovieCompanies { get; set; }
        public virtual ICollection<MovieCountry> MovieCountries { get; set; }
        public virtual ICollection<MovieCrew> MovieCrews { get; set; }
        public virtual ICollection<MovieFavourite> MovieFavourites { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MovieReview> MovieReviews { get; set; }
        public virtual ICollection<MovieSource> MovieSources { get; set; }
        public virtual ICollection<MovieComment> MovieComments { get; set; }
    }
}
