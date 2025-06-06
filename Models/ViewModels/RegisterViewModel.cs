using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduling.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100,ErrorMessage ="The {0} must be atleast {2} character long.",MinimumLength =8)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="The password doesn't match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}
