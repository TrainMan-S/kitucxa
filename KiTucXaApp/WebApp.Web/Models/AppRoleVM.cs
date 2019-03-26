using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class AppRoleVM
    {
        [StringLength(127)]
        public string Id { get; set; }

        [Required]
        [StringLength(127)]
        public string Name { get; set; }

        [Required]
        [StringLength(127)]
        public string Title { get; set; }

        //*****

        [Range(0, int.MaxValue)]
        public int RegionId { get; set; }

        public AppRegionVM AppRegion { set; get; }
    }
}