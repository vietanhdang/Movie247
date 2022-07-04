using Movie247.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie247.Logics
{
    public class GenreLogic
    {
        private readonly MOVIEPROJECTContext _context;

        public GenreLogic(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        // get all genres asynchronously
        public List<Genre> GetAllGenres()
        {
            var genres = _context.Genres.Include(x => x.MovieGenres).ToList();
            return genres;
        }
        public MultiSelectList GetAllGenresAsMultiSelectList()
        {
            var genres = GetAllGenres();
            var multiSelectList = new MultiSelectList(genres, "Id", "Name");
            return multiSelectList;
        }

    }
}
