using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie247.Data;

namespace Movie247.Logics
{
    public class CompanyLogic
    {
        private readonly Movie247Context _context;

        public CompanyLogic()
        {
            _context = new Movie247Context();
        }

        public async Task<List<ProductionCompany>> GetAllCompanies()
        {
            return await _context.ProductionCompanies.ToListAsync();
        }
        public ProductionCompany GetCompanyById(int id)
        {
            var company = _context.ProductionCompanies.Include(x => x.MovieCompanies)
                .ThenInclude(x => x.Movie)
                .FirstOrDefault(m => m.Id == id);
            return company;
        }
        public async Task<MultiSelectList> GetAllCompaniesAsMultiSelectList()
        {
            var companies = await GetAllCompanies();
            return new MultiSelectList(companies, "Id", "Name");
        }
    }
}
