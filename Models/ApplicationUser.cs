using Microsoft.AspNetCore.Identity;

namespace Sashiel_CLDV6211_Part2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
