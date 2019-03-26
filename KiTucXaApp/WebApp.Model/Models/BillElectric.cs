using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("BillElectrics")]
    public class BillElectric
    {
        [Key]
        [StringLength(127)]
        public string BillElectricId { get; set; }

        [Range(1, 12)]
        public int OfMonth { get; set; }

        [Range(2018, int.MaxValue)]
        public int OfYear { get; set; }

        [Range(0, int.MaxValue)]
        public int IndexFirst { get; set; }

        [Range(0, int.MaxValue)]
        public int IndexLast { get; set; }
 
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
