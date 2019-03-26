using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebApp.Model.Models;

namespace WebApp.Data
{
    public class WebAppDbContext : IdentityDbContext<AppUser>
    {
        public WebAppDbContext() : base("KiTucXaApp")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        //--- Identity ---
        public DbSet<AppGroup> AppGroups { get; set; }
        public DbSet<AppGroupRole> AppGroupRoles { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppRegion> AppRegions { get; set; }

        // --- Content ---
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }   
        public DbSet<Indenture> Indentures { get; set; }
        public DbSet<BillRoom> BillRooms { get; set; }
        public DbSet<BillElectric> BillElectrics { get; set; }
        public DbSet<BillWater> BillWaters { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackAnswer> FeedbackAnswers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SwitchRequest> SwitchRequests { get; set; }

        // --- Database context ---
        public static WebAppDbContext Create()
        {
            return new WebAppDbContext();
        }

        // --- Rename table context ---
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>().HasKey(m => new { m.UserId, m.RoleId }).ToTable("AppUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(m => m.UserId).ToTable("AppUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("AppRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(m => m.UserId).ToTable("AppUserClaims");
        }
    }
}
