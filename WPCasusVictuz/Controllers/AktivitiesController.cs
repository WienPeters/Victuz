using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WPCasusVictuz.Data;
using WPCasusVictuz.Models;

namespace WPCasusVictuz.Controllers
{
    public class AktivitiesController : Controller
    {
        private readonly AppDBContext _context;

        public AktivitiesController(AppDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchTerm)
        {
            // Start met een query die alle activiteiten selecteert
            var activities = from a in _context.Activities
                             select a;

            // Als de zoekterm niet leeg is, filteren op naam
            if (!string.IsNullOrEmpty(searchTerm))
            {
                activities = activities.Where(a => a.Name.Contains(searchTerm)|| a.Description.Contains(searchTerm));
            }

            // Stuur de lijst van activiteiten naar de view
            return View(await activities.ToListAsync());
        }
        public async Task<IActionResult> UpcomActView()
        {
            // Fetch the activities along with the number of registrations
            var activities = await _context.Activities
                .Where(a => a.Date > DateTime.Now)
                .Include(a => a.Registrations)  // Include registrations to count them
                .ToListAsync();

            return View(activities);
        }
        public async Task<IActionResult> PastActView()
        {
            // Fetch the activities along with the number of registrations
            var activities = await _context.Activities
                .Where(a => a.Date < DateTime.Now)
                .Include(a => a.Registrations)  // Include registrations to count them
                .ToListAsync();

            return View(activities);
        }


        // GET: Aktivities/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var activity = await _context.Activities
                .Include(a => a.Registrations)
                .ThenInclude(r => r.Member)  // Ensure related members are loaded
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }
        
        // GET: Aktivities/Create
        public IActionResult Create()
        {
            // Check if the user is a board member before allowing access to the creation page
            if (HttpContext.Session.GetString("IsBoardMember") != "true")
            {
                TempData["Error"] = "Only board members can create activities.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Aktivities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,MaxParticipants,Description,Location,Category,CreatedbyBM")] Aktivity aktivity)
        {
            // Check if the user is a board member before processing the creation request
            if (HttpContext.Session.GetString("IsBoardMember") != "true")
            {
                TempData["Error"] = "You are not authorized to create activities.";
                return RedirectToAction("Index", "Home");
            }

            // Get the logged-in board member ID
            var boardMemberId = HttpContext.Session.GetInt32("BoardMemberId");

            // Check if the ModelState is valid
            if (ModelState.IsValid)
            {
                // Assign the logged-in board member as the organizer
                aktivity.CreatedbyBM = boardMemberId;

                // Add the new activity to the database
                _context.Add(aktivity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return the same view with the model
            return View(aktivity);
        }

        // GET: Aktivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivity = await _context.Activities.FindAsync(id);
            if (aktivity == null)
            {
                return NotFound();
            }
            return View(aktivity);
        }

        // POST: Aktivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,MaxParticipants,Description")] Aktivity aktivity)
        {
            if (id != aktivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aktivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AktivityExists(aktivity.Id))
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
            return View(aktivity);
        }

        // GET: Aktivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivity = await _context.Activities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aktivity == null)
            {
                return NotFound();
            }

            return View(aktivity);
        }

        // POST: Aktivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aktivity = await _context.Activities.FindAsync(id);
            if (aktivity != null)
            {
                _context.Activities.Remove(aktivity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AktivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
