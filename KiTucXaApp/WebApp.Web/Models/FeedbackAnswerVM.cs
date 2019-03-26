using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class FeedbackAnswerVM
    {
        [StringLength(127)]
        public string FeedbackAnswerId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime AnsweredDate { get; set; }

        [StringLength(127)]
        public string AnsweredBy { get; set; }
    }
}