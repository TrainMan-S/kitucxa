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
    [RoutePrefix("api/bill_water")]
    [Authorize]
    public class BillWaterController : ApiController
    {
        private IBillWaterService _billWaterService;
        private IRoomService _roomService;

        public BillWaterController(
            IBillWaterService billWaterService,
            IRoomService roomService)
        {
            this._billWaterService = billWaterService;
            this._roomService = roomService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadBillWater")]
        [Route("get_bill_waters")]
        [HttpGet]
        public HttpResponseMessage GetBillWaters(HttpRequestMessage requestMessage)
        {
            var billWaters = _billWaterService.GetBillWaters();
            var billWatersVM = Mapper.Map<IQueryable<BillWater>, List<BillWaterVM>>(billWaters);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billWatersVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadBillWater")]
        [Route("get_bill_waters_by_roomid")]
        [HttpGet]
        public HttpResponseMessage GetBillWatersActive(HttpRequestMessage requestMessage, int roomid)
        {
            var billWaters = _billWaterService.GetBillWatersByRoomId(roomid);
            var billWatersVM = Mapper.Map<IQueryable<BillWater>, List<BillWaterVM>>(billWaters);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billWatersVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadBillWater")]
        [Route("get_bill_waters_by_roomcode")]
        [HttpGet]
        public HttpResponseMessage GetBillWatersActive(HttpRequestMessage requestMessage, string roomcode)
        {
            var billWaters = _billWaterService.GetBillWatersByRoomCode(roomcode);
            var billWatersVM = Mapper.Map<IQueryable<BillWater>, List<BillWaterVM>>(billWaters);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billWatersVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadBillWater")]
        [Route("get_bill_water_by_id")]
        [HttpGet]
        public HttpResponseMessage GetBillWaterById(HttpRequestMessage requestMessage, string id)
        {
            var billWater = _billWaterService.GetBillWaterById(id);
            if (billWater != null)
            {
                var billWaterVM = Mapper.Map<BillWater, BillWaterVM>(billWater);
                return requestMessage.CreateResponse(HttpStatusCode.OK, billWaterVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateBillWater")]
        [Route("create_bill_water")]
        [HttpPost]
        public HttpResponseMessage CreateBillWater(HttpRequestMessage requestMessage, BillWaterVM billWaterVM)
        {
            if (ModelState.IsValid)
            {
                if (!_roomService.CheckRoomExistById(billWaterVM.RoomId))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                }

                var billWater = new BillWater();

                billWater.MapBillWater(billWaterVM);
                billWater.CreatedBy = User.Identity.Name;
                billWater.CreatedDate = DateTime.Now;

                if (billWater.IsPaid)
                {
                    billWater.PaidDate = DateTime.Now;
                    billWater.PaidBy = User.Identity.Name;
                }

                _billWaterService.AddBillWater(billWater);
                _billWaterService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateBillWater")]
        [Route("update_bill_water")]
        [HttpPut]
        public HttpResponseMessage UpdateBillWater(HttpRequestMessage requestMessage, BillWaterVM billWaterVM)
        {
            if (ModelState.IsValid)
            {
                var billWater = _billWaterService.GetBillWaterById(billWaterVM.BillWaterId);
                if (billWater != null)
                {
                    if (billWater.IsPaid)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán, không thể sửa đổi");
                    }

                    if (!_roomService.CheckRoomExistById(billWaterVM.RoomId))
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                    }

                    billWater.MapBillWater(billWaterVM);
                    billWater.UpdatedBy = User.Identity.Name;
                    billWater.UpdatedDate = DateTime.Now;

                    if (billWater.IsPaid)
                    {
                        billWater.PaidDate = DateTime.Now;
                        billWater.PaidBy = User.Identity.Name;
                    }

                    _billWaterService.UpdateBillWater(billWater);
                    _billWaterService.SaveChanges();

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
        [Authorize(Roles = "UpdateBillWater")]
        [Route("confirm_paid")]
        [HttpDelete]
        public HttpResponseMessage ComfirmPaid(HttpRequestMessage requestMessage, string id)
        {
            var billWater = _billWaterService.GetBillWaterById(id);
            if (billWater != null)
            {
                if (billWater.IsPaid)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán");
                }

                billWater.IsPaid = true;
                billWater.PaidDate = DateTime.Now;
                billWater.PaidBy = User.Identity.Name;

                _billWaterService.UpdateBillWater(billWater);
                _billWaterService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteBillWater")]
        [Route("delete_bill_water")]
        [HttpDelete]
        public HttpResponseMessage DeleteBillWater(HttpRequestMessage requestMessage, string id)
        {
            var billWater = _billWaterService.GetBillWaterById(id);
            if (billWater != null)
            {
                _billWaterService.DeleteBillWater(id);
                _billWaterService.SaveChanges();

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
