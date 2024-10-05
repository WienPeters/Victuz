using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using WPCasusVictuz.Data;
using WPCasusVictuz.Models;

public class PictureController : Controller
{
    private readonly AppDBContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PictureController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var pictures = await _context.Pictures
            .Include(p => p.AddedByMember)
            .Include(p => p.AddedByBoardMember)
            .ToListAsync();

        return View(pictures);
    }
    // GET: Upload Picture
    public IActionResult UploadPicture()
    {
        return View();
    }

    // POST: Upload Picture
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadPicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please select a valid picture.");
            return View();
        }

        // Generate a unique file name
        string uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

        // Define the path where the file will be saved
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        // Ensure the directory exists
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        // Full path to save the file
        string filePath = Path.Combine(uploadPath, uniqueFileName);

        // Save the file to the server
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Determine whether it's a Member or BoardMember
        var userId = HttpContext.Session.GetInt32("UserId");
        var isBoardMember = HttpContext.Session.GetInt32("IsBoardMember") == 1;

        var picture = new Picture
        {
            FilePath = "/uploads/" + uniqueFileName
        };

        if (isBoardMember)
        {
            picture.AddedByBoardMemberId = userId;  // Assign BoardMember as uploader
        }
        else
        {
            picture.AddedByMemberId = userId;  // Assign Member as uploader
        }

        // Save the Picture record to the database
        _context.Pictures.Add(picture);
        await _context.SaveChangesAsync();

        // Redirect to a confirmation or list view
        return RedirectToAction("Index", "Home");
    }
}
