using ExamManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View();
        }
    }

}
