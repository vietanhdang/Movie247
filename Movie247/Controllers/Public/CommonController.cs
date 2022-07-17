using Microsoft.AspNetCore.Mvc;
using Movie247.Data;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie247.Controllers
{
    public static class CommonController
    {

        public static List<Genre> GetCategory()
        {
            using (var context = new Movie247Context())
            {
                return context.Genres.ToList();
            }
        }
        public static List<ProductionCountry> GetProductionCountry()
        {
            using (var context = new Movie247Context())
            {
                return context.ProductionCountries.ToList();
            }
        }
        public static List<ProductionCompany> GetCompany()
        {
            using (var context = new Movie247Context())
            {
                return context.ProductionCompanies.ToList();
            }
        }

    }

}
