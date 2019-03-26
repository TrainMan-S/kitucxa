using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Web.Models
{
    public class AppGroupVM
    {
        public int GroupId { get; set; }

        [Required]
        [StringLength(127)]
        public string GroupName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Level { get; set; }

        [Range(0, int.MaxValue)] 
        public int SortOrder { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public IEnumerable<AppRoleVM> AppRoles { set; get; }
    }
}