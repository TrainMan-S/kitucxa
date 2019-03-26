using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class MessageVM
    {
        [StringLength(127)]
        public string MessageId { get; set; }

        [Required]
        [StringLength(1023)]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsViewed { get; set; }

        public DateTime? ViewedDate { get; set; }
    }
}