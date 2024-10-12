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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PictureController(AppDBContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Index()
    {
        var pictures = await _context.Pictures
            .Include(p => p.AddedByBoardMember)
            .ToListAsync();

        return View(pictures);
    }
    // GET: Upload Picture
    public IActionResult UploadPicture()
    {
        if (_httpContextAccessor.HttpContext.Session.GetString("IsBoardMember") != "true")
        {
            return RedirectToAction("Login", "Members"); // Redirect unauthorized users to the homepage
        }
        return View();
    }

    // POST: Upload Picture
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadPicture(IFormFile file)
    {
        //if (_httpContextAccessor.HttpContext.Session.GetString("IsBoardMember") != "true")
        //{
        //    return RedirectToAction("Index", "Home"); // Redirect unauthorized users to the homepage
        //}

        
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
        var isBoardMember = HttpContext.Session.GetInt32("IsBoardMember") ;

        var picture = new Picture
        {
            FilePath = "/uploads/" + uniqueFileName
        };

        
        picture.AddedByBoardMemberId = HttpContext.Session.GetInt32("BoardMemberId"); ;  // Assign Member as uploader
        

        // Save the Picture record to the database
        _context.Pictures.Add(picture);
        await _context.SaveChangesAsync();

        // Redirect to a confirmation or list view
        return RedirectToAction("Index", "Home");
    }
}
