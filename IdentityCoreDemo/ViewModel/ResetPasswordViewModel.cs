using System.ComponentModel.DataAnnotations;

namespace IdentityCoreDemo.ViewModel
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage ="Please enter valid email id")]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage ="Please enter new password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="Password does not match")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
