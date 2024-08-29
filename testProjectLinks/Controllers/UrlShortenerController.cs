using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using testProjectLinks.Data;
using testProjectLinks.Models;
public class UrlShortenerController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UrlShortenerController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var urls = await _context.Link
            .Where(u => u.UserId == userId)
            .ToListAsync();

        return View(urls);
    }

    [HttpPost]
    public async Task<IActionResult> ShortenUrl(string LinkStr)
    {
        if (string.IsNullOrEmpty(LinkStr))
        {
            return BadRequest("URL поле не должно быть пустым");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string shortUrlCode = GenerateShortUrlCode();

        // Получаем полный URL сервера (например, https://localhost:7132)
        var request = HttpContext.Request;
        string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        string fullShortUrl = $"{baseUrl}/{shortUrlCode}";

        var shortenedUrl = new Link
        {
            LinkStr = LinkStr,
            NewLinkStr = shortUrlCode,  // Храним только код в базе данных
            UserId = userId
        };

        _context.Link.Add(shortenedUrl);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var shortenedUrl = await _context.Link.FirstOrDefaultAsync(u => u.NewLinkStr == code);

        if (shortenedUrl == null)
        {
            return NotFound("URL not found.");
        }
        shortenedUrl.LinkVisits++;
        await _context.SaveChangesAsync();
        string originalUrl = shortenedUrl.LinkStr;
        if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
        {
            originalUrl = "http://" + originalUrl;
        }

        return Redirect(originalUrl);
    }

    private string GenerateShortUrlCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
