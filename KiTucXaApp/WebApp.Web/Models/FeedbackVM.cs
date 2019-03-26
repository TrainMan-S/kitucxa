using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class FeedbackVM
    {
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

        public string ViewedBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public IEnumerable<FeedbackAnswerVM> FeedbackAnswers { set; get; }
    }
}