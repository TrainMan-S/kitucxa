using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Model.Models;
using WebApp.Service.Services;
using WebApp.Web.App_Start;
using WebApp.Web.Infrastructure.Extensions;
using WebApp.Web.Models;

namespace WebApp.Web.Controllers
{
    [RoutePrefix("api/app_region")]
    [Authorize]
    public class AppRegionController : ApiController
    {
        private AppUserManager _userManager;
        private IAppUserService _appUserService;
        private IAppRoleService _appRoleService;
        private IAppRegionService _appRegionService;

        public AppRegionController(
            AppUserManager userManager,
            IAppUserService appUserService,
            IAppRoleService appRoleService,
            IAppRegionService appRegionService)
        {
            this._userManager = userManager;
            this._appUserService = appUserService;
            this._appRoleService = appRoleService;
            this._appRegionService = appRegionService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        [Authorize(Users = "superadmin")]
        [Route("get_regions")]
        [HttpGet]
        public HttpResponseMessage GetRegions(HttpRequestMessage requestMessage)
        {
            var appRegions = _appRegionService.GetRegions();
            var appRegionsVM = Mapper.Map<IQueryable<AppRegion>, List<AppRegionVM>>(appRegions);

            return requestMessage.CreateResponse(HttpStatusCode.OK, appRegionsVM);
        }

        [Authorize(Users = "superadmin")]
        [Route("get_regions_and_roles")]
        [HttpGet]
        public HttpResponseMessage GetRegionsAndRoles(HttpRequestMessage requestMessage)
        {
            var appRegions = _appRegionService.GetRegions();
            var appRegionsVM = Mapper.Map<IQueryable<AppRegion>, List<AppRegionVM>>(appRegions);

            if (appRegionsVM.Any())
            {
                foreach (var item in appRegionsVM)
                {
                    var roles = _appRoleService.GetRolesByRegionId(item.RegionId);
                    item.AppRoles = Mapper.Map<IQueryable<AppRole>, List<AppRoleVM>>(roles);
                }
            }

            return requestMessage.CreateResponse(HttpStatusCode.OK, appRegionsVM);
        }

        [Authorize(Users = "superadmin")]
        [Route("get_region_by_id")]
        [HttpGet]
        public HttpResponseMessage GetRegionById(HttpRequestMessage requestMessage, int id)
        {
            var dbAppRegion = _appRegionService.GetRegionById(id);
            if (dbAppRegion != null)
            {
                var appRegionVM = Mapper.Map<AppRegion, AppRegionVM>(dbAppRegion);
                return requestMessage.CreateResponse(HttpStatusCode.OK, appRegionVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

        [Authorize(Users = "superadmin")]
        [Route("add_region")]
        [HttpPost]
        public HttpResponseMessage AddRegion(HttpRequestMessage requestMessage, AppRegionVM appRegionVM)
        {
            if (ModelState.IsValid)
            {
                var newAppRegion = new AppRegion();
                newAppRegion.MapAppRegion(appRegionVM);

                _appRegionService.AddRegion(newAppRegion);
                _appRegionService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin chưa hợp lệ");
            }
        }

        [Authorize(Users = "superadmin")]
        [Route("edit_region")]
        [HttpPut]
        public HttpResponseMessage EditRegion(HttpRequestMessage requestMessage, AppRegionVM appRegionVM)
        {
            var dbAppRegion = _appRegionService.GetRegionById(appRegionVM.RegionId);
            if (dbAppRegion != null)
            {
                if (ModelState.IsValid)
                {
                    dbAppRegion.MapAppRegion(appRegionVM);

                    _appRegionService.UpdateRegion(dbAppRegion);
                    _appRegionService.SaveChanges();

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
        [Route("delete_region")]
        [HttpDelete]
        public HttpResponseMessage DeleteRegion(HttpRequestMessage requestMessage, int id)
        {
            var dbAppRegion = _appRegionService.GetRegionById(id);
            if (dbAppRegion != null)
            {
                try
                {
                    _appRegionService.DeleteRegion(id);
                    _appRegionService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
                }
                catch (Exception)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bản ghi không xóa được");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.NotFound, "Thông tin không tồn tại");
            }
        }

    }
}
