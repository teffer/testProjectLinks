using Microsoft.AspNetCore.Mvc;
using testProjectLinks.Data;
using testProjectLinks.Models;
public class UrlShortenerController : Controller
{
    private readonly AppDbContext _context;

    public UrlShortenerController(AppDbContext context)
    {
        _context = context;
    }

    // Отображение формы для ввода ссылки
    public IActionResult Index()
    {
        return View();
    }

    // Обработка формы и генерация короткой ссылки
    [HttpPost]
    public async Task<IActionResult> ShortenUrl(string originalUrl)
    {
        if (string.IsNullOrEmpty(originalUrl))
        {
            return BadRequest("URL is required.");
        }

        // Генерация уникального кода
        string shortUrlCode = GenerateShortUrlCode();

        // Создание новой записи в БД
        var shortenedUrl = new Link
        {
            LinkStr = originalUrl,
            NewLinkStr = shortUrlCode
        };

        _context.ShortenedUrls.Add(shortenedUrl);
        await _context.SaveChangesAsync();

        // Отображение результата
        return View("ShortenedUrlResult", shortenedUrl);
    }

    // Перенаправление по короткой ссылке
    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var shortenedUrl = await _context.ShortenedUrls
            .FirstOrDefaultAsync(u => u.ShortUrl == code);

        if (shortenedUrl == null)
        {
            return NotFound("URL not found.");
        }

        return Redirect(shortenedUrl.OriginalUrl);
    }

    // Генерация случайного уникального кода
    private string GenerateShortUrlCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
