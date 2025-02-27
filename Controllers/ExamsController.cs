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
    public class ExamsController : Controller
    {
        private readonly ExamManagementContext _context;

        public ExamsController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: Exams
        public async Task<IActionResult> Index(int? subjectId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Check if CustomerId exists in session
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "LoginC");
            }
            // Lấy danh sách môn học từ database để hiển thị trong dropdown
            ViewBag.Subjects = await _context.Subjects.ToListAsync();
            ViewBag.SelectedSubjectId = subjectId;

            var exams = _context.Exams
                .Include(e => e.CreatedByNavigation)
                .Include(e => e.Subject)
                .AsQueryable();

            // Lọc theo môn học nếu có chọn
            if (subjectId.HasValue)
            {
                exams = exams.Where(e => e.SubjectId == subjectId);
            }

            return View(await exams.ToListAsync());
        }
        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.CreatedByNavigation)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: Exams/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId");
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamId,SubjectId,Title,Duration,CreatedAt,CreatedBy")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId", exam.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", exam.SubjectId);
            return View(exam);
        }

        // GET: Exams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId", exam.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", exam.SubjectId);
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamId,SubjectId,Title,Duration,CreatedAt,CreatedBy")] Exam exam)
        {
            if (id != exam.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.ExamId))
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
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "AdminId", exam.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", exam.SubjectId);
            return View(exam);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.CreatedByNavigation)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.ExamId == id);
        }
    }
}
