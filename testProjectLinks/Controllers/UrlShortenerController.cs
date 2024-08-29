using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testProjectLinks.Data;
using testProjectLinks.Models;
public class UrlShortenerController : Controller
{
    private readonly AppDbContext _context;

    public UrlShortenerController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ShortenUrl(string originalUrl)
    {
        if (string.IsNullOrEmpty(originalUrl))
        {
            return BadRequest("URL is required.");
        }

        string shortUrlCode = GenerateShortUrlCode();

        var shortenedUrl = new Link
        {
            LinkStr = originalUrl,
            NewLinkStr = shortUrlCode
        };

        _context.ShortenedUrls.Add(shortenedUrl);
        await _context.SaveChangesAsync();

        return View("ShortenedUrlResult", shortenedUrl);
    }

    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var shortenedUrl = await _context.ShortenedUrls
            .FirstOrDefaultAsync(u => u.NewLinkStr == code);

        if (shortenedUrl == null)
        {
            return NotFound("URL not found.");
        }

        return Redirect(shortenedUrl.LinkStr);
    }

    private string GenerateShortUrlCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
