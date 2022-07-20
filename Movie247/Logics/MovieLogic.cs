using Movie247.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Movie247.Helpers;
using System.Threading.Tasks;
using System;
using Movie247.Data;
using Movie247.Areas.Identity.Data;

namespace Movie247.Logics
{
    public class MovieLogic
    {
        // create  _context to access db
        private readonly Movie247Context _context;

        public MovieLogic()
        {
            _context = new Movie247Context();
        }
        // Get 5 trailer of movie latest
        public async Task<List<Movie>> Get5TrailerOfMovieLatest()
        {

            return await _context.Movies.OrderByDescending(m => m.ReleaseDate).Take(5).ToListAsync();
        }
        public async Task<List<Movie>> Get12TopRatedMovies()
        {
            return await _context.Movies.OrderByDescending(m => m.ImdbAverage).Take(10).ToListAsync();
        }
        // get 12 movie favorite of user most
        public async Task<List<Movie>> Get12MovieFavoriteOfUserMost()
        {
            return await _context.Movies.Include(m => m.MovieFavourites).Where(x => x.MovieFavourites.Count > 0).OrderByDescending(m => m.MovieFavourites.Count).Take(12).ToListAsync();
        }
        // get 12 movie most viewed of user most
        public async Task<List<Movie>> Get12MovieMostViewedOfUserMost()
        {
            return await _context.Movies.OrderByDescending(m => m.Views).Take(12).ToListAsync();

        }
        // Get 12 movie with highest rate
        public async Task<List<Movie>> Get12MovieNewest()
        {
            List<Movie> movies = new();
            return await _context.Movies
               .OrderByDescending(m => m.CreateAt)
               .Take(12)
               .Include(x => x.MovieGenres)
               .ThenInclude(x => x.Genre).ToListAsync();

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
        public async Task<List<Movie>> GetMovieByMultipleCondition(FilterModel filter)
        {
            var movies = from m in _context.Movies
                         where m.MovieStatus == true
                         select m;
            if (filter.Name != null)
            {
                movies = movies.Where(m => m.Title.ToLower().Contains(filter.Name.Trim().ToLower()));
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
                return await movies.ToListAsync();
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
            return await movies.Skip(from - 1).Take(filter.PageSize).ToListAsync();
        }

        public Movie GetMovieById(int id)
        {
            Movie movie = new Movie();
            movie = _context.Movies
                .Include(movie => movie.MovieCrews).ThenInclude(x => x.Person)
                .Include(movie => movie.MovieGenres).ThenInclude(x => x.Genre)
                .Include(movie => movie.MovieCompanies).ThenInclude(x => x.Company)
                .Include(movie => movie.MovieCountries).ThenInclude(x => x.Country)
                .Include(movie => movie.MovieCasts).ThenInclude(x => x.Person)
                .Include(movie => movie.MovieSources)
                .FirstOrDefault(movie => movie.Id == id && movie.MovieStatus == true);
            return movie;
        }
        public Movie GetMovieByIdAdmin(int id)
        {
            Movie movie = new Movie();
            movie = _context.Movies
                .Include(movie => movie.MovieCrews)
                .Include(movie => movie.MovieGenres)
                .Include(movie => movie.MovieCompanies)
                .Include(movie => movie.MovieCountries)
                .Include(movie => movie.MovieCasts)
                .Include(movie => movie.MovieSources)
                .FirstOrDefault(movie => movie.Id == id && movie.MovieStatus == true);
            return movie;
        }
        public async Task<List<Movie>> GetAllMovie()
        {
            return await _context.Movies.OrderByDescending(c => c.UpdateAt ?? c.CreateAt).ToListAsync();
        }
        public async Task<int> GetTotalReviews(int id)
        {
            int total = await _context.MovieReviews.Where(m => m.MovieId == id).CountAsync();
            return total;
        }
        public async Task<List<MovieComment>> GetCommentMovieId(int id)
        {
            var qr = _context.MovieComments
                     .Include(c => c.User)
                     .Include(c => c.ParentMovieComment)
                     .Include(c => c.MovieCommentChildren)
                     .Where(c => c.MovieId == id);

            var MovieComments = (await qr.ToListAsync())
                             .Where(c => c.ParentMovieComment == null).ToList();
            return MovieComments;
        }
        public async Task<int> GetTotalCommentMovieId(int id)
        {
            return await _context.MovieComments.Where(c => c.MovieId == id).CountAsync();
        }
        public List<MovieReview> GetReviewByMovieId(int id, int offset, int count, string SortBy, string OrderBy)
        {
            var reviews = _context.MovieReviews
            .Include(x => x.User)
            .Select(x => new MovieReview
            {
                Id = x.Id,
                MovieId = x.MovieId,
                UserId = x.UserId,
                User = new Movie247User
                {
                    Firstname = x.User.Firstname,
                    LastName = x.User.LastName,
                    Image = x.User.Image
                },
                Comment = x.Comment,
                CreateAt = x.CreateAt,
                Rating = x.Rating
            })
            .Where(x => x.MovieId == id);
            if (SortBy == "date")
            {
                if (OrderBy == "desc")
                {
                    reviews = reviews.OrderByDescending(x => x.CreateAt);
                }
                else
                {
                    reviews = reviews.OrderBy(x => x.CreateAt);
                }
            }
            else if (SortBy == "rating")
            {
                if (OrderBy == "desc")
                {
                    reviews = reviews.OrderByDescending(x => x.Rating);
                }
                else
                {
                    reviews = reviews.OrderBy(x => x.Rating);
                }
            }
            else
            {
                reviews = reviews.OrderBy(x => x.CreateAt);
            }
            return reviews.Skip(offset - 1).Take(count).ToList();
        }
        public Movie GetMovieFirstById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            return movie;
        }

        public Movie GetSourceMovieById(int id)
        {
            Movie movie = new Movie();
            movie = _context.Movies
                .Include(movie => movie.MovieGenres).ThenInclude(x => x.Genre)
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
        public int UpdateView(Movie movie)
        {
            movie.Views++;
            _context.Update(movie);
            return _context.SaveChanges();
        }
        public async Task<MovieReview> GetReviewByUserId(int movieId, string userId)
        {
            return await _context.MovieReviews.FirstOrDefaultAsync(x => x.MovieId == movieId && x.UserId == userId);
        }
    }
}
