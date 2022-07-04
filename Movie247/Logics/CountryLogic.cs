using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;

namespace Movie247.Logics
{
    public class CountryLogic
    {
        private readonly MOVIEPROJECTContext _context;

        public CountryLogic(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        public MultiSelectList GetAllCountriesAsMultiSelectList()
        {
            var countries = _context.ProductionCountries;
            var multiSelectList = new MultiSelectList(countries, "Id", "Name");
            return multiSelectList;
        }
    }
}
