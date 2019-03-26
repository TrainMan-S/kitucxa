using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models.AppUser
{
    public class ChangePasswordVM
    {
        [Required]
        [StringLength(127)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(127)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(127)]
        [Compare("NewPassword")]
        public string ReNewPassword { get; set; }
    }
}