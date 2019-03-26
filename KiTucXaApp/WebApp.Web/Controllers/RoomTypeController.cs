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
    [RoutePrefix("api/room_type")]
    [Authorize]
    public class RoomTypeController : ApiController
    {
        private IRoomTypeService _roomTypeService;

        public RoomTypeController(
            IRoomTypeService roomTypeService)
        {
            this._roomTypeService = roomTypeService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadRoomType")]
        [Route("get_room_types")]
        [HttpGet]
        public HttpResponseMessage GetRoomTypes(HttpRequestMessage requestMessage)
        {
            var roomTypes = _roomTypeService.GetRoomTypes();
            var roomTypesVM = Mapper.Map<IQueryable<RoomType>, List<RoomTypeVM>>(roomTypes);

            return requestMessage.CreateResponse(HttpStatusCode.OK, roomTypesVM);
        }

        // Lấy danh sách active
        [Authorize(Roles = "ReadRoomType")]
        [Route("get_room_types_active")]
        [HttpGet]
        public HttpResponseMessage GetRoomTypesActive(HttpRequestMessage requestMessage)
        {
            var roomTypes = _roomTypeService.GetRoomTypesActive();
            var roomTypesVM = Mapper.Map<IQueryable<RoomType>, List<RoomTypeVM>>(roomTypes);

            return requestMessage.CreateResponse(HttpStatusCode.OK, roomTypesVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadRoomType")]
        [Route("get_room_type_by_id")]
        [HttpGet]
        public HttpResponseMessage GetRoomTypeById(HttpRequestMessage requestMessage, int id)
        {
            var roomType = _roomTypeService.GetRoomTypeById(id);
            if (roomType != null)
            {
                var roomTypeVM = Mapper.Map<RoomType, RoomTypeVM>(roomType);
                return requestMessage.CreateResponse(HttpStatusCode.OK, roomTypeVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateRoomType")]
        [Route("create_room_type")]
        [HttpPost]
        public HttpResponseMessage CreateRoomType(HttpRequestMessage requestMessage, RoomTypeVM roomTypeVM)
        {
            if (ModelState.IsValid)
            {
                var roomType = new RoomType();

                roomType.MapRoomType(roomTypeVM);
                roomType.CreatedBy = User.Identity.Name;
                roomType.CreatedDate = DateTime.Now;

                _roomTypeService.AddRoomType(roomType);
                _roomTypeService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateRoomType")]
        [Route("update_room_type")]
        [HttpPut]
        public HttpResponseMessage UpdateRoomType(HttpRequestMessage requestMessage, RoomTypeVM roomTypeVM)
        {
            if (ModelState.IsValid)
            {
                var roomType = _roomTypeService.GetRoomTypeById(roomTypeVM.RoomTypeId);
                if (roomType != null)
                {
                    roomType.MapRoomType(roomTypeVM);
                    roomType.UpdatedBy = User.Identity.Name;
                    roomType.UpdatedDate = DateTime.Now;

                    _roomTypeService.UpdateRoomType(roomType);
                    _roomTypeService.SaveChanges();

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
        [Authorize(Roles = "UpdateRoomType")]
        [Route("change_room_type")]
        [HttpDelete]
        public HttpResponseMessage ChangeRoomType(HttpRequestMessage requestMessage, int id)
        {
            var roomType = _roomTypeService.GetRoomTypeById(id);
            if (roomType != null)
            {
                roomType.IsActived = !roomType.IsActived;
                roomType.UpdatedBy = User.Identity.Name;
                roomType.UpdatedDate = DateTime.Now;

                _roomTypeService.UpdateRoomType(roomType);
                _roomTypeService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteRoomType")]
        [Route("delete_room_type")]
        [HttpDelete]
        public HttpResponseMessage DeleteRoomType(HttpRequestMessage requestMessage, int id)
        {
            var roomType = _roomTypeService.GetRoomTypeById(id);
            if (roomType != null)
            {
                _roomTypeService.DeleteRoomType(id);
                _roomTypeService.SaveChanges();

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
