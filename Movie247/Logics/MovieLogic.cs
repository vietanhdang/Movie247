using Movie247.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Movie247.Helpers;
using System.Threading.Tasks;
using System;

namespace Movie247.Logics
{
    public class MovieLogic
    {
        // create  _context to access db
        private readonly MOVIEPROJECTContext _context;

        public MovieLogic(MOVIEPROJECTContext context)
        {
            _context = context;
        }


        // Get 5 trailer of movie latest
        public List<Movie> Get5TrailerOfMovieLatest()
        {
            List<Movie> movies = new();
            movies = _context.Movies.OrderByDescending(m => m.ReleaseDate).Take(5).ToList();
            return movies;
        }
        // Get 12 movie with highest rate
        public List<Movie> Get12MovieWithHighestRate()
        {
            List<Movie> movies = new();
            movies = _context.Movies
                .OrderByDescending(m => m.ImdbAverage)
                .Take(12)
                .Include(x => x.MovieGenres)
                .ThenInclude(x => x.Genre).ToList();
            return movies;
        }
        // get latest and oldest year of movie
        public (int, int) GetLatestAndOldestYearOfMovie()
        {
            int latestYear = _context.Movies.Max(m => m.ReleaseDate.Year);
            int oldestYear = _context.Movies.Min(m => m.ReleaseDate.Year);
            return (latestYear, oldestYear);
        }
        // Get movie by multiple condition
        public List<Movie> GetMovieByMultipleCondition(int Offset, int Count)
        {
            List<Movie> movies = new List<Movie>();
            movies = _context.Movies.OrderByDescending(m => m.ReleaseDate).Skip(Offset - 1).Take(Count).ToList();
            return movies;
        }
        public List<Movie> GetMovieByMultipleCondition(FilterModel filter)
        {
            var movies = from m in _context.Movies
                         select m;
            if (filter.Name != null)
            {
                movies = movies.Where(m => m.Title.ToLower().Contains(filter.Name.ToLower()));
            }
            if (filter.CompanyId != null)
            {
                movies = from m in movies
                         join c in _context.MovieCompanies on m.Id equals c.MovieId
                         where filter.CompanyId.Contains(c.CompanyId)
                         select m;
            }
            if (filter.CountryId != null)
            {
                movies = from m in movies
                         join c in _context.MovieCountries on m.Id equals c.MovieId
                         where filter.CountryId.Contains(c.CountryId)
                         select m;
            }
            if (filter.GenreId != null)
            {
                // select distinct movie by genre id
                movies = from m in movies
                         join g in _context.MovieGenres on m.Id equals g.MovieId
                         where filter.GenreId.Contains(g.GenreId)
                         select m;
            }
            movies = movies.Distinct();
            filter.TotalCount = movies.Distinct().Count();
            if (filter.TotalCount <= 0)
            {
                return movies.ToList();
            }
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalCount / filter.PageSize);

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }
            if (filter.Page > filter.TotalPages)
            {
                filter.Page = filter.TotalPages;
            }
            if (!(filter.PageSize == 12 || filter.PageSize == 24))
            {
                filter.PageSize = 12;
            }
            int from = (filter.Page - 1) * filter.PageSize + 1;
            if (filter.OrderBy == "desc")
            {
                if (filter.SortBy == "date")
                {
                    movies = movies.OrderByDescending(m => m.ReleaseDate);
                }
                else if (filter.SortBy == "popularity")
                {
                    movies = movies.OrderByDescending(m => m.ImdbAverage);
                }
                else if (filter.SortBy == "view")
                {
                    movies = movies.OrderByDescending(m => m.Views);
                }
            }
            else
            {
                if (filter.SortBy == "date")
                {
                    movies = movies.OrderBy(m => m.ReleaseDate);
                }
                else if (filter.SortBy == "popularity")
                {
                    movies = movies.OrderBy(m => m.ImdbAverage);
                }
                else if (filter.SortBy == "view")
                {
                    movies = movies.OrderBy(m => m.Views);
                }
            }
            return movies.Skip(from - 1).Take(filter.PageSize).ToList();
        }

        // Get movie by id
        public Movie GetMovieById(int id)
        {
            Movie movie = new Movie();
            movie = _context.Movies
                .Include(movie => movie.MovieCrews).ThenInclude(x => x.Person)
                .Include(movie => movie.MovieGenres).ThenInclude(x => x.Genre)
                .Include(movie => movie.MovieCompanies).ThenInclude(x => x.Company)
                .Include(movie => movie.MovieCountries).ThenInclude(x => x.Country)
                .Include(movie => movie.MovieCasts).ThenInclude(x => x.Person)
                .Include(movie => movie.MovieKeywords).ThenInclude(x => x.Keyword)
                .Include(movie => movie.MovieSources)
                .FirstOrDefault(movie => movie.Id == id);
            return movie;
        }

        public Movie GetSourceMovieById(int id)
        {
            Movie movie = new Movie();
            movie = _context.Movies
                .Include(movie => movie.MovieGenres).ThenInclude(x => x.Genre)
                .Include(movie => movie.MovieKeywords).ThenInclude(x => x.Keyword)
                .Include(movie => movie.MovieSources)
                .FirstOrDefault(movie => movie.Id == id);
            return movie;
        }
        // find 12 list movie in genre of movie by id
        public List<Movie> GetMovieInGenreById(List<MovieGenre> genres, int movieId)
        {
            List<int> genredId = genres.Select(x => x.GenreId).ToList();
            var movie = _context.Movies
                .Include(movie => movie.MovieGenres).ThenInclude(x => x.Genre)
                .Include(movie => movie.MovieCompanies).ThenInclude(x => x.Company)
                .Include(movie => movie.MovieCountries).ThenInclude(x => x.Country)
                .Include(movie => movie.MovieCasts).ThenInclude(x => x.Person)
                .Include(movie => movie.MovieCrews).ThenInclude(x => x.Person)
                .Where(movie => genredId.Contains(movie.MovieGenres.Select(x => x.GenreId).FirstOrDefault()))
                .Where(movie => movie.Id != movieId)
                .Take(12).OrderByDescending(m => m.ReleaseDate).ToList();
            return movie;
        }
    }
}
