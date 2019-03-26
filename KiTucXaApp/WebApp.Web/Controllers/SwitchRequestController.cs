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
    [RoutePrefix("api/switch_request")]
    [Authorize]
    public class SwitchRequestController : ApiController
    {
        private ISwitchRequestService _switchRequestService;
        private IRoomService _roomService;
        private IAppUserService _appUserService;

        public SwitchRequestController(
            ISwitchRequestService switchRequestService,
            IRoomService roomService,
            IAppUserService appUserService)
        {
            this._switchRequestService = switchRequestService;
            this._roomService = roomService;
            this._appUserService = appUserService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadSwitchRequest")]
        [Route("get_switch_requests")]
        [HttpGet]
        public HttpResponseMessage GetSwitchRequests(HttpRequestMessage requestMessage)
        {
            var switchRequests = _switchRequestService.GetSwitchRequests();
            var switchRequestsVM = Mapper.Map<IQueryable<SwitchRequest>, List<SwitchRequestVM>>(switchRequests);

            return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestsVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadSwitchRequest")]
        [Route("get_switch_requests_of_student_by_id")]
        [HttpGet]
        public HttpResponseMessage GetSwitchRequestsOfStudentById(HttpRequestMessage requestMessage, string userid)
        {
            var switchRequests = _switchRequestService.GetSwitchRequestsOfStudentById(userid);
            var switchRequestsVM = Mapper.Map<IQueryable<SwitchRequest>, List<SwitchRequestVM>>(switchRequests);

            return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestsVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadSwitchRequest")]
        [Route("get_switch_requests_of_student_by_name")]
        [HttpGet]
        public HttpResponseMessage GetSwitchRequestsOfStudentByName(HttpRequestMessage requestMessage, string username)
        {
            var switchRequests = _switchRequestService.GetSwitchRequestsOfStudentByName(username);
            var switchRequestsVM = Mapper.Map<IQueryable<SwitchRequest>, List<SwitchRequestVM>>(switchRequests);

            return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestsVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadSwitchRequest")]
        [Route("get_switch_request_by_id")]
        [HttpGet]
        public HttpResponseMessage GetSwitchRequestById(HttpRequestMessage requestMessage, string id)
        {
            var switchRequest = _switchRequestService.GetSwitchRequestById(id);
            if (switchRequest != null)
            {
                var switchRequestVM = Mapper.Map<SwitchRequest, SwitchRequestVM>(switchRequest);
                return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Trả lời yêu cầu
        [Authorize(Roles = "ReplySwitchRequest")]
        [Route("reply_switch_request")]
        [HttpPost]
        public HttpResponseMessage ReplySwitchRequest(HttpRequestMessage requestMessage, SwitchHandlVM switchHandlVM)
        {
            var switchRequest = _switchRequestService.GetSwitchRequestById(switchHandlVM.SwitchRequestId);
            if (switchRequest != null)
            {
                if (switchRequest.Status != null)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Yêu cầu đã được trả lời trước đó");
                }

                if (switchRequest.Status == true)
                {
                    var student = _appUserService.GetUserById(switchRequest.Id);
                    if (student == null || !student.IsActived)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản sinh viên không hợp lệ");
                    }

                    var fromRoom = _roomService.GetRoomById(switchRequest.FromRoomId);
                    if (fromRoom != null && fromRoom.RoomId != student.RoomId)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng đi không hợp lệ");
                    }

                    var toRoom = _roomService.GetRoomById(switchRequest.ToRoomId);
                    if (toRoom == null || !toRoom.IsActived)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng đến không hợp lệ");
                    }

                    if (toRoom.CapacityCur >= toRoom.CapacityMax)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng đến đã đầy");
                    }

                    student.RoomId = switchRequest.ToRoomId;
                    _appUserService.UpdateUser(student);

                    fromRoom.CapacityCur = fromRoom.CapacityCur - 1;
                    _roomService.UpdateRoom(fromRoom);

                    toRoom.CapacityCur = toRoom.CapacityCur + 1;
                    _roomService.UpdateRoom(toRoom);
                }

                switchRequest.Status = switchHandlVM.Status;
                switchRequest.HandledNote = switchHandlVM.HandledNote;
                switchRequest.HandledBy = User.Identity.Name;
                switchRequest.HandledDate = DateTime.Now;             
                _switchRequestService.UpdateSwitchRequest(switchRequest);
                    
                _switchRequestService.SaveChanges();
                return requestMessage.CreateResponse(HttpStatusCode.OK, "Trả lời thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteSwitchRequest")]
        [Route("delete_switch_request")]
        [HttpDelete]
        public HttpResponseMessage DeleteSwitchRequest(HttpRequestMessage requestMessage, string id)
        {
            var switchRequest = _switchRequestService.GetSwitchRequestById(id);
            if (switchRequest != null)
            {
                if (switchRequest.Status != null)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Yêu cầu đã được trả lời, không được xóa");
                }

                _switchRequestService.DeleteSwitchRequest(id);
                _switchRequestService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion

        #region < --- Dùng cho student --->

        // Lấy danh sách
        [Route("student_get_switch_requests")]
        [HttpGet]
        public HttpResponseMessage StudentGetSwitchRequests(HttpRequestMessage requestMessage)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null && student.GroupId == 5 && student.IsActived)
            {
                var switchRequests = _switchRequestService.GetSwitchRequestsOfStudentById(student.Id);
                var switchRequestsVM = Mapper.Map<IQueryable<SwitchRequest>, List<SwitchRequestVM>>(switchRequests);

                return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestsVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
            }   
        }

        // Lấy theo ID
        [Route("student_get_switch_request_by_id")]
        [HttpGet]
        public HttpResponseMessage StudentGetSwitchRequestById(HttpRequestMessage requestMessage, string id)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null && student.GroupId == 5 && student.IsActived)
            {
                var switchRequest = _switchRequestService.GetSwitchRequestById(id);
                if (switchRequest != null && switchRequest.Id != student.Id)
                {
                    var switchRequestVM = Mapper.Map<SwitchRequest, SwitchRequestVM>(switchRequest);
                    return requestMessage.CreateResponse(HttpStatusCode.OK, switchRequestVM);
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
            }  
        }

        // Tạo mới
        [Route("student_create_switch_request")]
        [HttpPost]
        public HttpResponseMessage StudentCreateSwitchRequest(HttpRequestMessage requestMessage, SwitchRequestVM switchRequestVM)
        {
            if (ModelState.IsValid)
            {
                var student = _appUserService.GetUserByName(User.Identity.Name);
                if (student != null && student.GroupId == 5 && student.IsActived)
                {
                    if (_switchRequestService.CheckSwitchRequestForStudent(student.Id))
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bạn đã có yêu cầu trước đó, không được tạo thêm yêu cầu");
                    }

                    if (switchRequestVM.FromRoomId == switchRequestVM.ToRoomId)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng đi và phòng đến không được trùng nhau");
                    }

                    var fromRoom = _roomService.GetRoomById(switchRequestVM.FromRoomId);
                    if (fromRoom == null || fromRoom.RoomId != student.RoomId)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng bắt đầu không hợp lệ");
                    }

                    var toRoom = _roomService.GetRoomById(switchRequestVM.ToRoomId);
                    if (toRoom == null || !toRoom.IsActived)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng đến không hợp lệ");
                    }

                    if (toRoom.CapacityCur >= toRoom.CapacityMax )
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng đến đã đầy");
                    }

                    var switchRequest = new SwitchRequest();

                    switchRequest.MapSwitchRequest(switchRequestVM);
                    switchRequest.SwitchRequestId = Guid.NewGuid().ToString();
                    switchRequest.Status = null;
                    switchRequest.CreatedDate = DateTime.Now;

                    switchRequest.FromRoomCode = fromRoom.Code;
                    switchRequest.FromRoomDesc = fromRoom.Description;
                    switchRequest.ToRoomCode = toRoom.Code;
                    switchRequest.ToRoomDesc = toRoom.Description;

                    switchRequest.Id = student.Id;

                    _switchRequestService.AddSwitchRequest(switchRequest);
                    _switchRequestService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
                }   
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Route("student_update_switch_request")]
        [HttpPut]
        public HttpResponseMessage StudentUpdateSwitchRequest(HttpRequestMessage requestMessage, SwitchRequestVM switchRequestVM)
        {
            if (ModelState.IsValid)
            {
                var student = _appUserService.GetUserByName(User.Identity.Name);
                if (student != null && student.GroupId == 5 && student.IsActived)
                {
                    var switchRequest = _switchRequestService.GetSwitchRequestById(switchRequestVM.SwitchRequestId);
                    if (switchRequest != null)
                    {
                        if (switchRequest.Status != null)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Yêu cầu đã được trả lời, không được sửa");
                        }

                        if (switchRequestVM.FromRoomId == switchRequestVM.ToRoomId)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng đi và phòng đến không được trùng nhau");
                        }

                        var fromRoom = _roomService.GetRoomById(switchRequestVM.FromRoomId);
                        if (fromRoom == null || fromRoom.RoomId != student.RoomId)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng bắt đầu không hợp lệ");
                        }

                        var toRoom = _roomService.GetRoomById(switchRequestVM.ToRoomId);
                        if (toRoom == null || !toRoom.IsActived)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng đến không hợp lệ");
                        }

                        if (toRoom.CapacityCur >= toRoom.CapacityMax)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Phòng đến đã đầy");
                        }

                        switchRequest.MapSwitchRequest(switchRequestVM);
                        switchRequest.UpdatedDate = DateTime.Now;

                        _switchRequestService.UpdateSwitchRequest(switchRequest);
                        _switchRequestService.SaveChanges();

                        return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
                    }
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Route("student_delete_switch_request")]
        [HttpDelete]
        public HttpResponseMessage StudentDeleteSwitchRequest(HttpRequestMessage requestMessage, string id)
        {
            var switchRequest = _switchRequestService.GetSwitchRequestById(id);
            if (switchRequest != null)
            {
                if (switchRequest.Status != null)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Yêu cầu đã được trả lời, không được xóa");
                }

                _switchRequestService.DeleteSwitchRequest(id);
                _switchRequestService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion
    }
}
