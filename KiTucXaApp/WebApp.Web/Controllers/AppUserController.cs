using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApp.Model.Models;
using WebApp.Service.Services;
using WebApp.Web.App_Start;
using WebApp.Web.Infrastructure.Extensions;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Controllers
{
    [RoutePrefix("api/app_user")]
    [Authorize]
    public class AppUserController : ApiController
    {
        private AppUserManager _userManager;
        private IAppUserService _appUserService;
        private IAppGroupService _appGroupService;
        private IAppRoleService _appRoleService;

        public AppUserController(
                AppUserManager userManager,
                IAppUserService appUserService,
                IAppGroupService appGroupService,
                IAppRoleService appRoleService)
        {
            this._userManager = userManager;
            this._appUserService = appUserService;
            this._appGroupService = appGroupService;
            this._appRoleService = appRoleService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region <-- Quản lý tài khoản hệ thống -->

        [Authorize(Roles = "ReadAppUser")]
        [Route("get_users")]
        [HttpGet]
        public HttpResponseMessage GetUsers(HttpRequestMessage requestMessage)
        {
            var users = _appUserService.GetUsersOfSystem();
            var usersVM = Mapper.Map<IQueryable<AppUser>, List<AppUserVM>>(users);

            return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
        }

        [Authorize(Roles = "ReadAppUser")]
        [Route("get_user_by_id")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetUserById(HttpRequestMessage requestMessage, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.GroupId != 5)
            {
                var usersVM = Mapper.Map<AppUser, AppUserVM>(user);
                return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "CreateAppUser")]
        [Route("create_user")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage requestMessage, CreateAppUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (_appUserService.CheckUsernameExist(string.Empty, userVM.UserName))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tên tài khoản đã tồn tại");
                }

                if (!_appGroupService.CheckGroupExits(userVM.GroupId) || userVM.GroupId == 5)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Nhóm tài khoản không hợp lệ");
                }

                var newUser = new AppUser();
                newUser.MapCreateAppUser(userVM);
                newUser.Id = Guid.NewGuid().ToString();
                newUser.IsActived = true;

                newUser.CreatedBy = User.Identity.Name;
                newUser.CreatedDate = DateTime.Now;

                var result = await _userManager.CreateAsync(newUser, userVM.Password);
                if (result.Succeeded)
                {
                    var roles = _appRoleService.GetRolesByGroupId(newUser.GroupId).Select(m => m.Name);
                    if (roles != null && roles.Any())
                    {
                        foreach (var item in roles)
                        {
                            await _userManager.AddToRoleAsync(newUser.Id, item);
                        }
                    }

                    _appGroupService.SaveChanges();
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
            }
        }

        [Authorize(Roles = "UpdateAppUser")]
        [Route("update_user")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateUser(HttpRequestMessage requestMessage, AppUserVM userVM)
        {
            var dbUser = await _userManager.FindByIdAsync(userVM.Id);
            if (dbUser != null || dbUser.GroupId != 5)
            {
                if (ModelState.IsValid)
                {
                    var bGroup = false;
                    var iGroupId = 0;
                    if (userVM.GroupId != dbUser.GroupId)
                    {
                        if (!_appGroupService.CheckGroupExits(userVM.GroupId) || userVM.GroupId != 5)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Nhóm tài khoản không hợp lệ");
                        }

                        bGroup = true;
                        iGroupId = dbUser.GroupId;
                    }

                    dbUser.MapAppUser(userVM);
                    dbUser.UpdatedBy = User.Identity.Name;
                    dbUser.UpdatedDate = DateTime.Now;

                    var resultUpdate = await _userManager.UpdateAsync(dbUser);
                    if (resultUpdate.Succeeded)
                    {
                        if (bGroup)
                        {
                            var oldRoles = _appRoleService.GetRolesByGroupId(iGroupId).Select(m => m.Name);
                            if (oldRoles != null && oldRoles.Any())
                            {
                                foreach (var item in oldRoles)
                                {
                                    await _userManager.RemoveFromRoleAsync(dbUser.Id, item);
                                }
                            }

                            var roles = _appRoleService.GetRolesByGroupId(dbUser.GroupId).Select(m => m.Name);
                            if (roles != null && roles.Any())
                            {
                                foreach (var item in roles)
                                {
                                    await _userManager.AddToRoleAsync(dbUser.Id, item);
                                }
                            }
                        }

                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "DeleteAppUser")]
        [Route("delete_user")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser(HttpRequestMessage requestMessage, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.GroupId != 5)
            {
                var dbRoles = _appRoleService.GetRolesByUserId(user.Id);
                if (dbRoles != null && dbRoles.Any())
                {
                    foreach (var item in dbRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(user.Id, item.Name);
                    }
                }

                var resultDeleteUser = await _userManager.DeleteAsync(user);
                if (resultDeleteUser.Succeeded)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Không xóa được");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "UpdateAppUser")]
        [Route("change_active_user")]
        [HttpDelete]
        public async Task<HttpResponseMessage> ChangeActiveUser(HttpRequestMessage requestMessage, string id)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser != null && dbUser.GroupId != 5)
            {
                dbUser.IsActived = !dbUser.IsActived;
                var result = await _userManager.UpdateAsync(dbUser);

                if (result.Succeeded)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thay đổi thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "UpdateAppUser")]
        [Route("reset_password_user")]
        [HttpDelete]
        public async Task<HttpResponseMessage> ResetPasswordUser(HttpRequestMessage requestMessage, string id)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser != null && dbUser.GroupId != 5)
            {
                var resultRemovePass = await _userManager.RemovePasswordAsync(dbUser.Id);
                if (resultRemovePass.Succeeded)
                {
                    var resultAddPass = await _userManager.AddPasswordAsync(dbUser.Id, "123456ok");
                    if (resultAddPass.Succeeded)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Reset thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        #endregion

        #region <-- Quản lý sinh viên -->

        [Authorize(Roles = "ReadStudent")]
        [Route("get_students")]
        [HttpGet]
        public HttpResponseMessage GetStudents(HttpRequestMessage requestMessage)
        {
            var users = _appUserService.GetUsersOfStudent();
            var usersVM = Mapper.Map<IQueryable<AppUser>, List<StudentVM>>(users);

            return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
        }

        [Authorize(Roles = "ReadStudent")]
        [Route("get_student_by_id")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetStudentById(HttpRequestMessage requestMessage, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.GroupId == 5)
            {
                var usersVM = Mapper.Map<AppUser, StudentVM>(user);
                return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "CreateStudent")]
        [Route("create_student")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateStudent(HttpRequestMessage requestMessage, StudentVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (_appUserService.CheckUsernameExist(string.Empty, userVM.UserName))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tên tài khoản đã tồn tại");
                }

                var newUser = new AppUser();
                newUser.MapCreateStudent(userVM);
                newUser.Id = Guid.NewGuid().ToString();
                newUser.IsActived = true;

                newUser.CreatedBy = User.Identity.Name;
                newUser.CreatedDate = DateTime.Now;

                var result = await _userManager.CreateAsync(newUser, userVM.UserName);
                if (result.Succeeded)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
            }
        }

        [Authorize(Roles = "UpdateStudent")]
        [Route("update_student")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateStudent(HttpRequestMessage requestMessage, StudentVM userVM)
        {
            var dbUser = await _userManager.FindByIdAsync(userVM.Id);
            if (dbUser != null || dbUser.GroupId == 5)
            {
                if (ModelState.IsValid)
                {
                    dbUser.MapStudent(userVM);
                    dbUser.UpdatedBy = User.Identity.Name;
                    dbUser.UpdatedDate = DateTime.Now;

                    var resultUpdate = await _userManager.UpdateAsync(dbUser);
                    if (resultUpdate.Succeeded)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "DeleteStudent")]
        [Route("delete_student")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteStudent(HttpRequestMessage requestMessage, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.GroupId == 5)
            {
                var resultDeleteUser = await _userManager.DeleteAsync(user);
                if (resultDeleteUser.Succeeded)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Không xóa được");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "UpdateStudent")]
        [Route("change_active_student")]
        [HttpDelete]
        public async Task<HttpResponseMessage> ChangeActiveStudent(HttpRequestMessage requestMessage, string id)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser != null && dbUser.GroupId == 5)
            {
                dbUser.IsActived = !dbUser.IsActived;
                var result = await _userManager.UpdateAsync(dbUser);

                if (result.Succeeded)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thay đổi thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "UpdateStudent")]
        [Route("reset_password_student")]
        [HttpDelete]
        public async Task<HttpResponseMessage> ResetPasswordStudent(HttpRequestMessage requestMessage, string id)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser != null && dbUser.GroupId == 5)
            {
                var resultRemovePass = await _userManager.RemovePasswordAsync(dbUser.Id);
                if (resultRemovePass.Succeeded)
                {
                    var resultAddPass = await _userManager.AddPasswordAsync(dbUser.Id, "123456ok");
                    if (resultAddPass.Succeeded)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Reset thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.NotModified, "Lỗi không xác định");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        #endregion

        #region <-- Dùng trong profile -->


        [Route("get_info")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInfo(HttpRequestMessage requestMessage)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                if (user.GroupId != 5)
                {
                    var usersVM = Mapper.Map<AppUser, AppUserVM>(user);
                    return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
                }
                else
                {
                    var usersVM = Mapper.Map<AppUser, StudentVM>(user);
                    return requestMessage.CreateResponse(HttpStatusCode.OK, usersVM);
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Route("update_info_user")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateInfoUser(HttpRequestMessage requestMessage, AppUserVM userVM)
        {
            var dbUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (dbUser != null && dbUser.Id == userVM.Id && dbUser.GroupId != 5)
            {
                if (ModelState.IsValid)
                {
                    dbUser.MapAppUser(userVM);

                    dbUser.UpdatedBy = User.Identity.Name;
                    dbUser.UpdatedDate = DateTime.Now;

                    var resultUpdate = await _userManager.UpdateAsync(dbUser);
                    if (resultUpdate.Succeeded)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Route("update_info_students")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateInfoStudent(HttpRequestMessage requestMessage, StudentVM userVM)
        {
            var dbUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (dbUser != null && dbUser.Id == userVM.Id && dbUser.GroupId == 5)
            {
                if (ModelState.IsValid)
                {
                    dbUser.MapStudent(userVM);

                    dbUser.UpdatedBy = User.Identity.Name;
                    dbUser.UpdatedDate = DateTime.Now;

                    var resultUpdate = await _userManager.UpdateAsync(dbUser);
                    if (resultUpdate.Succeeded)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        #endregion


    }
}
