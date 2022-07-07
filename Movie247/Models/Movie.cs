using System;
using System.Collections.Generic;

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
            MovieGenres = new HashSet<MovieGenre>();
            MovieKeywords = new HashSet<MovieKeyword>();
            MovieSources = new HashSet<MovieSource>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
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
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MovieKeyword> MovieKeywords { get; set; }
        public virtual ICollection<MovieSource> MovieSources { get; set; }
        public virtual ICollection<Favourite> FavouriteMovies { get; set; }
    }
}
