using ExamManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json; // Thêm namespace này
namespace ExamManagement.Areas.AdminManagement.Controllers {

    public class DashboardController : BaseController
    {
        private readonly ExamManagementContext _context;

        public DashboardController(ExamManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sodethi = _context.Exams.ToArray();
            ViewBag.sodethi = sodethi.Length;
            var sobaidalam = _context.ExamResults.ToArray();
            ViewBag.sobaidalam = sobaidalam.Length;
            var sohocsinh = _context.Students.ToArray();
            ViewBag.sohocsinh = sohocsinh.Length;
            var sobaidat = _context.ExamResults.Where(s => s.Score >= 6).ToArray();
            ViewBag.sobaidat = sobaidat.Length;

            var examResults = await _context.ExamResults
                .Include(er => er.Exam)
                .ThenInclude(e => e.Subject)
                .Include(er => er.Student)
                .ToListAsync();

            // Dữ liệu cho biểu đồ diện tích
            var monthlyData = examResults
                .Where(er => er.SubmittedAt.HasValue)
                .GroupBy(er => new { er.SubmittedAt.Value.Year, er.SubmittedAt.Value.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    StudentCount = g.Count()
                })
                .OrderBy(x => DateTime.ParseExact(x.Month, "M/yyyy", CultureInfo.InvariantCulture))
                .ToList();

            ViewBag.ChartLabels = JsonSerializer.Serialize(monthlyData.Select(x => x.Month));
            ViewBag.ChartData = JsonSerializer.Serialize(monthlyData.Select(x => x.StudentCount));

            // Dữ liệu cho biểu đồ tròn (phần trăm điểm trung bình, khá, giỏi)
            var totalResults = examResults.Count;
            var trungbinh = examResults.Count(er => er.Score.HasValue && er.Score >= 5.0m && er.Score <= 6.9m);
            var kha = examResults.Count(er => er.Score.HasValue && er.Score >= 7.0m && er.Score <= 8.9m);
            var gioi = examResults.Count(er => er.Score.HasValue && er.Score >= 9.0m && er.Score <= 10.0m);

            var pieData = new[]
            {
                totalResults > 0 ? (trungbinh * 100.0 / totalResults) : 0,
                totalResults > 0 ? (kha * 100.0 / totalResults) : 0,
                totalResults > 0 ? (gioi * 100.0 / totalResults) : 0
            };

            ViewBag.PieLabels = JsonSerializer.Serialize(new[] { "Trung bình (5.0-6.9)", "Khá (7.0-8.9)", "Giỏi (9.0-10.0)" });
            ViewBag.PieData = JsonSerializer.Serialize(pieData);

            return View(examResults);
        }
    }

}
