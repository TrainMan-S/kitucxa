using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class BillWaterVM
    {
        [StringLength(127)]
        public string BillWaterId { get; set; }

        [Range(1, 12)]
        public int OfMonth { get; set; }

        [Range(2018, int.MaxValue)]
        public int OfYear { get; set; }

        [Range(0, double.MaxValue)]
        public double IndexFirst { get; set; }

        [Range(0, double.MaxValue)]
        public double IndexLast { get; set; }

        public decimal Amount { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }

        public string PaidBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

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