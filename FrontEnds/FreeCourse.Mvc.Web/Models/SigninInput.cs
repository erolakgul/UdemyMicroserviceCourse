using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Mvc.Web.Models
{
    public class SigninInput
    {
        [Display(Name = "dn.Email")]
        [Required(ErrorMessage = "dn.Error.Email")]
        public string? Email { get; set; }
        [Display(Name = "dn.Password")]
        [Required(ErrorMessage = "dn.Error.Password")]
        public string? Password { get; set; }
        [Display(Name = "dn.IsRemember")]
        public bool IsRemember { get; set; }
    }
}
