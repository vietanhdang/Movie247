using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class User
    {
        public User()
        {
            MovieReviews = new HashSet<MovieReview>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Description { get; set; }
        public bool Activated { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<MovieReview> MovieReviews { get; set; }
    }
}
