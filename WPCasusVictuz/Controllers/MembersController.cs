using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WPCasusVictuz.Data;
using WPCasusVictuz.Models;
using Microsoft.AspNetCore.Identity;

namespace WPCasusVictuz.Controllers
{
    public class MembersController : Controller
    {
        private readonly AppDBContext db;
        

        public MembersController(AppDBContext context)
        {
            db = context;
            
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await db.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await db.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Status")] Member member)
        {
            if (ModelState.IsValid)
            {
                // Optional: Hash the password before saving (using a basic hash mechanism or a library)
                //member.Password = HashPassword(member.Password); // Replace with actual hashing logic
                member.Status = "active";

                db.Members.Add(member);
                await db.SaveChangesAsync();
                HttpContext.Session.SetInt32("UserId", member.Id);
                HttpContext.Session.SetString("UserName", member.Name);
                HttpContext.Session.SetInt32("MemberId", member.Id);

                // Redirect to home or any other page
                return RedirectToAction("Index", "Home");
            }
            return View(member);
        }
        // GET: Members/Create
        public IActionResult CreateBoardMember()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBoardMember([Bind("Name, Password, MemberId")] BoardMember bm)
        {
            if (ModelState.IsValid)
            {
                // Optional: Hash the password before saving
                // Gebruik een daadwerkelijke hash functie

                // Opslaan in de database
                db.BoardMembers.Add(bm);
                await db.SaveChangesAsync();

                // Optioneel: Stel sessievariabelen in of andere logica
                HttpContext.Session.SetInt32("BoardMemberId", bm.Id);
                HttpContext.Session.SetString("UserName", bm.Name);

                // Redirect naar de index of een andere pagina
                return RedirectToAction("Index");
            }

            // Als er een fout is, herlaad de MemberId lijst en toon het formulier opnieuw
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", bm.MemberId);
            return View(bm);
        }
        // GET: Members/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Members/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before checking
                //var hashedPassword = HashPassword(password);

                // Check if a member with this name and hashed password exists
                var member = await db.Members
                    .Include(m => m.BoardMember) // Include BoardMember info
                    .FirstOrDefaultAsync(m => m.Name == name && m.Password == password);

                if (member != null)
                {
                    // Set the session values
                    HttpContext.Session.SetString("UserName", member.Name);
                    HttpContext.Session.SetString("UserId", member.Id.ToString());
                    HttpContext.Session.SetInt32("MemberId", member.Id);

                    // Check if the member is a board member and store it in session
                    if (member.BoardMember != null)
                    {
                        HttpContext.Session.SetString("IsBoardMember", "true");
                        HttpContext.Session.SetInt32("BoardMemberId", member.BoardMember.Id);
                    }
                    else
                    {
                        HttpContext.Session.SetString("IsBoardMember", "false");
                    }

                    // Redirect to home or another page
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View();
        }

        private string HashPassword(string password)
        {
            // Use SHA256 to hash the password
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Logout action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Members");
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Status")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(member);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await db.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await db.Members.FindAsync(id);
            if (member != null)
            {
                db.Members.Remove(member);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return db.Members.Any(e => e.Id == id);
        }
    }
}
