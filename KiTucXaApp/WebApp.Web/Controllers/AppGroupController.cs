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
using WebApp.Web.Models;

namespace WebApp.Web.Controllers
{
    [RoutePrefix("api/app_group")]
    [Authorize]
    public class AppGroupController : ApiController
    {
        private AppUserManager _userManager;
        private IAppRoleService _appRoleService;
        private IAppGroupRoleService _appGroupRoleService;
        private IAppGroupService _appGroupService;
        private IAppUserService _appUserService;


        public AppGroupController(
            AppUserManager userManager,
            IAppRoleService appRoleService,
            IAppGroupRoleService appGroupRoleService,
            IAppGroupService appGroupService,
            IAppUserService appUserService)
        {
            this._userManager = userManager;
            this._appRoleService = appRoleService;
            this._appGroupRoleService = appGroupRoleService;
            this._appGroupService = appGroupService;
            this._appUserService = appUserService;

        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        [Authorize(Roles = "ReadGroup")]
        [Route("get_groups")]
        [HttpGet]
        public HttpResponseMessage GetGroups(HttpRequestMessage requestMessage)
        {
            var groups = _appGroupService.GetGroups();
            var groupsVM = Mapper.Map<IQueryable<AppGroup>, IEnumerable<AppGroupVM>>(groups);
            return requestMessage.CreateResponse(HttpStatusCode.OK, groupsVM);
        }

        [Authorize(Roles = "ReadGroup")]
        [Route("get_group_by_id")]
        [HttpGet]
        public HttpResponseMessage GetGroupById(HttpRequestMessage requestMessage, int id)
        {
            var dbGroup = _appGroupService.GetGroupById(id);
            if (dbGroup != null)
            {
                var groupVM = Mapper.Map<AppGroup, AppGroupVM>(dbGroup);

                var roles = _appRoleService.GetRolesByGroupId(id);
                groupVM.AppRoles = Mapper.Map<IQueryable<AppRole>, IEnumerable<AppRoleVM>>(roles);

                return requestMessage.CreateResponse(HttpStatusCode.OK, groupVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Roles = "UpdateGroup")]
        [Route("edit_group")]
        [HttpPut]
        public async Task<HttpResponseMessage> EditGroup(HttpRequestMessage requestMessage, AppGroupVM groupVM)
        {
            var dbGroup = _appGroupService.GetGroupById(groupVM.GroupId);
            if (dbGroup != null)
            {
                if (ModelState.IsValid)
                {
                    var users = _appUserService.GetUsersByGroup(dbGroup.GroupId);
                    if (users != null && users.Any())
                    {
                        foreach (var itemUser in users)
                        {
                            var rolesOfGroup = _appRoleService.GetRolesByGroupId(dbGroup.GroupId).Select(m => m.Name);
                            if (rolesOfGroup != null && rolesOfGroup.Any())
                            {
                                foreach (var itemRole in rolesOfGroup)
                                {
                                    await _userManager.RemoveFromRoleAsync(itemUser.Id, itemRole);
                                }
                            }
                        }
                    }

                    if (users != null && users.Any() && groupVM.AppRoles != null && groupVM.AppRoles.Any())
                    {
                        foreach (var itemUser in users)
                        {
                            foreach (var itemRole in groupVM.AppRoles)
                            {
                                var role = _appRoleService.GetRoleById(itemRole.Id);
                                if (role != null)
                                {
                                    await _userManager.AddToRolesAsync(itemUser.Id, role.Name);
                                }
                            }
                        }
                    }

                    _appGroupRoleService.DeleteGroupRoleByGroup(dbGroup.GroupId);

                    if (groupVM.AppRoles != null && groupVM.AppRoles.Any())
                    {
                        foreach (var itemRole in groupVM.AppRoles)
                        {
                            _appGroupRoleService.AddGroupRole(new AppGroupRole { GroupId = dbGroup.GroupId, RoleId = itemRole.Id });
                        }
                    }

                    dbGroup.MapAppGroup(groupVM);
                    _appGroupService.UpdateGroup(dbGroup);
                    _appGroupService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
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

    }
}
