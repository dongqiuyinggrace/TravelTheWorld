using Microsoft.AspNetCore.Identity;

namespace TheWorld.Models
{
    public class WorldUser : IdentityUser
    {
        public string Name { get; set; }
    }
}