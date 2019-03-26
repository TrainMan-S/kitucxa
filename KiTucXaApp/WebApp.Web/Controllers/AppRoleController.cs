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
    [RoutePrefix("api/app_role")]
    [Authorize]
    public class AppRoleController : ApiController
    {
        private AppUserManager _userManager;
        private IAppUserService _appUserService;
        private IAppRoleService _appRoleService;

        public AppRoleController(
            AppUserManager userManager,
            IAppUserService appUserService,
            IAppRoleService appRoleService)
        {
            this._userManager = userManager;
            this._appUserService = appUserService;
            this._appRoleService = appRoleService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        [Authorize(Users = "superadmin")]
        [Route("get_roles")]
        [HttpGet]
        public HttpResponseMessage GetRoles(HttpRequestMessage requestMessage)
        {
            var roles = _appRoleService.GetRoles();
            var rolesVM = Mapper.Map<IQueryable<AppRole>, IEnumerable<AppRoleVM>>(roles);

            return requestMessage.CreateResponse(HttpStatusCode.OK, rolesVM);
        }

        [Authorize(Users = "superadmin")]
        [Route("get_role_by_id")]
        [HttpGet]
        public HttpResponseMessage GetRoleById(HttpRequestMessage requestMessage, string id)
        {
            var dbRole = _appRoleService.GetRoleById(id);
            if (dbRole != null)
            {
                var roleVM = Mapper.Map<AppRole, AppRoleVM>(dbRole);
                return requestMessage.CreateResponse(HttpStatusCode.OK, roleVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Users = "superadmin")]
        [Route("add_role")]
        [HttpPost]
        public HttpResponseMessage AddRole(HttpRequestMessage requestMessage, AppRoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                if (_appRoleService.CheckNameRole(roleVM.Name))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Quyền đã tồn tại");
                }

                var newRole = new AppRole();
                newRole.MapAppRole(roleVM);
                newRole.Id = Guid.NewGuid().ToString();
                newRole.Name = roleVM.Name;

                _appRoleService.AddRole(newRole);
                _appRoleService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
            }
        }

        [Authorize(Users = "superadmin")]
        [Route("edit_role")]
        [HttpPut]
        public HttpResponseMessage EditRole(HttpRequestMessage requestMessage, AppRoleVM roleVM)
        {
            var dbRole = _appRoleService.GetRoleById(roleVM.Id);
            if (dbRole != null)
            {
                if (ModelState.IsValid)
                {
                    dbRole.MapAppRole(roleVM);

                    _appRoleService.UpdateRole(dbRole);
                    _appRoleService.SaveChanges();

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

        [Authorize(Users = "superadmin")]
        [Route("delete_role")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteRole(HttpRequestMessage requestMessage, string id)
        {
            var dbRole = _appRoleService.GetRoleById(id);
            if (dbRole != null)
            {
                var users = _appUserService.GetUsersByRole(id);
                if (users != null && users.Any())
                {
                    foreach (var item in users)
                    {
                        await _userManager.RemoveFromRoleAsync(item.Id, dbRole.Name);
                    }
                }

                _appRoleService.DeleteRole(id);
                _appRoleService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

    }
}
