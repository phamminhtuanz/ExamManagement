using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamManagement.Models;

namespace ExamManagement.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ExamManagementContext _context;

        public QuestionsController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: Questions
        public IActionResult Index(int eId, int sId, int examId)
        {
            HttpContext.Session.SetInt32("ExamSubjectId", examId);
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Kiểm tra nếu chưa đăng nhập
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "LoginC");
            }

            // Lấy danh sách câu hỏi dựa trên môn học (eId) và bao gồm cả đáp án
            var questions = _context.Questions
                .Where(q => q.SubjectId == eId)
                .Include(q => q.Answers)
                .ToList();

            // Random thứ tự câu hỏi
            questions = questions.OrderBy(q => Guid.NewGuid()).ToList();

            // Random thứ tự đáp án trong mỗi câu hỏi
            foreach (var question in questions)
            {
                question.Answers = question.Answers.OrderBy(a => Guid.NewGuid()).ToList();
            }

            // Truyền thời gian làm bài và các thông tin khác
            ViewBag.Duration = sId;          // sId là thời gian thi
            ViewBag.ExamId = examId;
            ViewBag.StudentId = customerId;

            return View(questions);
        }


        /*[HttpPost]
        public async Task<IActionResult> SaveExamResult([FromBody] ExamResult examResult)
        {
            var studentId = HttpContext.Session.GetInt32("CustomerId");
            var examId = HttpContext.Session.GetInt32("ExamSubjectId");

            if (studentId == null || examId == null)
            {
                return Json(new { success = false, message = "Lỗi: Không tìm thấy thông tin người dùng hoặc kỳ thi." });
            }

            var result = new ExamResult
            {
                StudentId = studentId,
                ExamId = examId,
                Score = examResult.Score,
                SubmittedAt = DateTime.Now
            };

            _context.ExamResults.Add(result);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }*/
        [HttpPost]
        public async Task<IActionResult> SaveExamResult([FromBody] ExamSubmissionModel examSubmission)
        {
            var studentId = HttpContext.Session.GetInt32("CustomerId");
            var examId = HttpContext.Session.GetInt32("ExamSubjectId");

            if (studentId == null || examId == null)
            {
                return Json(new { success = false, message = "Lỗi: Không tìm thấy thông tin người dùng hoặc kỳ thi." });
            }

            // Lưu vào bảng ExamResult trước
            var examResult = new ExamResult
            {
                StudentId = studentId,
                ExamId = examId,
                Score = examSubmission.Score,
                MaCauTL=examSubmission.MaCauTL,
                SubmittedAt = DateTime.Now
            };

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync(); // Lưu để có ResultId

            // Kiểm tra danh sách câu trả lời
            

            return Json(new { success = true });
        }

    }
}
