using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using testProjectLinks.Data;
using testProjectLinks.Models;

namespace testProjectLinks.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // «агружаем ссылки, созданные пользователем
            var userLinks = await _context.Link.Where(link => link.UserId == userId).ToListAsync();

            return View(userLinks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
