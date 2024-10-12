using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WPCasusVictuz.Models;

namespace WPCasusVictuz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Bylaw()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContactForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                // Hier kan je de logica toevoegen om het bericht te versturen, zoals een e-mail
                TempData["Success"] = "Je bericht is succesvol verstuurd!";
                return RedirectToAction("Contact");
            }

            return View("Contact", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
