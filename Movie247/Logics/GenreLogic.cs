using Movie247.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie247.Data;

namespace Movie247.Logics
{
    public class GenreLogic
    {
        private readonly Movie247Context _context;

        public GenreLogic()
        {
            _context = new Movie247Context();
        }

        // get all genres asynchronously
        public async Task<List<Genre>> GetAllGenres()
        {
            return await _context.Genres.Include(x => x.MovieGenres).ToListAsync();

        }
        public async Task<MultiSelectList> GetAllGenresAsMultiSelectList()
        {
            var genres = await GetAllGenres();
            return new MultiSelectList(genres, "Id", "Name");
        }

    }
}
