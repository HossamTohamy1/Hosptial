using System.ComponentModel.DataAnnotations;

namespace Hospitel_Project.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Pass")]
        public string ConfirmPassword { get; set; }
  
    }
}
