using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SlackOverload.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; }
        public ApplicationUser()
        {
            Reputation = 0;
        }

        public static implicit operator ApplicationUser(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
