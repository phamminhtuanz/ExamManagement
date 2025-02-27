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
    public class QuestionsController : Controller
    {
        private readonly ExamManagementContext _context;

        public QuestionsController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/Questions
        public async Task<IActionResult> Index(string searchString)
        {
            var examManagementContext = _context.Questions
                .Include(q => q.CreatedByNavigation)
                .Include(q => q.Subject)
                .Include(q => q.Answers) // Bao gồm cả danh sách câu trả lời
                .AsQueryable(); // Đảm bảo có thể tiếp tục áp dụng bộ lọc

            // Nếu có từ khóa tìm kiếm, áp dụng điều kiện lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                examManagementContext = examManagementContext.Where(q =>
                    q.Content.Contains(searchString) ||
                    q.Subject.SubjectName.Contains(searchString));
            }

            return View(await examManagementContext.ToListAsync());
        }

        // GET: AdminManagement/Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CreatedByNavigation)
                .Include(q => q.Subject)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: AdminManagement/Questions/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: AdminManagement/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question model, List<string> Answers, int CorrectAnswerIndex)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(model);
                await _context.SaveChangesAsync(); // Lưu câu hỏi trước để có QuestionId

                // Lưu danh sách đáp án
                for (int i = 0; i < Answers.Count; i++)
                {
                    Answer answer = new Answer
                    {
                        QuestionId = model.QuestionId, // Lấy ID câu hỏi vừa lưu
                        Content = Answers[i],
                        IsCorrect = (i == CorrectAnswerIndex) // Xác định đáp án đúng
                    };
                    _context.Answers.Add(answer);
                }

                await _context.SaveChangesAsync(); // Lưu danh sách đáp án

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        // GET: AdminManagement/Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "FullName", question.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", question.SubjectId);
            return View(question);
        }

        // POST: AdminManagement/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,SubjectId,Content,Level,CreatedBy")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
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
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId", question.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", question.SubjectId);
            return View(question);
        }

        // GET: AdminManagement/Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CreatedByNavigation)
                .Include(q => q.Subject)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: AdminManagement/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
