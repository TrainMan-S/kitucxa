using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class RoomVM
    {
        public int RoomId { get; set; }

        [Required]
        [StringLength(127)]
        public string Code { get; set; }

        [Required]
        [StringLength(1023)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int CapacityMax { get; set; }

        [Range(0, int.MaxValue)]
        public int CapacityCur { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }

        public bool IsActived { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        public int RoomTypeId { get; set; }

        public RoomTypeVM RoomType { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}