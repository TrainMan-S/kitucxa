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
    [RoutePrefix("api/bill_electric")]
    [Authorize]
    public class BillElectricController : ApiController
    {
        private IBillElectricService _billElectricService;
        private IRoomService _roomService;

        public BillElectricController(
            IBillElectricService billElectricService,
            IRoomService roomService)
        {
            this._billElectricService = billElectricService;
            this._roomService = roomService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadBillElectric")]
        [Route("get_bill_electrics")]
        [HttpGet]
        public HttpResponseMessage GetBillElectrics(HttpRequestMessage requestMessage)
        {
            var billElectrics = _billElectricService.GetBillElectrics();
            var billElectricsVM = Mapper.Map<IQueryable<BillElectric>, List<BillElectricVM>>(billElectrics);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billElectricsVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadBillElectric")]
        [Route("get_bill_electrics_by_roomid")]
        [HttpGet]
        public HttpResponseMessage GetBillElectricsActive(HttpRequestMessage requestMessage, int roomid)
        {
            var billElectrics = _billElectricService.GetBillElectricsByRoomId(roomid);
            var billElectricsVM = Mapper.Map<IQueryable<BillElectric>, List<BillElectricVM>>(billElectrics);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billElectricsVM);
        }

        // Lấy danh sách
        [Authorize(Roles = "ReadBillElectric")]
        [Route("get_bill_electrics_by_roomcode")]
        [HttpGet]
        public HttpResponseMessage GetBillElectricsActive(HttpRequestMessage requestMessage, string roomcode)
        {
            var billElectrics = _billElectricService.GetBillElectricsByRoomCode(roomcode);
            var billElectricsVM = Mapper.Map<IQueryable<BillElectric>, List<BillElectricVM>>(billElectrics);

            return requestMessage.CreateResponse(HttpStatusCode.OK, billElectricsVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadBillElectric")]
        [Route("get_bill_electric_by_id")]
        [HttpGet]
        public HttpResponseMessage GetBillElectricById(HttpRequestMessage requestMessage, string id)
        {
            var billElectric = _billElectricService.GetBillElectricById(id);
            if (billElectric != null)
            {
                var billElectricVM = Mapper.Map<BillElectric, BillElectricVM>(billElectric);
                return requestMessage.CreateResponse(HttpStatusCode.OK, billElectricVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Tạo mới
        [Authorize(Roles = "CreateBillElectric")]
        [Route("create_bill_electric")]
        [HttpPost]
        public HttpResponseMessage CreateBillElectric(HttpRequestMessage requestMessage, BillElectricVM billElectricVM)
        {
            if (ModelState.IsValid)
            {
                if (!_roomService.CheckRoomExistById(billElectricVM.RoomId))
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                }

                var billElectric = new BillElectric();

                billElectric.MapBillElectric(billElectricVM);
                billElectric.CreatedBy = User.Identity.Name;
                billElectric.CreatedDate = DateTime.Now;

                if (billElectric.IsPaid)
                {
                    billElectric.PaidDate = DateTime.Now;
                    billElectric.PaidBy = User.Identity.Name;
                }

                _billElectricService.AddBillElectric(billElectric);
                _billElectricService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Authorize(Roles = "UpdateBillElectric")]
        [Route("update_bill_electric")]
        [HttpPut]
        public HttpResponseMessage UpdateBillElectric(HttpRequestMessage requestMessage, BillElectricVM billElectricVM)
        {
            if (ModelState.IsValid)
            {
                var billElectric = _billElectricService.GetBillElectricById(billElectricVM.BillElectricId);
                if (billElectric != null)
                {
                    if (billElectric.IsPaid)
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán, không thể sửa đổi");
                    }

                    if (!_roomService.CheckRoomExistById(billElectricVM.RoomId))
                    {
                        return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin phòng không hợp lệ");
                    }

                    billElectric.MapBillElectric(billElectricVM);
                    billElectric.UpdatedBy = User.Identity.Name;
                    billElectric.UpdatedDate = DateTime.Now;

                    if (billElectric.IsPaid)
                    {
                        billElectric.PaidDate = DateTime.Now;
                        billElectric.PaidBy = User.Identity.Name;
                    }

                    _billElectricService.UpdateBillElectric(billElectric);
                    _billElectricService.SaveChanges();

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
        [Authorize(Roles = "UpdateBillElectric")]
        [Route("confirm_paid")]
        [HttpDelete]
        public HttpResponseMessage ComfirmPaid(HttpRequestMessage requestMessage, string id)
        {
            var billElectric = _billElectricService.GetBillElectricById(id);
            if (billElectric != null)
            {
                if (billElectric.IsPaid)
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin đã được thanh toán");
                }

                billElectric.IsPaid = true;
                billElectric.PaidDate = DateTime.Now;
                billElectric.PaidBy = User.Identity.Name;

                _billElectricService.UpdateBillElectric(billElectric);
                _billElectricService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteBillElectric")]
        [Route("delete_bill_electric")]
        [HttpDelete]
        public HttpResponseMessage DeleteBillElectric(HttpRequestMessage requestMessage, string id)
        {
            var billElectric = _billElectricService.GetBillElectricById(id);
            if (billElectric != null)
            {
                _billElectricService.DeleteBillElectric(id);
                _billElectricService.SaveChanges();

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
