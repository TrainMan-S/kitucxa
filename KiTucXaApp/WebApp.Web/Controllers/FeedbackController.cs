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
    [RoutePrefix("api/feedback")]
    [Authorize]
    public class FeedbackController : ApiController
    {
        private IFeedbackService _feedbackService;
        private IAppUserService _appUserService;
        private IFeedbackAnswerService _feedbackAnswerService;

        public FeedbackController(
            IFeedbackService feedbackService,
            IAppUserService appUserService,
            IFeedbackAnswerService feedbackAnswerService)
        {
            this._feedbackService = feedbackService;
            this._appUserService = appUserService;
            this._feedbackAnswerService = feedbackAnswerService;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        #region < --- Dùng trong admin --->

        // Lấy danh sách
        [Authorize(Roles = "ReadFeedback")]
        [Route("get_feedbacks")]
        [HttpGet]
        public HttpResponseMessage GetFeedbacks(HttpRequestMessage requestMessage)
        {
            var feedbacks = _feedbackService.GetFeedbacks();
            var feedbacksVM = Mapper.Map<IQueryable<Feedback>, List<FeedbackVM>>(feedbacks);

            return requestMessage.CreateResponse(HttpStatusCode.OK, feedbacksVM);
        }

        // Lấy theo ID
        [Authorize(Roles = "ReadFeedback")]
        [Route("get_feedback_by_id")]
        [HttpGet]
        public HttpResponseMessage GetFeedbackById(HttpRequestMessage requestMessage, string id)
        {
            var feedback = _feedbackService.GetFeedbackById(id);
            if (feedback != null)
            {
                feedback.IsViewed = true;
                feedback.ViewedBy = User.Identity.Name;
                feedback.ViewedDate = DateTime.Now;
                _feedbackService.AddFeedback(feedback);
                _feedbackService.SaveChanges();

                var feedbackVM = Mapper.Map<Feedback, FeedbackVM>(feedback);

                var answers = _feedbackAnswerService.GetAnswersOfFeedback(id);
                feedbackVM.FeedbackAnswers = Mapper.Map<IQueryable<FeedbackAnswer>, List<FeedbackAnswerVM>>(answers);

                return requestMessage.CreateResponse(HttpStatusCode.OK, feedbackVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không tồn tại");
            }
        }

        // Trả lời feedback
        [Authorize(Roles = "AnswerFeedback")]
        [Route("answer_feedback")]
        [HttpPost]
        public HttpResponseMessage AnswerFeedback(HttpRequestMessage requestMessage, FeedbackAnswerVM feedbackAnswerVM)
        {
            if (ModelState.IsValid)
            {
                var feedback = _feedbackService.GetFeedbackById(feedbackAnswerVM.FeedbackAnswerId);
                if (feedback != null)
                {
                    var answer = new FeedbackAnswer();
                    answer.MapFeedbackAnswer(feedbackAnswerVM);
                    answer.FeedbackAnswerId = Guid.NewGuid().ToString();
                    answer.FeedbackId = feedback.FeedbackId;
                    answer.AnsweredDate = DateTime.Now;
                    answer.AnsweredBy = User.Identity.Name;

                    _feedbackAnswerService.AddFeedbackAnswer(answer);
                    _feedbackAnswerService.SaveChanges();

                    if (!feedback.IsAnswered)
                    {
                        feedback.IsAnswered = true;
                        _feedbackService.AddFeedback(feedback);
                        _feedbackService.SaveChanges();
                    }
                    
                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Trả lời thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin góp ý không hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Xóa
        [Authorize(Roles = "DeleteFeedback")]
        [Route("delete_feedback")]
        [HttpDelete]
        public HttpResponseMessage DeleteFeedback(HttpRequestMessage requestMessage, string id)
        {
            var feedback = _feedbackService.GetFeedbackById(id);
            if (feedback != null)
            {
                _feedbackService.DeleteFeedback(id);
                _feedbackService.SaveChanges();

                return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        #endregion


        #region < --- Dùng cho student --->

        // Lấy danh sách active
        [Route("student_get_feedbacks")]
        [HttpGet]
        public HttpResponseMessage GetFeedbacksOfStudent(HttpRequestMessage requestMessage)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null || student.IsActived && student.GroupId == 5)
            {
                var feedbacks = _feedbackService.GetFeedbacksOfStudent(student.Id);
                var feedbacksVM = Mapper.Map<IQueryable<Feedback>, List<FeedbackVM>>(feedbacks);

                return requestMessage.CreateResponse(HttpStatusCode.OK, feedbacksVM);
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Lấy theo ID
        [Route("student_get_feedback_by_id")]
        [HttpGet]
        public HttpResponseMessage StudentGetFeedbackById(HttpRequestMessage requestMessage, string id)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null || student.IsActived && student.GroupId == 5)
            {
                var feedback = _feedbackService.GetFeedbackById(id);
                if (feedback != null && feedback.StudentId == student.Id)
                {
                    var feedbackVM = Mapper.Map<Feedback, FeedbackVM>(feedback);

                    var answers = _feedbackAnswerService.GetAnswersOfFeedback(id);
                    feedbackVM.FeedbackAnswers = Mapper.Map<IQueryable<FeedbackAnswer>, List<FeedbackAnswerVM>>(answers);

                    return requestMessage.CreateResponse(HttpStatusCode.OK, feedbackVM);
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
        [Route("student_create_feedback")]
        [HttpPost]
        public HttpResponseMessage StudentCreateFeedback(HttpRequestMessage requestMessage, FeedbackVM feedbackVM)
        {
            if (ModelState.IsValid)
            {
                var student = _appUserService.GetUserByName(User.Identity.Name);
                if (student != null || student.IsActived && student.GroupId == 5)
                {
                    var feedback = new Feedback();

                    feedback.MapFeedback(feedbackVM);
                    feedback.FeedbackId = Guid.NewGuid().ToString();
                    feedback.CreatedDate = DateTime.Now;

                    feedback.StudentId = student.Id;
                    if (!feedback.IsIncognito)
                    {
                        feedback.StudentCode = student.UserName;
                    }

                    _feedbackService.AddFeedback(feedback);
                    _feedbackService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Thêm mới thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
                }   
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
            }
        }

        // Cập nhật
        [Route("student_update_feedback")]
        [HttpPut]
        public HttpResponseMessage StudentUpdateFeedback(HttpRequestMessage requestMessage, FeedbackVM feedbackVM)
        {
            if (ModelState.IsValid)
            {
                var student = _appUserService.GetUserByName(User.Identity.Name);
                if (student != null || student.IsActived && student.GroupId == 5)
                {
                    var feedback = _feedbackService.GetFeedbackById(feedbackVM.FeedbackId);
                    if (feedback != null && feedback.StudentId == student.Id)
                    {
                        if (feedback.IsViewed || feedback.IsAnswered)
                        {
                            return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Góp ý đã được xem hoặc xử lý, không thể cập nhật");
                        }

                        feedback.MapFeedback(feedbackVM);
                        feedback.UpdatedDate = DateTime.Now;

                        if (!feedback.IsIncognito)
                        {
                            feedback.StudentCode = student.UserName;
                        }

                        _feedbackService.UpdateFeedback(feedback);
                        _feedbackService.SaveChanges();

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
        [Route("student_delete_feedback")]
        [HttpDelete]
        public HttpResponseMessage StudentDeleteFeedback(HttpRequestMessage requestMessage, string id)
        {
            var student = _appUserService.GetUserByName(User.Identity.Name);
            if (student != null || student.IsActived && student.GroupId == 5)
            {
                var feedback = _feedbackService.GetFeedbackById(id);
                if (feedback != null && feedback.StudentId == student.Id)
                {
                    _feedbackService.DeleteFeedback(id);
                    _feedbackService.SaveChanges();

                    return requestMessage.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
                }
                else
                {
                    return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Thông tin không hợp lệ");
                }
            }
            else
            {
                return requestMessage.CreateResponse(HttpStatusCode.MethodNotAllowed, "Tài khoản không hợp lệ");
            }
                
        }

        #endregion

    }
}
