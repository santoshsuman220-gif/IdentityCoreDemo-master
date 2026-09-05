using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCoreDemo.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please Enter User Name")]
        public string Name { get; set; }
        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Please Select DOB")]
        public DateOnly DOB {  get; set; }
        [EmailAddress(ErrorMessage ="Please Enter Valid Email Id")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Please Enter Valid Phone Nuber")]
        public string Phone { get; set;  }
        [Required(ErrorMessage = "Pleae Enter Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password Does Not Match")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {  get; set; }
       
    }
}
