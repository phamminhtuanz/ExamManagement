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
    public class ExamsController : BaseController
    {
        private readonly ExamManagementContext _context;

        public ExamsController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/Exams
        public async Task<IActionResult> Index(string searchString)
        {
            // Bắt đầu với IQueryable để có thể áp dụng bộ lọc
            var examManagementContext = _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.CreatedByNavigation) // Bao gồm thông tin người tạo kỳ thi
                .AsQueryable(); // Đảm bảo có thể tiếp tục áp dụng bộ lọc

            // Nếu có từ khóa tìm kiếm, áp dụng điều kiện lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                examManagementContext = examManagementContext.Where(e =>
                    e.Title.Contains(searchString) ||
                    e.Subject.SubjectName.Contains(searchString));
            }

            // Thực thi truy vấn và trả về danh sách
            return View(await examManagementContext.ToListAsync());
        }



        // GET: AdminManagement/Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.CreatedByNavigation) // Bao gồm thông tin người tạo
                .FirstOrDefaultAsync(m => m.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }
            ViewBag.TenNguoiTao = exam;
            return View(exam);
        }

        // GET: AdminManagement/Exams/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "FullName");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: AdminManagement/Exams/Create
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

        // GET: AdminManagement/Exams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.CreatedByNavigation) // Bao gồm thông tin người tạo
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "FullName", exam.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", exam.SubjectId);
            return View(exam);
        }



        // POST: AdminManagement/Exams/Edit/5
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
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "AdminId", "FullName", exam.CreatedBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", exam.SubjectId);
            return View(exam);
        }

        // GET: AdminManagement/Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.CreatedByNavigation) // Bao gồm thông tin người tạo
                .FirstOrDefaultAsync(m => m.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }


        // POST: AdminManagement/Exams/Delete/5
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
