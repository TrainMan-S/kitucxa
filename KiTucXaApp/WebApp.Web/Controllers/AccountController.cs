using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApp.Model.Models;
using WebApp.Service.Services;
using WebApp.Web.App_Start;
using WebApp.Web.Infrastructure.Extensions;
using WebApp.Web.Models;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Controllers
{
    [RoutePrefix("api/account")]
    [Authorize]
    public class AccountController : ApiController
    {
        private AppSignInManager _signInManager;
        private AppUserManager _userManager;

        public AccountController(
            AppSignInManager signInManager,
            AppUserManager userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        //*********************************************************************************
        //*********************************************************************************
        //*********************************************************************************

        [AllowAnonymous]
        [Route("logout")]
        [HttpPost]
        public HttpResponseMessage Logout(HttpRequestMessage request)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return request.CreateResponse(HttpStatusCode.OK, "Đăng xuất thành công.");
        }

        [Route("change_password")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePassword(HttpRequestMessage request, ChangePasswordVM changePass)
        {
            if (changePass.NewPassword == changePass.ReNewPassword)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var check = await _userManager.CheckPasswordAsync(user, changePass.OldPassword);
                    if (check)
                    {
                        var result = await _userManager.ChangePasswordAsync(user.Id, changePass.OldPassword, changePass.NewPassword);
                        if (result.Succeeded)
                        {
                            return request.CreateResponse(HttpStatusCode.OK, "Thay đổi mật khẩu thành công");
                        }
                        else
                        {
                            return request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Lỗi không xác định");
                        }
                    }
                    else
                    {
                        return request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Mật khẩu không đúng");
                    }
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin tài khoản không chính xác");
                }
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Xác nhận mật khẩu không đúng");
            }
        }
    }
}
