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
    [RoutePrefix("api/furniture")]
    [Authorize]
    public class FurnitureController : ApiController
    {
        private IFurnitureService _furnitureService;
        private IRoomService _roomService;

        public FurnitureController(
            IFurnitureService furnitureService,
            IRoomService roomService)
        {
            this._furnitureService = furnitureService;
            this._roomService = roomService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadFurniture")]
        [Route("get_furnitures")]
        [HttpGet]
        public HttpResponseMessage GetFurnitures(HttpRequestMessage requestMessage)
        {
            var furnitures = _furnitureService.GetFurnitures();
            var furnituresVM = Mapper.Map<IQueryable<Furniture>, List<FurnitureVM>>(furnitures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, furnituresVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadFurniture")]
        [Route("get_furnitures_by_roomid")]
        [HttpGet]
        public HttpResponseMessage GetFurnituresByRoomId(HttpRequestMessage requestMessage, int roomid)
        {
            var furnitures = _furnitureService.GetFurnituresByRoomId(roomid);
            var furnituresVM = Mapper.Map<IQueryable<Furniture>, List<FurnitureVM>>(furnitures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, furnituresVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadFurniture")]
        [Route("get_furnitures_by_roomcode")]
        [HttpGet]
        public HttpResponseMessage GetFurnituresByRoomCode(HttpRequestMessage requestMessage, string roomcode)
        {
            var furnitures = _furnitureService.GetFurnituresByRoomCode(roomcode);
            var furnituresVM = Mapper.Map<IQueryable<Furniture>, List<FurnitureVM>>(furnitures);

            return requestMessage.CreateResponse(HttpStatusCode.OK, furnituresVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadFurniture")]
        [Route("get_furniture_by_id")]
        [HttpGet]
        public HttpResponseMessage GetFurnitureById(HttpRequestMessage requestMessage, string id)
        {
            var furniture = _furnitureService.GetFurnitureById(id);
            if (furniture != null)
            {
                var furnitureVM = Mapper.Map<Furniture, FurnitureVM>(furniture);
                return requestMessage.CreateResponse(HttpStatusCode.OK, furnitureVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateFurniture")]
        [Route("create_furniture")]
        [HttpPost]
        public HttpResponseMessage CreateFurniture(HttpRequestMessage requestMessage, FurnitureVM furnitureVM)
        {
            if (ModelState.IsValid)
            {
                if (!_roomService.CheckRoomExistById(furnitureVM.RoomId))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                }


                furnitureVM.RepairDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(furnitureVM.RepairDate, "North Asia Standard Time").Date;
                var furniture = new Furniture();

                furniture.MapFurniture(furnitureVM);
                furniture.FurnitureId = Guid.NewGuid().ToString();
                furniture.CreatedBy = User.Identity.Name;
                furniture.CreatedDate = DateTime.Now;

                if (furniture.IsPaid)
                {
                    furniture.PaidDate = DateTime.Now;
                    furniture.PaidBy = User.Identity.Name;
                }

                _furnitureService.AddFurniture(furniture);
                _furnitureService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateFurniture")]
        [Route("update_furniture")]
        [HttpPut]
        public HttpResponseMessage UpdateFurniture(HttpRequestMessage requestMessage, FurnitureVM furnitureVM)
        {
            if (ModelState.IsValid)
            {
                var furniture = _furnitureService.GetFurnitureById(furnitureVM.FurnitureId);
                if (furniture != null)
                {
                    if (furniture.IsPaid)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán, không thể sửa đổi");
                    }

                    if (!_roomService.CheckRoomExistById(furnitureVM.RoomId))
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                    }

                    furniture.MapFurniture(furnitureVM);
                    furniture.UpdatedBy = User.Identity.Name;
                    furniture.UpdatedDate = DateTime.Now;

                    if (furniture.IsPaid)
                    {
                        furniture.PaidDate = DateTime.Now;
                        furniture.PaidBy = User.Identity.Name;
                    }

                    _furnitureService.UpdateFurniture(furniture);
                    _furnitureService.SaveChanges();

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

        // Xác nhận thanh toán
        [Authorize(Roles = "UpdateFurniture")]
        [Route("confirm_paid")]
        [HttpDelete]
        public HttpResponseMessage ComfirmPaid(HttpRequestMessage requestMessage, string id)
        {
            var furniture = _furnitureService.GetFurnitureById(id);
            if (furniture != null)
            {
                if (furniture.IsPaid)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán");
                }

                furniture.IsPaid = true;
                furniture.PaidDate = DateTime.Now;
                furniture.PaidBy = User.Identity.Name;

                _furnitureService.UpdateFurniture(furniture);
                _furnitureService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteFurniture")]
        [Route("delete_furniture")]
        [HttpDelete]
        public HttpResponseMessage DeleteFurniture(HttpRequestMessage requestMessage, string id)
        {
            var furniture = _furnitureService.GetFurnitureById(id);
            if (furniture != null)
            {
                _furnitureService.DeleteFurniture(id);
                _furnitureService.SaveChanges();

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
