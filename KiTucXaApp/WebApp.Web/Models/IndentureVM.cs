using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Models
{
    public class IndentureVM
    {
        [StringLength(127)]
        public string IndentureId { get; set; }

        [Required]
        [StringLength(127)]
        public string Code { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool IsCanceled { get; set; }

        public DateTime? CanceledDate { get; set; }

        [StringLength(127)]
        public string CanceledBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        // Mã sinh viên
        [Required]
        [StringLength(127)]
        public string Id { get; set; }

        public AppUserVM AppUser { get; set; }

        // Mã phòng
        [Required]
        public int RoomId { get; set; }

        public RoomVM Room { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}