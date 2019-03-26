using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    [Table("AppGroupRoles")]
    public class AppGroupRole
    {
        [Key]
        [Column(Order = 0)]
        public int GroupId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(127)]
        public string RoleId { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroups { get; set; }

        [ForeignKey("RoleId")]
        public virtual AppRole AppRoles { get; set; }
    }
}
