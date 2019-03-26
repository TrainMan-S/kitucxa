using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Models
{
    public class SwitchRequestVM
    {
        [StringLength(127)]
        public string SwitchRequestId { get; set; }

        [Required]
        public int FromRoomId { get; set; }

        [StringLength(127)]
        public string FromRoomCode { get; set; }

        [StringLength(1023)]
        public string FromRoomDesc { get; set; }

        [Required]
        public int ToRoomId { get; set; }

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

        [StringLength(127)]
        public string Id { get; set; }

        public AppUserVM AppUser { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool? Status { get; set; }

        public string HandledNote { get; set; }

        public DateTime? HandledDate { get; set; }

        public string HandledBy { get; set; }
    }
}