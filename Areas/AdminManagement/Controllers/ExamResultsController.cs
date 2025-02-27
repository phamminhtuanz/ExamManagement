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
    public class ExamResultsController : Controller
    {
        private readonly ExamManagementContext _context;

        public ExamResultsController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/ExamResults
        public async Task<IActionResult> Index()
        {
            var examManagementContext = _context.ExamResults.Include(e => e.Exam).Include(e => e.Student).Include(e => e.Exam);
            var examResults = _context.ExamResults
            .Include(er => er.Exam)
            .ThenInclude(e => e.Subject)
            .Include(er => er.Student)
            .ToList();

            return View(examResults); // ✅ Trả về List<ExamResult> đúng kiểu mà View mong đợi

        }

        // GET: AdminManagement/ExamResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examResult = await _context.ExamResults
                .Include(e => e.Exam)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.ResultId == id);
            if (examResult == null)
            {
                return NotFound();
            }

            return View(examResult);
        }

        // GET: AdminManagement/ExamResults/Create
        public IActionResult Create()
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: AdminManagement/ExamResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultId,StudentId,ExamId,Score,SubmittedAt")] ExamResult examResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", examResult.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", examResult.StudentId);
            return View(examResult);
        }

        // GET: AdminManagement/ExamResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examResult = await _context.ExamResults.FindAsync(id);
            if (examResult == null)
            {
                return NotFound();
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", examResult.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", examResult.StudentId);
            return View(examResult);
        }

        // POST: AdminManagement/ExamResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResultId,StudentId,ExamId,Score,SubmittedAt")] ExamResult examResult)
        {
            if (id != examResult.ResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamResultExists(examResult.ResultId))
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
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", examResult.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", examResult.StudentId);
            return View(examResult);
        }

        // GET: AdminManagement/ExamResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examResult = await _context.ExamResults
                .Include(e => e.Exam)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.ResultId == id);
            if (examResult == null)
            {
                return NotFound();
            }

            return View(examResult);
        }

        // POST: AdminManagement/ExamResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examResult = await _context.ExamResults.FindAsync(id);
            if (examResult != null)
            {
                _context.ExamResults.Remove(examResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamResultExists(int id)
        {
            return _context.ExamResults.Any(e => e.ResultId == id);
        }
    }
}
