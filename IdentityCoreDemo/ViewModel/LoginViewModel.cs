using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCoreDemo.ViewModel
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage ="Please enter valid email id")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
