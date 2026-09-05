using Microsoft.AspNetCore.Identity;

namespace IdentityCoreDemo.Models
{
    public class Users:IdentityUser
    {
        public string Name { get; set; }
        public DateOnly DOB { get; set; }
    }
}
