using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Movie247.Models;

namespace Movie247.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the Movie247User class
    public class Movie247User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Firstname { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Image { get; set; }

        // vituarl with FavoriteMovies
        public virtual ICollection<MovieFavourite> MovieFavourites { get; set; }
        public virtual ICollection<MovieReview> MovieReviews { get; set; }
        public virtual ICollection<MovieComment> MovieComments { get; set; }

    }

}
