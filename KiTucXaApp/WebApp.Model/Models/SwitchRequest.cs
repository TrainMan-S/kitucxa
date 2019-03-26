using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("SwitchRequests")]
    public class SwitchRequest
    {
        [Key]
        [StringLength(127)]
        public string SwitchRequestId { get; set; }

        [Required]
        public int FromRoomId { get; set; }

        [Required]
        [StringLength(127)]
        public string FromRoomCode { get; set; }

        [StringLength(1023)]
        public string FromRoomDesc { get; set; }

        [Required]
        public int ToRoomId { get; set; }

        [Required]
        [StringLength(127)]
        public string ToRoomCode { get; set; }

        [StringLength(1023)]
        public string ToRoomDesc { get; set; }

        public DateTime SwitchDate { get; set; }

        [Required]
        [StringLength(1023)]
        public string SwitchReason { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        [StringLength(127)]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual AppUser AppUser { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool? Status { get; set; }

        [StringLength(1023)]
        public string HandledNote { get; set; }

        public DateTime? HandledDate { get; set; }

        [StringLength(127)]
        public string HandledBy { get; set; }
    }
}
