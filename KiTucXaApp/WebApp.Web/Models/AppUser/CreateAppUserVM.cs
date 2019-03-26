using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models.AppUser
{
    public class CreateAppUserVM
    {
        [StringLength(127)]
        public string Id { set; get; }

        [Required]
        [MinLength(6)]
        [MaxLength(127)]
        public string UserName { set; get; }

        [Required]
        [StringLength(127)]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [StringLength(127)]
        public string PhoneNumber { set; get; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        [StringLength(127)]
        public string Password { set; get; }

        [Required]
        [StringLength(127)]
        [Compare("Password")]
        public string RePassword { set; get; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        [StringLength(127)]
        public string Fullname { get; set; }

        public bool Gender { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool IsActived { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public int GroupId { get; set; }
    }
}