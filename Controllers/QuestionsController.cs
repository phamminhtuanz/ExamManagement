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

            // Check if CustomerId exists in session
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "LoginC");
            }

            // Get questions based on subject (eId) and include answers
            var questions = _context.Questions
                .Where(q => q.SubjectId == eId)
                .Include(q => q.Answers)
                .ToList();

            // Pass the duration for exam timing
            ViewBag.Duration = sId; // sId is the exam duration
            ViewBag.ExamId = examId;
            ViewBag.StudentId = customerId;// sId is the exam duration

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
                SubmittedAt = DateTime.Now
            };

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync(); // Lưu để có ResultId

            // Kiểm tra danh sách câu trả lời
            

            return Json(new { success = true });
        }

    }
}
