using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models.AppUser
{
    public class AppUserVM
    {
        [StringLength(127)]
        public string Id { set; get; }

        [Required]
        [MinLength(6)]
        [StringLength(127)]
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
        public string Fullname { get; set; }

        public bool Gender { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool IsActived { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public int GroupId { get; set; }

        public AppGroupVM AppGroup { get; set; }
    }
}