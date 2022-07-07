using Microsoft.AspNetCore.Mvc;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie247.Controllers
{
    public class CommonController : Controller
    {
        public readonly MOVIEPROJECTContext _context;

        public CommonController(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        public ActionResult GetCategory()
        {
            List<Genre> lists = _context.Genres.ToList();
            return PartialView("_Genre", lists);
        }

    }

}
