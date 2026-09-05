using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCoreDemo.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
    }
}
