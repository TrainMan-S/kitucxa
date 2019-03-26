using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key]
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
