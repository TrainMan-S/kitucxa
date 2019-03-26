using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomType RoomType { get; set; }

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
