using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model.Models
{
    public class AppRole : IdentityRole
    {
        [StringLength(127)]
        public string Title { get; set; }

        // *****
        [Required]
        public int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public virtual AppRegion AppRegion { get; set; }
    }
}
