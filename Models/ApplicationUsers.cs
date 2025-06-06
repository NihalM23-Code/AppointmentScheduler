using Microsoft.AspNetCore.Identity;

namespace AppointmentScheduling.Models
{
    public class ApplicationUsers:IdentityUser
    {
        public string Name { get; set; }
    }
}
