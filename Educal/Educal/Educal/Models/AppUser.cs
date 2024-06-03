using Microsoft.AspNetCore.Identity;

namespace Educal.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
