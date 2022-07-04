using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie247.Logics
{
    public class CompanyLogic
    {
        private readonly MOVIEPROJECTContext _context;

        public CompanyLogic(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        public List<ProductionCompany> GetAllCompanies()
        {
            var companies = _context.ProductionCompanies.ToList();
            return companies;
        }
        public ProductionCompany GetCompanyById(int id)
        {
            var company = _context.ProductionCompanies.Include(x => x.MovieCompanies)
                .ThenInclude(x => x.Movie)
                .FirstOrDefault(m => m.Id == id);
            return company;
        }
        public MultiSelectList GetAllCompaniesAsMultiSelectList()
        {
            var companies = GetAllCompanies();
            var multiSelectList = new MultiSelectList(companies, "Id", "Name");
            return multiSelectList;
        }
    }
}
