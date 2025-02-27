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
    public class ExamAnswersController : Controller
    {
        private readonly ExamManagementContext _context;

        public ExamAnswersController(ExamManagementContext context)
        {
            _context = context;
        }

        // GET: ExamAnswers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExamAnswers.ToListAsync());
        }

        // GET: ExamAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examAnswers = await _context.ExamAnswers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examAnswers == null)
            {
                return NotFound();
            }

            return View(examAnswers);
        }

        // GET: ExamAnswers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExamAnswers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExamId,StudentId,Answer,SubmittedAt")] ExamAnswers examAnswers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examAnswers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(examAnswers);
        }

        // GET: ExamAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examAnswers = await _context.ExamAnswers.FindAsync(id);
            if (examAnswers == null)
            {
                return NotFound();
            }
            return View(examAnswers);
        }

        // POST: ExamAnswers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExamId,StudentId,Answer,SubmittedAt")] ExamAnswers examAnswers)
        {
            if (id != examAnswers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examAnswers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamAnswersExists(examAnswers.Id))
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
            return View(examAnswers);
        }

        // GET: ExamAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examAnswers = await _context.ExamAnswers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examAnswers == null)
            {
                return NotFound();
            }

            return View(examAnswers);
        }

        // POST: ExamAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examAnswers = await _context.ExamAnswers.FindAsync(id);
            if (examAnswers != null)
            {
                _context.ExamAnswers.Remove(examAnswers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamAnswersExists(int id)
        {
            return _context.ExamAnswers.Any(e => e.Id == id);
        }
    }
}
