using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class FurnitureVM
    {
        [StringLength(127)]
        public string FurnitureId { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime RepairDate { get; set; }

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