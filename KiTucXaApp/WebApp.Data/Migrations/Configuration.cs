namespace WebApp.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApp.Model.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Data.WebAppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebAppDbContext context)
        {
            CreateGroups(context);
            CreateUsers(context);
            CreateRegions(context);
            CreateRoles(context);
            CreateRoomTypes(context);
            CreateRoleForAdmin(context);
        }

        // Tạo nhóm tài khoản mặc định...
        private void CreateGroups(WebAppDbContext context)
        {
            if (!context.AppGroups.Any())
            {
                var groups = new List<AppGroup>();

                groups.Add(new AppGroup { GroupName = "SuperAdmin", Level = 1, SortOrder = 1 });
                groups.Add(new AppGroup { GroupName = "Admin", Level = 2, SortOrder = 2 });
                groups.Add(new AppGroup { GroupName = "Mod", Level = 3, SortOrder = 3 });
                groups.Add(new AppGroup { GroupName = "Staff", Level = 4, SortOrder = 4 });
                groups.Add(new AppGroup { GroupName = "Student", Level = 5, SortOrder = 5 });

                foreach (var item in groups)
                {
                    context.AppGroups.Add(item);
                }
                context.SaveChanges();
            }
        }

        // Tạo tài khoản mặc định...
        private void CreateUsers(WebAppDbContext context)
        {
            var _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new WebAppDbContext()));

            if (!_userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0838142857",
                    PhoneNumberConfirmed = true,

                    Fullname = "Super admin",
                    Gender = true,
                    Address = "Super admin",
                    GroupId = 1,

                    IsActived = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                };

                var result = _userManager.Create(user, "123456ok");
            }
        }

        // Tạo nhóm quyền mặc định...
        private void CreateRegions(WebAppDbContext context)
        {
            if (!context.AppRegions.Any())
            {
                var regions = new List<AppRegion>();
                regions.Add(new AppRegion { Title = "Tài khoản", SortOrder = 1 });
                regions.Add(new AppRegion { Title = "Nhóm tài khoản", SortOrder = 2 });
                regions.Add(new AppRegion { Title = "Loại phòng", SortOrder = 3 });
                regions.Add(new AppRegion { Title = "Phòng", SortOrder = 4 });
                regions.Add(new AppRegion { Title = "Hợp đồng", SortOrder = 5 });
                regions.Add(new AppRegion { Title = "Sửa chữa CSVC", SortOrder = 6 });
                regions.Add(new AppRegion { Title = "Kỷ luật", SortOrder = 7 });
                regions.Add(new AppRegion { Title = "Tiền phòng", SortOrder = 6 });
                regions.Add(new AppRegion { Title = "Tiền điện", SortOrder = 9 });
                regions.Add(new AppRegion { Title = "Tiền nước", SortOrder = 10 });
                regions.Add(new AppRegion { Title = "Chuyển phòng", SortOrder = 11 });
                regions.Add(new AppRegion { Title = "Góp ý", SortOrder = 12 });

                foreach (var item in regions)
                {
                    context.AppRegions.Add(item);
                }
                context.SaveChanges();
            }
        }

        // Tạo quyền mặc định...
        private void CreateRoles(WebAppDbContext context)
        {
            if (!context.AppRoles.Any())
            {
                var roles = new List<AppRole>();

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadAppUser", Title = "Xem tài khoản hệ thông", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateAppUser", Title = "Tạo tài khoản hệ thông", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateAppUser", Title = "Cập nhật tài khoản hệ thống", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteAppUser", Title = "Xóa tài khoản hệ thống", RegionId = 1 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadStudent", Title = "Xem tài khoản sinh viên", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateStudent", Title = "Tao tài khoản sinh viên", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateStudent", Title = "Cập nhật tài khoản sinh viên", RegionId = 1 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteStudent", Title = "Xóa tài khoản sinh viên", RegionId = 1 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadGroup", Title = "Xem nhóm tài khoản", RegionId = 2 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateGroup", Title = "Cập nhật nhóm tài khoản", RegionId = 2 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadRoomType", Title = "Xem loại phòng", RegionId = 3 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateRoomType", Title = "Tạo loại phòng", RegionId = 3 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateRoomType", Title = "Cập nhật loại phòng", RegionId = 3 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteRoomType", Title = "Xóa loại phòng", RegionId = 3 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadRoom", Title = "Xem phòng", RegionId = 4 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateRoom", Title = "Tạo phòng", RegionId = 4 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateRoom", Title = "Cập nhật phòng", RegionId = 4 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteRoom", Title = "Xóa phòng", RegionId = 4 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadIndenture", Title = "Xem hợp đồng", RegionId = 5 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateIndenture", Title = "Tạo hợp đồng", RegionId = 5 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CancelIndenture", Title = "Hủy hợp đồng", RegionId = 5 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteIndenture", Title = "Xóa hợp đồng", RegionId = 5 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadFurniture", Title = "Xem thông tin sửa chữa CSVC", RegionId = 6 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateFurniture", Title = "Tạo thông tin sửa chữa CSVC", RegionId = 6 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateFurniture", Title = "Cập nhật thông tin sửa chữa CSVC", RegionId = 6 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteFurniture", Title = "Xóa thông tin sửa chữa CSVC", RegionId = 6 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadDiscipline", Title = "Xem kỷ luật", RegionId = 7 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateDiscipline", Title = "Tạo kỷ luật", RegionId = 7 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateDiscipline", Title = "Cập nhật kỷ luật", RegionId = 7 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteDiscipline", Title = "Xóa kỷ luật", RegionId = 7 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadBillRoom", Title = "Xem tiền thuê phòng", RegionId = 8 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateBillRoom", Title = "Tạo tiền thuê phòng", RegionId = 8 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateBillRoom", Title = "Cập nhật tiền thuê phòng", RegionId = 8 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteBillRoom", Title = "Xóa tiền thuê phòng", RegionId = 8 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadBillElectric", Title = "Xem tiền điện phòng", RegionId = 9 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateBillElectric", Title = "Tạo tiền điện phòng", RegionId = 9 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateBillElectric", Title = "Cập nhật tiền điện phòng", RegionId = 9 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteBillElectric", Title = "Xóa tiền điện phòng", RegionId = 9 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadBillWater", Title = "Xem tiền nước phòng", RegionId = 10 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "CreateBillWater", Title = "Tạo tiền nước phòng", RegionId = 10 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "UpdateBillWater", Title = "Cập nhật nước điện phòng", RegionId = 10 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteBillWater", Title = "Xóa tiền nước phòng", RegionId = 10 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadSwitchRequest", Title = "Xem yêu cầu chuyển phòng", RegionId = 11 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReplySwitchRequest", Title = "Trả lời yêu cầu chuyển phòng", RegionId = 11 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteSwitchRequest", Title = "Xóa yêu cầu chuyển phòng", RegionId = 11 });

                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "ReadFeedback", Title = "Xem góp ý", RegionId = 12 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "AnswerFeedback", Title = "Trả lời góp ý", RegionId = 12 });
                roles.Add(new AppRole { Id = Guid.NewGuid().ToString(), Name = "DeleteFeedback", Title = "Xóa góp ý", RegionId = 12 });

                foreach (var item in roles)
                {
                    context.AppRoles.Add(item);
                }
                context.SaveChanges();
            }
        }

        // Tạo loại phòng mặc định...
        private void CreateRoomTypes(WebAppDbContext context)
        {
            if (!context.RoomTypes.Any())
            {
                var roomTypes = new List<RoomType>();

                roomTypes.Add(new RoomType { Title = "Loại phòng 1", Description = "Loại phòng 1", SortOrder = 1, IsActived = true, CreatedDate = DateTime.Now, CreatedBy = "System" });
                roomTypes.Add(new RoomType { Title = "Loại phòng 2", Description = "Loại phòng 2", SortOrder = 2, IsActived = true, CreatedDate = DateTime.Now, CreatedBy = "System" });
                roomTypes.Add(new RoomType { Title = "Loại phòng 3", Description = "Loại phòng 3", SortOrder = 3, IsActived = true, CreatedDate = DateTime.Now, CreatedBy = "System" });
                roomTypes.Add(new RoomType { Title = "Loại phòng 4", Description = "Loại phòng 4", SortOrder = 4, IsActived = true, CreatedDate = DateTime.Now, CreatedBy = "System" });
                roomTypes.Add(new RoomType { Title = "Loại phòng 5", Description = "Loại phòng 5", SortOrder = 5, IsActived = true, CreatedDate = DateTime.Now, CreatedBy = "System" });

                foreach (var item in roomTypes)
                {
                    context.RoomTypes.Add(item);
                }
                context.SaveChanges();
            }
        }

        // Add quyền cho admin
        private void CreateRoleForAdmin(WebAppDbContext context)
        {
            var _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new WebAppDbContext()));

            var admins = context.Users.Where(m => m.GroupId == 1).Select(m => m.Id).ToList();
            var roles = context.AppRoles.ToList();

            if (admins != null && admins.Any() && roles != null && roles.Any() && !context.AppGroupRoles.Any())
            {
                var arrRoles = roles.Select(m => m.Name).ToArray();
                foreach (var item in admins)
                {
                    var result = _userManager.AddToRoles(item, arrRoles);
                }

                foreach (var itemR in roles)
                {
                    var gr = new AppGroupRole { GroupId = 1, RoleId = itemR.Id };
                    context.AppGroupRoles.Add(gr);
                }

                context.SaveChanges();
            }
        }

    }
}
