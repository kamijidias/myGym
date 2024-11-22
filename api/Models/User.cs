using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User:IdentityUser
    {
        public string? userName { get; set; }
    }
}