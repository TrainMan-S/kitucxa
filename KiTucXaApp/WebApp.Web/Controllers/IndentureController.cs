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
    [RoutePrefix("api/indenture")]
    [Authorize]
    public class IndentureController : ApiController
    {
        private IIndentureService _indentureService;
        private IRoomService _roomService;
        private IAppUserService _appUserService;

        public IndentureController(
            IIndentureService indentureService,
            IRoomService roomService,
            IAppUserService appUserService)
        {
            this._indentureService = indentureService;
            this._roomService = roomService;
            this._appUserService = appUserService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadIndenture")]
        [Route("get_indentures")]
        [HttpGet]
        public HttpResponseMessage GetIndentures(HttpRequestMessage requestMessage)
        {
            var indentures = _indentureService.GetIndentures();
            var indenturesVM = Mapper.Map<IQueryable<Indenture>, List<IndentureVM>>(indentures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, indenturesVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadIndenture")]
        [Route("get_indentures_of_student_by_id")]
        [HttpGet]
        public HttpResponseMessage GetIndenturesOfStudentById(HttpRequestMessage requestMessage, string userid)
        {
            var indentures = _indentureService.GetIndenturesOfStudentById(userid);
            var indenturesVM = Mapper.Map<IQueryable<Indenture>, List<IndentureVM>>(indentures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, indenturesVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadIndenture")]
        [Route("get_indentures_of_student_by_name")]
        [HttpGet]
        public HttpResponseMessage GetIndenturesOfStudentByName(HttpRequestMessage requestMessage, string username)
        {
            var indentures = _indentureService.GetIndenturesOfStudentByName(username);
            var indenturesVM = Mapper.Map<IQueryable<Indenture>, List<IndentureVM>>(indentures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, indenturesVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadIndenture")]
        [Route("get_indenture_by_id")]
        [HttpGet]
        public HttpResponseMessage GetIndentureById(HttpRequestMessage requestMessage, string id)
        {
            var indenture = _indentureService.GetIndentureById(id);
            if (indenture != null)
            {
                var indentureVM = Mapper.Map<Indenture, IndentureVM>(indenture);
                return requestMessage.CreateResponse(HttpStatusCode.OK, indentureVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateIndenture")]
        [Route("create_indenture")]
        [HttpPost]
        public HttpResponseMessage CreateIndenture(HttpRequestMessage requestMessage, IndentureVM indentureVM)
        {
            if (ModelState.IsValid)
            {
                indentureVM.DateFrom = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(indentureVM.DateFrom, "North Asia Standard Time").Date;
                indentureVM.DateTo = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(indentureVM.DateTo, "North Asia Standard Time").Date;

                if (indentureVM.DateFrom <= indentureVM.DateTo)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thời gian của hợp đồng không hợp lệ");
                }

                var student = _appUserService.GetUserById(indentureVM.Id);
                if (student == null || student.GroupId != 5 || !student.IsActived)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản sinh viên không hợp lệ");
                }

                var room = _roomService.GetRoomById(indentureVM.RoomId);
                if (room == null || !room.IsActived)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng không hợp lệ");
                }

                if (_indentureService.CheckHasIndenturesValid(student.Id))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Sinh viên đã có hợp đồng còn hiệu lực tời hiện tại");
                }

                var capacityCur = _roomService.CountCapacityNowOfRoom(room.RoomId);
                if (capacityCur >= room.CapacityMax)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng hiện tại đã đầy");
                }

                var indenture = new Indenture();
                indenture.MapIndenture(indentureVM);
                indenture.IndentureId = Guid.NewGuid().ToString();
                indenture.Id = student.Id;
                indenture.RoomId = room.RoomId;
                indenture.IsCanceled = false;
                indenture.CreatedBy = User.Identity.Name;
                indenture.CreatedDate = DateTime.Now;

                _indentureService.AddIndenture(indenture);
                _indentureService.SaveChanges();

                student.RoomId = room.RoomId;
                _appUserService.UpdateUser(student);
                _appUserService.SaveChanges();

                room.CapacityCur = _roomService.CountCapacityNowOfRoom(room.RoomId);
                _roomService.UpdateRoom(room);
                _roomService.SaveChanges();
                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        // Việc cập nhật xử lý rất phức tạp vekalo (liên quan đến xác định sĩ số trong phòng) nên không cho cập nhật - nếu tạo lỗi thì xóa đi rồi tạo mới (Hết)

        // Hủy hợp đồng
        [Authorize(Roles = "CancelIndenture")]
        [Route("cancel_indenture")]
        [HttpDelete]
        public HttpResponseMessage CancelIndenture(HttpRequestMessage requestMessage, string id)
        {
            var indenture = _indentureService.GetIndentureById(id);
            if (indenture != null)
            {
                if (indenture.DateTo < DateTime.Now.Date)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Hợp đồng đã hết hạn, không hủy được");
                }

                if (indenture.IsCanceled)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Hợp đồng đã bị hủy trước đó");
                }

                indenture.IsCanceled = true;
                indenture.CanceledBy = User.Identity.Name;
                indenture.CanceledDate = DateTime.Now;
                _indentureService.UpdateIndenture(indenture);
                
                var student = _appUserService.GetUserById(indenture.Id);
                if (student != null && student.RoomId == indenture.RoomId)
                {
                    student.RoomId = null;
                    _appUserService.UpdateUser(student);

                    var room = _roomService.GetRoomById(indenture.RoomId);
                    if (room != null)
                    {
                        room.CapacityCur = room.CapacityCur - 1;
                        _roomService.UpdateRoom(room);
                    }
                }

                _indentureService.SaveChanges();
                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteIndenture")]
        [Route("delete_indenture")]
        [HttpDelete]
        public HttpResponseMessage DeleteIndenture(HttpRequestMessage requestMessage, string id)
        {
            var indenture = _indentureService.GetIndentureById(id);
            if (indenture != null)
            {
                if (indenture.DateTo < DateTime.Now.Date)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Hợp đồng đã hết hạn, không được xóa");
                }

                if (!indenture.IsCanceled)
                {
                    var student = _appUserService.GetUserById(indenture.Id);
                    if (student != null && student.RoomId == indenture.RoomId)
                    {
                        student.RoomId = null;
                        _appUserService.UpdateUser(student);

                        var room = _roomService.GetRoomById(indenture.RoomId);
                        if (room != null)
                        {
                            room.CapacityCur = room.CapacityCur - 1;
                            _roomService.UpdateRoom(room);
                        }
                    }
                }

                _indentureService.DeleteIndenture(id);
                _indentureService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion

        #region < --- Dùng trong profile của sinh viên --->

        // Lấy danh sách
        [Route("get_indentures_of_student")]
        [HttpGet]
        public HttpResponseMessage GetIndenturesOfStudentByName(HttpRequestMessage requestMessage)
        {
            var indentures = _indentureService.GetIndenturesOfStudentByName(User.Identity.Name);
            var indenturesVM = Mapper.Map<IQueryable<Indenture>, List<IndentureVM>>(indentures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, indenturesVM);
        }

        #endregion
    }
}
