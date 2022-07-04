using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie247.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie247.Controllers
{
    [NonViewComponent]
    public class CommonController : Controller
    {
        // GET: CommonController
        public static List<Genre> GetAllCategory()
        {
            List<Genre> lists = new List<Genre>();
            using (var _context = new MOVIEPROJECTContext())
            {
                lists = _context.Genres.ToList();
            }
            return lists;
        }
        [NonAction]
        public ActionResult GetCategory()
        {
            List<Genre> lists = new();
            using (var _context = new MOVIEPROJECTContext())
            {
                lists = _context.Genres.ToList();
            }
            return PartialView("_Genre", lists);
        }

    }

}
