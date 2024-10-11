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
    public class RegistrationsController : Controller
    {
        private readonly AppDBContext _context;

        public RegistrationsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Registrations.Include(r => r.Aktivity).Include(r => r.Member);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.Aktivity)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }
        #region gg
        //// GET: Registrations/Create
        //public IActionResult Create()
        //{
        //    ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name"); // Use Activity Name
        //    ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name"); // Use Member Name
        //    return View();
        //}

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,MemberId,AktivityId,RegistrationDate")] Registration registration)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Automatically set the creator's ID
        //        registration.MemberId = HttpContext.Session.GetInt32("MemberId");

        //        _context.Add(registration);

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name", registration.AktivityId); // Use Activity Name
        //    ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", registration.MemberId); // Use Member Name
        //    return View(registration);
        //}
        #endregion gg
        // GET: Registrations/Create?activityId=5
        public async Task<IActionResult> Create(int? activityId)
        {
            if (activityId == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            // Controleer of de activiteit al vol is
            if (activity.CurrentParticipants >= activity.MaxParticipants)
            {
                TempData["Error"] = "Deze activiteit is al vol.";
                return RedirectToAction("Details", "Aktivities", new { id = activityId });
            }

            // Controleer of de gebruiker al is ingeschreven
            var userId = HttpContext.Session.GetInt32("MemberId");
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.MemberId == userId && r.AktivityId == activityId);

            if (existingRegistration != null)
            {
                TempData["Error"] = "Je bent al geregistreerd voor deze activiteit.";
                return RedirectToAction("Details", "Aktivities", new { id = activityId });
            }

            var registration = new Registration
            {
                AktivityId = activity.Id,
                MemberId = userId.Value
            };

            return View(registration);
        }


        // POST: Registrations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,MemberId")] Registration registration)
        {
            var activity = await _context.Activities.FindAsync(registration.AktivityId);
            var member = await _context.Members.FindAsync(registration.MemberId);

            if (activity == null || member == null)
            {
                return NotFound();
            }

            // Controleer of de activiteit al vol is
            if (activity.CurrentParticipants >= activity.MaxParticipants)
            {
                ModelState.AddModelError("", "Deze activiteit is al vol.");
                return View(registration);
            }

            // Controleer of de gebruiker al is geregistreerd
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.MemberId == registration.MemberId && r.AktivityId == registration.AktivityId);

            if (existingRegistration != null)
            {
                ModelState.AddModelError("", "Je bent al geregistreerd voor deze activiteit.");
                return View(registration);
            }

            if (ModelState.IsValid)
            {
                _context.Registrations.Add(registration);
                activity.CurrentParticipants += 1;

                // Controleer of de activiteit nu vol is
                if (activity.CurrentParticipants == activity.MaxParticipants)
                {
                    // Voeg logica toe voor het verzenden van een melding, bijvoorbeeld via e-mail
                    // await SendActivityFullNotification(activity);
                    TempData["Success"] = "Je bent succesvol geregistreerd! Deze activiteit is nu vol.";
                }
                else
                {
                    TempData["Success"] = "Je bent succesvol geregistreerd voor de activiteit.";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Aktivities", new { id = registration.AktivityId });
            }

            return View(registration);
        }
        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name", registration.AktivityId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", registration.MemberId);
            return View(registration);
        }





        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,AktivityId,RegistrationDate")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
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
            ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name", registration.AktivityId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", registration.MemberId);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.Aktivity)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            // Controleer of de huidige gebruiker de eigenaar van de registratie is of een bestuurslid
            var currentUserId = HttpContext.Session.GetInt32("MemberId");
            var isBoardMember = HttpContext.Session.GetString("IsBoardMember") == "true";

            if (registration.MemberId != currentUserId && !isBoardMember)
            {
                return Unauthorized();
            }

            return View(registration);
        }


        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(registration.AktivityId);
            if (activity != null)
            {
                activity.CurrentParticipants -= 1;
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Je bent succesvol uitgeschreven van de activiteit.";
            return RedirectToAction("Details", "Aktivities", new { id = registration.AktivityId });
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterForActivity(int? activityId)
        {
            // Check if the activityId is valid and not null
            if (activityId == 0)
            {
                ModelState.AddModelError("", "Invalid activity ID.");
                return RedirectToAction("Index");
            }

            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            // Check if the activity is full
            if (activity.CurrentParticipants >= activity.MaxParticipants)
            {
                TempData["Error"] = "This activity is full.";
                return RedirectToAction("Details", "Aktivities", new { id = activityId });
            }

            // Register the user
            var userId = GetCurrentMemberId();
            var registration = new Registration
            {
                MemberId = userId,
                AktivityId = activityId
            };

            _context.Registrations.Add(registration);
            activity.CurrentParticipants += 1;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Successfully registered for the activity.";
            return RedirectToAction("Details", "Aktivities", new { id = activityId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnregisterFromActivity(int? activityId)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.AktivityId == activityId && r.MemberId == GetCurrentMemberId());

            if (registration == null)
            {
                return NotFound();
            }

            // Verwijder de registratie
            _context.Registrations.Remove(registration);

            // Verminder het aantal deelnemers
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity != null)
            {
                activity.CurrentParticipants -= 1;
                _context.Activities.Update(activity);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Aktivities", new { id = activityId });
        }

        private int GetCurrentMemberId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // Handle the case when the user is not logged in
                throw new InvalidOperationException("User is not logged in");
            }

            return int.Parse(userIdString);
        }

    }
}

