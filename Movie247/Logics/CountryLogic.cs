using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;
using Movie247.Data;

namespace Movie247.Logics
{
    public class CountryLogic
    {
        private readonly Movie247Context _context;

        public CountryLogic()
        {
            _context = new Movie247Context();
        }

        public async Task<MultiSelectList> GetAllCountriesAsMultiSelectList()
        {
            var countries = await _context.ProductionCountries.ToListAsync();
            return new MultiSelectList(countries, "Id", "Name");

        }
    }
}
