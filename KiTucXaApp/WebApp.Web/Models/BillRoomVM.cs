using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Models
{
    public class BillRoomVM
    {
        [StringLength(127)]
        public string BillRoomId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public decimal Amount { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }

        [StringLength(127)]
        public string PaidBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        public int RoomId { get; set; }

        public virtual RoomVM Room { get; set; }

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