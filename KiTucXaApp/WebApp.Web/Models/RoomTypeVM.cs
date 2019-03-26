using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class RoomTypeVM
    {
        public int RoomTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(1023)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }

        public bool IsActived { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}