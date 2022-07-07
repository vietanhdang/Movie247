using System.ComponentModel.DataAnnotations;

namespace Movie247.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email or Username is required")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
