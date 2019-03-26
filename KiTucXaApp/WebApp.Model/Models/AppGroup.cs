using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("AppGroups")]
    public class AppGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [Required]
        [StringLength(127)]
        public string GroupName { get; set; }

        [Range(0, int.MaxValue)]
        public int Level { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }
    }
}
