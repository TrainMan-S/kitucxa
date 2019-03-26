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
    [RoutePrefix("api/room")]
    [Authorize]
    public class RoomController : ApiController
    {
        private IRoomService _roomService;
        private IRoomTypeService _roomTypeService;

        public RoomController(
            IRoomService roomService,
            IRoomTypeService roomTypeService)
        {
            this._roomService = roomService;
            this._roomTypeService = roomTypeService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadRoom")]
        [Route("get_rooms")]
        [HttpGet]
        public HttpResponseMessage GetRooms(HttpRequestMessage requestMessage)
        {
            var rooms = _roomService.GetRooms();
            var roomsVM = Mapper.Map<IQueryable<Room>, List<RoomVM>>(rooms);

            return requestMessage.CreateResponse(HttpStatusCode.OK, roomsVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadRoom")]
        [Route("get_room_by_id")]
        [HttpGet]
        public HttpResponseMessage GetRoomById(HttpRequestMessage requestMessage, int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room != null)
            {
                var roomVM = Mapper.Map<Room, RoomVM>(room);
                return requestMessage.CreateResponse(HttpStatusCode.OK, roomVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateRoom")]
        [Route("create_room")]
        [HttpPost]
        public HttpResponseMessage CreateRoom(HttpRequestMessage requestMessage, RoomVM roomVM)
        {
            if (ModelState.IsValid)
            {
                if (!_roomTypeService.CheckRoomTypeActive(roomVM.RoomTypeId))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Loại phòng không hợp lệ");
                }

                if (_roomService.CheckRoomExistByCode(roomVM.Code))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tên phòng đã tồn tại");
                }

                var room = new Room();

                room.MapRoom(roomVM);
                room.Code = roomVM.Code;
                room.CapacityCur = 0;
                room.CreatedBy = User.Identity.Name;
                room.CreatedDate = DateTime.Now;

                _roomService.AddRoom(room);
                _roomService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateRoom")]
        [Route("update_room")]
        [HttpPut]
        public HttpResponseMessage UpdateRoom(HttpRequestMessage requestMessage, RoomVM roomVM)
        {
            if (ModelState.IsValid)
            {
                var room = _roomService.GetRoomById(roomVM.RoomId);
                if (room != null)
                {
                    if (!_roomTypeService.CheckRoomTypeActive(roomVM.RoomTypeId))
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Loại phòng không hợp lệ");
                    }

                    room.MapRoom(roomVM);
                    room.UpdatedBy = User.Identity.Name;
                    room.UpdatedDate = DateTime.Now;

                    _roomService.UpdateRoom(room);
                    _roomService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
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

        // Change active
        [Authorize(Roles = "UpdateRoom")]
        [Route("change_room")]
        [HttpDelete]
        public HttpResponseMessage ChangeRoom(HttpRequestMessage requestMessage, int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room != null)
            {
                room.IsActived = !room.IsActived;
                room.UpdatedBy = User.Identity.Name;
                room.UpdatedDate = DateTime.Now;

                _roomService.UpdateRoom(room);
                _roomService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteRoom")]
        [Route("delete_room")]
        [HttpDelete]
        public HttpResponseMessage DeleteRoom(HttpRequestMessage requestMessage, int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room != null)
            {
                _roomService.DeleteRoom(id);
                _roomService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion

        #region < --- Dùng chung --->

        // Lấy danh sách active
        [Route("get_rooms_active")]
        [HttpGet]
        public HttpResponseMessage GetRoomsActive(HttpRequestMessage requestMessage)
        {
            var rooms = _roomService.GetRoomsActive();
            var roomsVM = Mapper.Map<IQueryable<Room>, List<RoomVM>>(rooms);

            return requestMessage.CreateResponse(HttpStatusCode.OK, roomsVM);
        }

        #endregion


    }
}
