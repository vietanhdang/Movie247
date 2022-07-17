
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie247.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Movie247.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Movie247.Logics;
using Movie247.Models;
using System.Linq;
using Movie247.Helpers;

namespace Movie247.Controllers.User
{
    public class UserController : Controller
    {
        private readonly SignInManager<Movie247User> _signInManager;

        private readonly Movie247Context _context;
        private readonly UserManager<Movie247User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public UserController(SignInManager<Movie247User> signInManager, Movie247Context context, UserManager<Movie247User> userManager, ILogger<UserController> logger, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _env = env;
        }

        [BindProperty]
        public Movie247User Movie247User { get; set; }

        /*DO GET ACTIONS*/
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["Title"] = $"Profile of {user.UserName}";
            return View("Profile", user);
        }
        public IActionResult Favourite()
        {
            var movies = new UserLogic().GetFavouriteMovies(_userManager.GetUserId(User));
            ViewData["Title"] = "Favourite Movies";
            return View("Favourite", movies);
        }
        public IActionResult Rated()
        {
            var reviews = new UserLogic().GetReviews(_userManager.GetUserId(User));
            ViewData["Title"] = "Rated Movies";
            return View("Rated", reviews);
        }
        public async Task<IActionResult> ReConfirmEmail()
        {
            ViewData["Title"] = "ReConfirm Email";
            var user = await _userManager.GetUserAsync(User);
            if (user != null && !user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            return RedirectToAction("Index", "Redirect", new { message = "Please check your email and confirm your account" });
        }

        /*DO POST ACTIONS*/
        [HttpPost]
        public async Task<JsonResult> UpdatePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return JsonReturn.Error("Passwords do not match");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return JsonReturn.Error("User not found");
            }
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                return JsonReturn.Success("Password updated successfully");
            }
            else
            {
                return JsonReturn.Error("Password update failed");
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateProfile(string firstname, string lastname, string username)
        {
            var user = await _userManager.GetUserAsync(User);
            // check if the username is already taken
            if (await _userManager.FindByNameAsync(username) != null && username != user.UserName)
            {
                return JsonReturn.Error("Username is already taken");
            }
            else
            {
                user.UserName = username;
            }
            if (user == null)
            {
                return JsonReturn.Error("User not found");
            }
            user.Firstname = firstname;
            user.LastName = lastname;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // update user in session
                await _signInManager.RefreshSignInAsync(user);
                return JsonReturn.Success("Profile updated successfully");
            }
            else
            {
                return JsonReturn.Error("Profile update failed");
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateImage(IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return JsonReturn.Error("User not found");
            }
            if (image != null)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(_env.WebRootPath, "web", "images", "users", user.Id, "profile", imageName);
                if (!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                // delete the old image
                try
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "web", "images", "users", user.Id, "profile", user.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                catch (Exception ex)
                {

                }
                // set user image
                user.Image = imageName;
                await _signInManager.RefreshSignInAsync(user);
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return JsonReturn.Success("Profile updated successfully");
            }
            else
            {
                return JsonReturn.Error("Profile update failed");
            }
        }
        [HttpPost]
        public async Task<JsonResult> ReviewMovie(int MovieId, string Comment, int Rate, string Action)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User not found" });
            }
            var comment = _context.MovieReviews.FirstOrDefault(x => x.UserId == user.Id && x.MovieId == MovieId);
            if (Action == "Submit")
            {
                if (comment == null)
                {
                    comment = new MovieReview()
                    {
                        UserId = _userManager.GetUserId(User),
                        MovieId = MovieId,
                        Comment = Comment,
                        Rating = (short)Rate,
                        CreateAt = DateTime.Now

                    };
                    _context.MovieReviews.Add(comment);

                    if (_context.SaveChanges() > 0)
                    {
                        return JsonReturn.Success("Review submitted successfully");
                    }
                    else
                    {
                        return JsonReturn.Error("Review submission failed");
                    }
                }
                else
                {
                    return JsonReturn.Error("You have already reviewed this movie");
                }
            }
            if (Action == "Edit")
            {
                if (comment != null)
                {
                    comment.Comment = Comment;
                    comment.Rating = (short)Rate;
                    comment.UpdateAt = DateTime.Now;
                    // save the changes
                    if (_context.SaveChanges() > 0)
                    {

                        return new JsonResult(new { success = true, message = "Review updated" });
                    }
                    else
                    {
                        _logger.LogError("Review not updated");
                        return new JsonResult(new { success = false, message = "Review not updated" });
                    }
                }
                else
                {
                    _logger.LogError("Review not found");
                    return new JsonResult(new { success = false, message = "Review not found" });
                }
            }
            if (Action == "Delete")
            {
                if (comment != null)
                {
                    _context.MovieReviews.Remove(comment);

                    if (_context.SaveChanges() > 0)
                    {

                        return new JsonResult(new { success = true, message = "Review deleted" });
                    }
                    else
                    {
                        _logger.LogError("Review not deleted");
                        return new JsonResult(new { success = false, message = "Review not deleted" });
                    }
                }
                else
                {
                    _logger.LogError("Review not found");
                    return new JsonResult(new { success = false, message = "Review not found" });
                }
            }
            return new JsonResult(new { success = false, message = "Action not found" });
        }
        [HttpPost]
        public JsonResult AddToFavourite(int movieId)
        {
            var userId = _userManager.GetUserId(User);
            var movie = _context.Movies.Find(movieId);
            if (movie != null)
            {
                var favourite = _context.MovieFavourites.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefault();
                if (favourite != null)
                {
                    return JsonReturn.Error("Movie already in favourite");
                }
                MovieFavourite favouriteMovie = new MovieFavourite
                {
                    Movie = movie,
                    UserId = userId
                };
                _context.MovieFavourites.Add(favouriteMovie);
                if (_context.SaveChanges() > 0)
                {
                    return JsonReturn.Success("Movie added to favourite");
                }
                else
                {
                    return JsonReturn.Error("Something went wrong");
                }
            }
            else
            {
                return JsonReturn.Error("Movie not found");
            }
        }
        [HttpPost]
        public JsonResult RemoveFromFavourite(int movieId)
        {
            var userId = _userManager.GetUserId(User);
            var favourite = _context.MovieFavourites.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefault();
            if (favourite != null)
            {
                _context.MovieFavourites.Remove(favourite);
                if (_context.SaveChanges() > 0)
                {
                    return JsonReturn.Success("Movie removed from favourite");
                }
                else
                {
                    return JsonReturn.Error("Something went wrong");
                }
            }
            else
            {
                return JsonReturn.Error("Movie not found");
            }
        }
        public JsonResult ReplyComment(int commentId, int movieId, string reply)
        {
            var userId = _userManager.GetUserId(User);
            var comment = _context.MovieComments.FirstOrDefault(x => x.Id == commentId && x.MovieId == movieId);
            if (comment != null)
            {
                MovieComment movieComment = new MovieComment
                {
                    MovieId = movieId,
                    UserId = userId,
                    Comment = reply,
                    ParentMovieId = commentId,
                    CreateAt = DateTime.Now
                };
                _context.MovieComments.Add(movieComment);
                if (_context.SaveChanges() > 0)
                {
                    return JsonReturn.Success("Reply submitted successfully");
                }
                else
                {
                    return JsonReturn.Error("Reply submission failed");
                }
            }
            else
            {
                return JsonReturn.Error("Comment not found");
            }
        }
        public JsonResult DeleteComment(int commentId)
        {
            var userId = _userManager.GetUserId(User);
            var comment = _context.MovieComments.FirstOrDefault(x => x.Id == commentId && x.UserId == userId);
            if (comment != null)
            {
                // get all child comments
                var childComments = _context.MovieComments.Where(x => x.ParentMovieId == commentId);
                // get all comments reply child comments
                var childReplyComments = _context.MovieComments.Where(x => childComments.Any(y => y.Id == x.ParentMovieId));
                // delete all child comments
                _context.MovieComments.RemoveRange(childReplyComments);
                // delete all child comments
                _context.MovieComments.RemoveRange(childComments);
                // delete comment
                _context.MovieComments.Remove(comment);
                if (_context.SaveChanges() > 0)
                {
                    return JsonReturn.Success("Comment deleted");
                }
                else
                {
                    return JsonReturn.Error("Something went wrong");
                }
            }
            else
            {
                return JsonReturn.Error("Comment not found");
            }
        }
        public JsonResult EditComment(int commentId, string comment)
        {
            var userId = _userManager.GetUserId(User);
            var commentToEdit = _context.MovieComments.FirstOrDefault(x => x.Id == commentId && x.UserId == userId);
            if (commentToEdit != null)
            {
                commentToEdit.Comment = comment;
                commentToEdit.UpdateAt = DateTime.Now;
                if (_context.SaveChanges() > 0)
                {
                    return JsonReturn.Success("Comment updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Comment update failed");
                }
            }
            else
            {
                return JsonReturn.Error("Comment not found");
            }
        }
    }
}
