using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Model.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(127)]
        public string Fullname { get; set; }

        public bool Gender { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool IsActived { get; set; }

        // *********************************
        // *********************************
        // *********************************

        [StringLength(255)]
        public string NativeLand { get; set; }

        [StringLength(127)]
        public string SchoolYear { get; set; }

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public DateTime CreatedDate { get; set; }

        [StringLength(127)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(127)]
        public string UpdatedBy { get; set; }

        // *********************************
        // *********************************
        // *********************************

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
