using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Furnitures")]
    public class Furniture
    {
        [Key]
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

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        [StringLength(127)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(127)]
        public string UpdatedBy { get; set; }
    }
}
