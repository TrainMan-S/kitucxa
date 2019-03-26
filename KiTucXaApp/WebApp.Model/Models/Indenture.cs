using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Indentures")]
    public class Indenture
    {
        [Key]
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

        [Required]
        [StringLength(127)]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual AppUser AppUser { get; set; }

        [Required]
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
