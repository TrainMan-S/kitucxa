using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Models
{
    public class DisciplineVM
    {
        [StringLength(127)]
        public string DisciplineId { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime AtDate { get; set; }

        // *********************************
        // *********************************
        // *********************************

        // Mã sinh viên
        [Required]
        [StringLength(127)]
        public string Id { get; set; }

        public AppUserVM AppUser { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}