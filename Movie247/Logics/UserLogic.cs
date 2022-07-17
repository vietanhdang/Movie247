using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Movie247.Data;
using Movie247.Models;

namespace Movie247.Logics
{
    public class UserLogic
    {
        private readonly Movie247Context _context;

        public UserLogic()
        {
            _context = new Movie247Context();
        }
        public IEnumerable<MovieFavourite> GetFavouriteMovies(string userId)
        {
            var movies = _context.MovieFavourites.Include(x => x.Movie).Where(x => x.UserId == userId);
            return movies;
        }
        public IEnumerable<MovieReview> GetReviews(string userId)
        {
            var reviews = _context.MovieReviews.Include(x => x.Movie).Where(x => x.UserId == userId);
            return reviews;
        }

    }
}
