using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("AppRegions")]
    public class AppRegion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegionId { get; set; }

        [Required]
        [StringLength(127)]
        public string Title { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }
    }
}
