using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Movie247.Models
{
    public class Test
    {
        [Required(ErrorMessage = "Phải có tên")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Chiều dài không chính xác")]
        [Display(Name = "TÊN KHÁCH")] // Label hiện thị
        public string Customername { set; get; }
    }
}
