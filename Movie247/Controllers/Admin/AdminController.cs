using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movie247.Areas.Identity.Data;
using Movie247.Data;
using Movie247.Helpers;
using Movie247.Logics;
using Movie247.Models;
using X.PagedList;

namespace Movie247.Controllers.Admin
{

    public class Movie247UserWithRoles : Movie247User
    {
        public string Roles { get; set; }
    }
    [Authorize(Roles = "Admin,Editor")]
    public class AdminController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<Movie247User> _userManager;
        private readonly Movie247Context _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<Movie247User> userManager, Movie247Context context, IWebHostEnvironment env)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        /*===================================*/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var users = await _userManager.Users.Select(u => new Movie247UserWithRoles
            {
                Id = u.Id,
                UserName = u.UserName,
                Firstname = u.Firstname,
                LastName = u.LastName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                LockoutEnabled = u.LockoutEnabled,
            }).ToListAsync();
            foreach (var user in users)
            {
                user.Roles = string.Join(",", _userManager.GetRolesAsync(user).Result);
            }
            ViewData["roles"] = roles;
            return View("User/Users", users);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Users/EditUserRole")]
        public async Task<JsonResult> EditUserRole(string userId, string[] role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                try
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var rolename in role)
                    {
                        if (roles.Contains(rolename)) continue;
                        await _userManager.AddToRoleAsync(user, rolename);
                    }
                    foreach (var rolename in roles)
                    {
                        if (role.Contains(rolename)) continue;
                        await _userManager.RemoveFromRoleAsync(user, rolename);
                    }
                    return JsonReturn.Success("User role updated successfully");
                }
                catch (Exception ex)
                {
                    return JsonReturn.Error("User role could not be updated " + ex.Message);
                }
            }
            else
            {
                return JsonReturn.Error("User could not be found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Users/LockAndUnlock")]
        public async Task<JsonResult> LockAndUnlock(string userId, bool isLocked)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = isLocked;
                user.LockoutEnd = isLocked ? DateTime.Now.AddYears(100) : DateTime.Now;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return JsonReturn.Success(isLocked ? "User locked successfully" : "User unlocked successfully");
                }
                else
                {
                    return JsonReturn.Error("User could not be locked/unlocked");
                }
            }
            else
            {
                return JsonReturn.Error("User could not be found");
            }
        }
        /*===============DASHBOARD====================*/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            //total users
            var totalUsers = await _userManager.Users.CountAsync();
            //total movies
            var totalMovies = await _context.Movies.CountAsync();
            //total genres
            var totalGenres = await _context.Genres.CountAsync();
            // total reviews
            var totalReviews = await _context.MovieReviews.CountAsync();

            ViewData["totalReviews"] = totalReviews;
            ViewData["totalUsers"] = totalUsers;
            ViewData["totalMovies"] = totalMovies;
            ViewData["totalGenres"] = totalGenres;
            return View("Dashboard/Index");
        }

        /*===============Genres====================*/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Genres()
        {
            var genres = await _context.Genres.ToListAsync();
            return View("Genre/Index", genres);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Genres/Edit")]
        public async Task<JsonResult> EditGenre(int id, string name, string description)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                genre.Name = name;
                genre.Description = description;
                genre.UpdateAt = DateTime.Now;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return JsonReturn.Success("Genre updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Genre could not be updated");
                }
            }
            else
            {
                return JsonReturn.Error("Genre could not be found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Genres/Delete")]
        public async Task<JsonResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                if (_context.MovieGenres.FirstOrDefault(x => x.GenreId == id) != null)
                {
                    return JsonReturn.Error("Genre could not be deleted because it is associated with a movie");
                }
                _context.Genres.Remove(genre);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return JsonReturn.Success("Genre deleted successfully");
                }
                else
                {
                    return JsonReturn.Error("Genre could not be deleted");
                }
            }
            else
            {
                return JsonReturn.Error("Genre could not be found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Genres/Add")]
        public async Task<JsonResult> AddGenre(string name, string description)
        {
            var genre = new Genre
            {
                Name = name,
                Description = description,
                CreateAt = DateTime.Now,
            };
            _context.Genres.Add(genre);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return JsonReturn.Success("Genre added successfully");
            }
            else
            {
                return JsonReturn.Error("Genre could not be added");
            }
        }
        /*===================================*/
        /*===============Countries====================*/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Countries()
        {
            var countries = await _context.ProductionCountries.ToListAsync();
            return View("Country/Index", countries);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Countries/Edit")]
        public async Task<JsonResult> EditCountry(String id, string name)
        {
            var country = await _context.ProductionCountries.FindAsync(id);
            if (country != null)
            {
                country.Id = id;
                country.Name = name;
                country.UpdateAt = DateTime.Now;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return JsonReturn.Success("Country updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Country could not be updated");
                }
            }
            else
            {
                return JsonReturn.Error("Country could not be found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Countries/Delete")]
        public async Task<JsonResult> DeleteCountry(string id)
        {
            var country = await _context.ProductionCountries.FindAsync(id);
            if (country != null)
            {
                if (_context.MovieCountries.FirstOrDefault(x => x.CountryId == id) != null)
                {
                    return JsonReturn.Error("Country could not be deleted because it is associated with a movie");
                }
                _context.ProductionCountries.Remove(country);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return JsonReturn.Success("Country deleted successfully");
                }
                else
                {
                    return JsonReturn.Error("Country could not be deleted");
                }
            }
            else
            {
                return JsonReturn.Error("Country could not be found");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Countries/Add")]
        public async Task<JsonResult> AddCountry(string id, string name)
        {
            var country = new ProductionCountry
            {
                Id = id,
                Name = name,
                CreateAt = DateTime.Now,
            };
            _context.ProductionCountries.Add(country);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return JsonReturn.Success("Country added successfully");
            }
            else
            {
                return JsonReturn.Error("Country could not be added");
            }
        }
        /*==================================*/
        [HttpGet]
        public async Task<IActionResult> Movies()
        {
            var movies = await new MovieLogic().GetAllMovie();
            return View("Movie/Movies", movies);
        }
        [HttpGet]
        [Route("/Admin/Movies/Add")]
        public async Task<IActionResult> AddMovie()
        {
            var genres = await _context.Genres.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToListAsync();
            var companies = await _context.ProductionCompanies.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();
            var countries = await _context.ProductionCountries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();
            var persons = await _context.People.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToListAsync();
            ViewData["genres"] = genres;
            ViewData["companies"] = companies;
            ViewData["countries"] = countries;
            ViewData["persons"] = persons;
            return View("Movie/Add");
        }
        [HttpGet]
        [Route("/Admin/Movies/Edit/{id}")]
        public async Task<IActionResult> EditMovie()
        {
            Movie movie = new MovieLogic().GetMovieByIdAdmin(Convert.ToInt32(RouteData.Values["id"]));
            var genres = await _context.Genres.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToListAsync();
            var companies = await _context.ProductionCompanies.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();
            var persons = await _context.People.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToListAsync();
            var countries = await _context.ProductionCountries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();

            ViewData["genres"] = genres;
            ViewData["companies"] = companies;
            ViewData["countries"] = countries;
            ViewData["persons"] = persons;
            return View("Movie/Edit", movie);
        }
        [HttpPost]
        [Route("/Admin/Movies/HideAndShow")]
        public async Task<JsonResult> HideAndShow(int movieId, bool status)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie != null)
            {
                movie.MovieStatus = status;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return JsonReturn.Success(!status ? "Movie hidden successfully" : "Movie unhidden successfully");
                }
                else
                {
                    return JsonReturn.Error("Movie could not be hidden/unhidden");
                }
            }
            else
            {
                return JsonReturn.Error("Movie could not be found");
            }
        }
        [HttpPost]
        [Route("/Admin/Movies/EditMovie")]
        public async Task<JsonResult> EditMovieAdvanced(Movie movie, IFormFile Poster,
        IFormFile Backdrop, string[] Countries, int[] Companies, int[] Genres,
        int[] Actors, int[] Directors, string[] Sources)
        {
            try
            {
                movie.UpdateAt = DateTime.Now;
                if (Poster != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Poster.FileName);
                    var filePath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "poster", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Poster.CopyToAsync(stream);
                    }
                    if (System.IO.File.Exists(movie.PosterPath))
                    {
                        System.IO.File.Delete(movie.PosterPath);
                    }
                    movie.PosterPath = fileName;
                }
                if (Backdrop != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Backdrop.FileName);
                    var filePath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "backdrop", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Backdrop.CopyToAsync(stream);
                    }
                    if (System.IO.File.Exists(movie.BackdropPath))
                    {
                        System.IO.File.Delete(movie.BackdropPath);
                    }
                    movie.BackdropPath = fileName;
                }
                _context.Movies.Update(movie);
                if (await _context.SaveChangesAsync() > 0)
                {
                    // update all movie genres, production companies, production countries, actors, directors, sources
                    var movieId = movie.Id;
                    _context.MovieCasts.RemoveRange(_context.MovieCasts.Where(c => c.MovieId == movieId));
                    _context.MovieCrews.RemoveRange(_context.MovieCrews.Where(c => c.MovieId == movieId));
                    _context.MovieCountries.RemoveRange(_context.MovieCountries.Where(c => c.MovieId == movieId));
                    _context.MovieCompanies.RemoveRange(_context.MovieCompanies.Where(c => c.MovieId == movieId));
                    _context.MovieGenres.RemoveRange(_context.MovieGenres.Where(c => c.MovieId == movieId));
                    _context.MovieSources.RemoveRange(_context.MovieSources.Where(c => c.MovieId == movieId));

                    _context.MovieCasts.AddRange(Actors.Select(a => new MovieCast { MovieId = movieId, PersonId = a }));
                    _context.MovieCrews.AddRange(Directors.Select(d => new MovieCrew { MovieId = movieId, PersonId = d }));
                    _context.MovieCountries.AddRange(Countries.Select(c => new MovieCountry { MovieId = movieId, CountryId = c }));
                    _context.MovieCompanies.AddRange(Companies.Select(c => new MovieCompany { MovieId = movieId, CompanyId = c }));
                    _context.MovieGenres.AddRange(Genres.Select(g => new MovieGenre { MovieId = movieId, GenreId = g }));
                    if (Sources != null)
                    {
                        foreach (var source in Sources)
                        {
                            string Description = source.Split("@@")[0].Trim();
                            string LinkSource = source.Split("@@")[1].Trim();
                            _context.MovieSources.Add(new MovieSource { MovieId = movie.Id, Description = Description, LinkSource = LinkSource });
                        }
                    }
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return JsonReturn.Success("Movie updated successfully");
                    }
                    else
                    {
                        return JsonReturn.Error("Movie could not be updated");
                    }
                }
                else
                {
                    return JsonReturn.Error("Movie could not be updated");
                }
            }
            catch (Exception ex)
            {
                return JsonReturn.Error("Movie could not be updated" + ex.Message);
            }
        }
        [HttpPost]
        [Route("/Admin/Movies/Edit")]
        /**
        Edit quick movie
        */
        public async Task<JsonResult> Edit(Movie movie)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var old_movie = await _context.Movies.FindAsync(movie.Id);
                    if (old_movie != null)
                    {
                        old_movie.Title = movie.Title;
                        old_movie.ReleaseDate = movie.ReleaseDate;
                        old_movie.ImdbAverage = movie.ImdbAverage;
                        old_movie.Duration = movie.Duration;
                        old_movie.ReleaseDate = movie.ReleaseDate;
                        old_movie.UpdateAt = DateTime.Now;
                    }
                    _context.Movies.Update(old_movie);
                    await _context.SaveChangesAsync();
                    return JsonReturn.Success("Movie updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Movie could not be updated");
                }
            }
            catch (Exception ex)
            {
                return JsonReturn.Error("Movie could not be updated " + ex.Message);
            }
        }
        [HttpPost]
        [Route("/Admin/Movies/Add")]
        public async Task<JsonResult> Add(Movie movie, IFormFile Poster,
        IFormFile Backdrop, string[] Countries, int[] Companies, int[] Genres,
        int[] Actors, int[] Directors, string[] Sources)
        {
            if (Poster == null)
            {
                return JsonReturn.Error("Poster is required");
            }
            if (Backdrop == null)
            {
                return JsonReturn.Error("Backdrop is required");
            }
            try
            {
                movie.CreateAt = DateTime.Now;
                movie.Views = 0;
                _context.Movies.Add(movie);
                if (await _context.SaveChangesAsync() > 0)
                {
                    _context.MovieCrews.AddRange(Directors.Select(a => new MovieCrew { MovieId = movie.Id, PersonId = a }));
                    _context.MovieCasts.AddRange(Actors.Select(d => new MovieCast { MovieId = movie.Id, PersonId = d }));
                    _context.MovieCountries.AddRange(Countries.Select(c => new MovieCountry { MovieId = movie.Id, CountryId = c }));
                    _context.MovieCompanies.AddRange(Companies.Select(c => new MovieCompany { MovieId = movie.Id, CompanyId = c }));
                    _context.MovieGenres.AddRange(Genres.Select(g => new MovieGenre { MovieId = movie.Id, GenreId = g }));
                    if (Sources != null)
                    {
                        foreach (var source in Sources)
                        {
                            string Description = source.Split("@@")[0].Trim();
                            string LinkSource = source.Split("@@")[1].Trim();
                            _context.MovieSources.Add(new MovieSource { MovieId = movie.Id, Description = Description, LinkSource = LinkSource });
                        }
                    }
                    if (Poster != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Poster.FileName);
                        var filePath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "poster", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Poster.CopyToAsync(stream);
                        }
                        movie.PosterPath = fileName;
                    }
                    if (Backdrop != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Backdrop.FileName);
                        var filePath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "backdrop", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Backdrop.CopyToAsync(stream);
                        }
                        movie.BackdropPath = fileName;
                    }
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return JsonReturn.Success("Movie added successfully");
                    }
                    else
                    {
                        return JsonReturn.Error("Movie could not be added");
                    }
                }
                else
                {
                    return JsonReturn.Error("Movie could not be added");
                }
            }
            catch (Exception ex)
            {
                return JsonReturn.Error("Movie could not be added " + ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/Movies/Delete")]
        public async Task<JsonResult> Delete(int movieId)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(movieId);
                if (movie != null)
                {
                    try
                    {
                        _context.MovieCasts.RemoveRange(_context.MovieCasts.Where(c => c.MovieId == movieId));
                        _context.MovieCrews.RemoveRange(_context.MovieCrews.Where(c => c.MovieId == movieId));
                        _context.MovieCountries.RemoveRange(_context.MovieCountries.Where(c => c.MovieId == movieId));
                        _context.MovieCompanies.RemoveRange(_context.MovieCompanies.Where(c => c.MovieId == movieId));
                        _context.MovieGenres.RemoveRange(_context.MovieGenres.Where(c => c.MovieId == movieId));
                        _context.MovieFavourites.RemoveRange(_context.MovieFavourites.Where(c => c.MovieId == movieId));
                        _context.MovieComments.RemoveRange(_context.MovieComments.Where(c => c.MovieId == movieId));
                        _context.MovieSources.RemoveRange(_context.MovieSources.Where(c => c.MovieId == movieId));
                    }
                    catch (Exception) { }
                    if (movie.PosterPath != null)
                    {
                        string posterPath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "poster", movie.PosterPath);
                        if (System.IO.File.Exists(posterPath))
                        {
                            System.IO.File.Delete(posterPath);
                        }
                    }
                    if (movie.BackdropPath != null)
                    {
                        string backdropPath = Path.Combine(_env.WebRootPath, "web", "images", "movie", "backdrop", movie.BackdropPath);
                        if (System.IO.File.Exists(backdropPath))
                        {
                            System.IO.File.Delete(backdropPath);
                        }
                    }
                    _context.Movies.Remove(movie);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return JsonReturn.Success("Movie deleted successfully");
                    }
                    else
                    {
                        return JsonReturn.Error("Movie could not be deleted");
                    }
                }
                else
                {
                    return JsonReturn.Error("Movie could not be found");
                }
            }
            catch (Exception ex)
            {
                return JsonReturn.Error("Movie could not be deleted " + ex.Message);
            }
        }
        /*===================================*/
        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            var companies = await _context.ProductionCompanies.OrderByDescending(c => c.UpdateAt ?? c.CreateAt).ToListAsync();
            return View("Company/Companies", companies);
        }
        [HttpPost]
        [Route("/Admin/Companies/Edit")]
        public async Task<JsonResult> EditCompany(ProductionCompany productionCompany, IFormFile Image)
        {
            ProductionCompany old_company = await _context.ProductionCompanies.FindAsync(productionCompany.Id);
            if (Image != null)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var imagePath = Path.Combine(_env.WebRootPath, "web", "images", "company", imageName);
                if (!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                productionCompany.FilePath = imageName;
            }
            if (old_company != null)
            {
                if (Image != null)
                {
                    try
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "web", "images", "company", old_company.FilePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    old_company.FilePath = productionCompany.FilePath;
                }
                old_company.Name = productionCompany.Name;
                old_company.Description = productionCompany.Description;
                old_company.UpdateAt = DateTime.Now;
                _context.ProductionCompanies.Update(old_company);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return JsonReturn.Success("Company updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Company could not be updated");
                }

            }
            else
            {
                _context.ProductionCompanies.Add(productionCompany);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return JsonReturn.Success("Company added successfully");
                }
                else
                {
                    return JsonReturn.Error("Company could not be added");
                }
            }
        }
        [HttpPost]
        [Route("/Admin/Companies/Delete")]
        public async Task<JsonResult> DeleteCompany(int companyId)
        {
            var company = await _context.ProductionCompanies.FindAsync(companyId);
            if (company != null)
            {
                var productionCompany = await _context.MovieCompanies.Where(x => x.Id == companyId).ToListAsync();
                _context.MovieCompanies.RemoveRange(productionCompany);
                _context.ProductionCompanies.Remove(company);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return JsonReturn.Success("Company deleted successfully");
                }
                else
                {
                    return JsonReturn.Error("Company could not be deleted");
                }
            }
            else
            {
                return JsonReturn.Error("Company could not be found");
            }
        }
        /*===================================*/
        [HttpGet]
        public async Task<IActionResult> Celebrities(int? page, string search, int show)
        {
            IPagedList<Person> celebrities = null;
            if (search != null)
            {
                celebrities = await _context.People.OrderByDescending(c => c.UpdateAt ?? c.CreateAt).Where(x => x.Name.Contains(search)).ToPagedListAsync(page ?? 1, show == 0 ? 10 : show);
            }
            else
            {
                celebrities = await _context.People.OrderByDescending(c => c.UpdateAt ?? c.CreateAt).ToPagedListAsync(page ?? 1, show == 0 ? 10 : show);

            }
            ViewData["search"] = search;
            ViewData["show"] = show == 0 ? 10 : show;
            return View("Celebrity/Celebrities", celebrities);
        }
        [HttpPost]
        [Route("/Admin/Celebrities/Delete")]
        public async Task<JsonResult> DeleteCelebrity(int id)
        {
            var celebrity = await _context.People.FindAsync(id);
            if (celebrity != null)
            {
                var movie_cast = await _context.MovieCasts.Where(mc => mc.PersonId == id).ToListAsync();
                var movie_crew = await _context.MovieCrews.Where(mc => mc.PersonId == id).ToListAsync();
                _context.MovieCasts.RemoveRange(movie_cast);
                _context.MovieCrews.RemoveRange(movie_crew);
                _context.People.Remove(celebrity);
                await _context.SaveChangesAsync();
                return JsonReturn.Success("Celebrity deleted successfully");
            }
            else
            {
                return JsonReturn.Error("Celebrity could not be found");
            }
        }
        [HttpPost]
        [Route("/Admin/Celebrities/Edit")]
        public async Task<JsonResult> EditCelebrity(Person celebrity, IFormFile Image)
        {
            Person old_celebrity = await _context.People.FindAsync(celebrity.Id);
            if (Image != null)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var imagePath = Path.Combine(_env.WebRootPath, "web", "images", "person", imageName);
                if (!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                celebrity.ProfilePath = imageName;
            }
            if (old_celebrity != null)
            {
                if (Image != null)
                {
                    try
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "web", "images", "person", old_celebrity.ProfilePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    old_celebrity.ProfilePath = celebrity.ProfilePath;
                }
                old_celebrity.Name = celebrity.Name;
                old_celebrity.Birthday = celebrity.Birthday;
                old_celebrity.Description = celebrity.Description;
                old_celebrity.Popularity = celebrity.Popularity;
                old_celebrity.UpdateAt = DateTime.Now;
                _context.People.Update(old_celebrity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return JsonReturn.Success("Celebrity updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Celebrity could not be updated");
                }

            }
            else
            {
                _context.People.Add(celebrity);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return JsonReturn.Success("Celebrity added successfully");
                }
                else
                {
                    return JsonReturn.Error("Celebrity could not be added");
                }
            }
        }
        /*===============API CALL ====================*/
        [HttpGet]
        [Route("/Admin/API/GetTopViews")]
        public async Task<JsonResult> GetTopViews()
        {
            var topViews = await _context.Movies.OrderByDescending(x => x.Views).Take(10).ToListAsync();
            return Json(topViews);
        }
    }


}
