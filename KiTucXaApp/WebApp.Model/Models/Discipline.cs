using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("Disciplines")]
    public class Discipline
    {
        [Key]
        [StringLength(127)]
        public string DisciplineId { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime AtDate { get; set; }

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

        public DateTime CreatedDate { get; set; }

        [StringLength(127)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(127)]
        public string UpdatedBy { get; set; }

    }
}
