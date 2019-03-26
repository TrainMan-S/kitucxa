using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Feedbacks")]
    public class Feedback
    {
        [Key]
        [StringLength(127)]
        public string FeedbackId { get; set; }

        [Required]
        [StringLength(1023)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsIncognito { get; set; }

        public bool IsAnswered { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        [StringLength(127)]
        public string StudentId { get; set; }

        [StringLength(127)]
        public string StudentCode { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public bool IsViewed { get; set; }

        public DateTime? ViewedDate { get; set; }

        [StringLength(127)]
        public string ViewedBy { get; set; }
    }
}
