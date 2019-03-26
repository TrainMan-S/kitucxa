using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("FeedbackAnswers")]
    public class FeedbackAnswer
    {
        [Key]
        [StringLength(127)]
        public string FeedbackAnswerId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime AnsweredDate { get; set; }

        [StringLength(127)]
        public string AnsweredBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [Required]
        [StringLength(127)]
        public string FeedbackId { get; set; }

        [ForeignKey("FeedbackId")]
        public virtual Feedback Feedback { get; set; }
    }
}
