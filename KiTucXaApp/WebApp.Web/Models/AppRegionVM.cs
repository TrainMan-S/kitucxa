using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class AppRegionVM
    {
        public int RegionId { get; set; }

        [Required]
        [StringLength(127)]
        public string Title { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }

        //*****

        public IEnumerable<AppRoleVM> AppRoles { set; get; }
    }
}