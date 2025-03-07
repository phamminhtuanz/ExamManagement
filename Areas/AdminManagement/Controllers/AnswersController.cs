using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamManagement.Models;

namespace ExamManagement.Areas.AdminManagement.Controllers
{
    [Area("AdminManagement")]
    public class AnswersController : Controller
    {
        private readonly ExamManagementContext _context;

        public AnswersController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/Answers
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 10; // Số bản ghi trên mỗi trang
            var examManagementContext = _context.Answers
                .Include(a => a.Question)
                .Include(a => a.Question.Subject)
                .AsQueryable(); // Đảm bảo có thể áp dụng bộ lọc

            // Nếu có từ khóa tìm kiếm, áp dụng điều kiện lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                examManagementContext = examManagementContext.Where(a =>
                    a.Content.Contains(searchString) ||
                    a.Question.Content.Contains(searchString) ||
                    a.Question.Subject.SubjectName.Contains(searchString));
            }

            // Tổng số bản ghi sau khi lọc (nếu có)
            int totalRecords = await examManagementContext.CountAsync();

            // Lấy dữ liệu cho trang hiện tại
            var paginatedAnswers = await examManagementContext
                .OrderBy(a => a.AnswerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi dữ liệu phân trang qua ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchString = searchString;

            return View(paginatedAnswers);
        }



        // GET: AdminManagement/Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: AdminManagement/Answers/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Content");
            return View();
        }

        // POST: AdminManagement/Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnswerId,QuestionId,Content,IsCorrect")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Content", answer.QuestionId);
            return View(answer);
        }

        // GET: AdminManagement/Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Content", answer.QuestionId);
            return View(answer);
        }

        // POST: AdminManagement/Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnswerId,QuestionId,Content,IsCorrect")] Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.AnswerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Content", answer.QuestionId);
            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy câu trả lời!" });
            }

            try
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Gửi lỗi chi tiết về cho Ajax để xem cụ thể
                return Json(new { success = false, message = "Lỗi chi tiết: " + ex.Message });
            }
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.AnswerId == id);
        }
    }
}
