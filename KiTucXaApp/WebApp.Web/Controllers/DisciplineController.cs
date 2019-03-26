using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Model.Models;
using WebApp.Service.Services;
using WebApp.Web.Infrastructure.Extensions;
using WebApp.Web.Models;

namespace WebApp.Web.Controllers
{
    [RoutePrefix("api/discipline")]
    [Authorize]
    public class DisciplineController : ApiController
    {
        private IDisciplineService _disciplineService;
        private IRoomService _roomService;
        private IAppUserService _appUserService;

        public DisciplineController(
            IDisciplineService disciplineService,
            IRoomService roomService,
            IAppUserService appUserService)
        {
            this._disciplineService = disciplineService;
            this._roomService = roomService;
            this._appUserService = appUserService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadDiscipline")]
        [Route("get_disciplines")]
        [HttpGet]
        public HttpResponseMessage GetDisciplines(HttpRequestMessage requestMessage)
        {
            var disciplines = _disciplineService.GetDisciplines();
            var disciplinesVM = Mapper.Map<IQueryable<Discipline>, List<DisciplineVM>>(disciplines);

            return requestMessage.CreateResponse(HttpStatusCode.OK, disciplinesVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadDiscipline")]
        [Route("get_disciplines_of_student_by_id")]
        [HttpGet]
        public HttpResponseMessage GetDisciplinesOfStudentById(HttpRequestMessage requestMessage, string userId)
        {
            var student = _appUserService.GetUserById(userId);
            if (student != null || student.IsActived && student.GroupId == 5)
            {
                var disciplines = _disciplineService.GetDisciplinesOfStudentById(userId);
                var disciplinesVM = Mapper.Map<IQueryable<Discipline>, List<DisciplineVM>>(disciplines);

                return requestMessage.CreateResponse(HttpStatusCode.OK, disciplinesVM);
            }
            else {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin sinh viên không hợp lệ");
            }
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadDiscipline")]
        [Route("get_disciplines_of_student_by_name")]
        [HttpGet]
        public HttpResponseMessage GetDisciplinesOfStudentByName(HttpRequestMessage requestMessage, string username)
        {
            var student = _appUserService.GetUserById(username);
            if (student != null || student.IsActived && student.GroupId == 5)
            {
                var disciplines = _disciplineService.GetDisciplinesOfStudentByName(username);
                var disciplinesVM = Mapper.Map<IQueryable<Discipline>, List<DisciplineVM>>(disciplines);

                return requestMessage.CreateResponse(HttpStatusCode.OK, disciplinesVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin sinh viên không hợp lệ");
            }     
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadDiscipline")]
        [Route("get_discipline_by_id")]
        [HttpGet]
        public HttpResponseMessage GetDisciplineById(HttpRequestMessage requestMessage, string id)
        {
            var discipline = _disciplineService.GetDisciplineById(id);
            if (discipline != null)
            {
                var disciplineVM = Mapper.Map<Discipline, DisciplineVM>(discipline);
                return requestMessage.CreateResponse(HttpStatusCode.OK, disciplineVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateDiscipline")]
        [Route("create_discipline")]
        [HttpPost]
        public HttpResponseMessage CreateDiscipline(HttpRequestMessage requestMessage, DisciplineVM disciplineVM)
        {
            if (ModelState.IsValid)
            {
                var student = _appUserService.GetUserById(disciplineVM.Id);
                if (student != null && student.GroupId == 5)
                {
                    disciplineVM.AtDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(disciplineVM.AtDate, "North Asia Standard Time").Date;
                    var discipline = new Discipline();

                    discipline.MapDiscipline(disciplineVM);
                    discipline.DisciplineId = Guid.NewGuid().ToString();
                    discipline.CreatedBy = User.Identity.Name;
                    discipline.CreatedDate = DateTime.Now;

                    _disciplineService.AddDiscipline(discipline);
                    _disciplineService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin sinh viên không hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateDiscipline")]
        [Route("update_discipline")]
        [HttpPut]
        public HttpResponseMessage UpdateDiscipline(HttpRequestMessage requestMessage, DisciplineVM disciplineVM)
        {
            if (ModelState.IsValid)
            {
                var discipline = _disciplineService.GetDisciplineById(disciplineVM.DisciplineId);
                if (discipline != null)
                {
                    var student = _appUserService.GetUserById(disciplineVM.Id);
                    if (student != null || student.IsActived && student.GroupId == 5)
                    {
                        disciplineVM.AtDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(disciplineVM.AtDate, "North Asia Standard Time").Date;

                        discipline.MapDiscipline(disciplineVM);
                        discipline.UpdatedBy = User.Identity.Name;
                        discipline.UpdatedDate = DateTime.Now;

                        _disciplineService.UpdateDiscipline(discipline);
                        _disciplineService.SaveChanges();
                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin sinh viên không hợp lệ");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteDiscipline")]
        [Route("delete_discipline")]
        [HttpDelete]
        public HttpResponseMessage DeleteDiscipline(HttpRequestMessage requestMessage, string id)
        {
            var discipline = _disciplineService.GetDisciplineById(id);
            if (discipline != null)
            {
                _disciplineService.DeleteDiscipline(id);
                _disciplineService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion

        #region < --- Dùng cho sinh viên --->

        // Lấy danh sách
        [Route("student_get_disciplines")]
        [HttpGet]
        public HttpResponseMessage StudentGetDisciplines(HttpRequestMessage requestMessage)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null && student.IsActived && student.GroupId == 5)
            {
                var disciplines = _disciplineService.GetDisciplinesOfStudentById(student.Id);
                var disciplinesVM = Mapper.Map<IQueryable<Discipline>, List<DisciplineVM>>(disciplines);

                return requestMessage.CreateResponse(HttpStatusCode.OK, disciplinesVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
            }
        }

        #endregion
    }
}
