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

        // GET: Registrations/Create
        public IActionResult Create()
        {
            ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name"); // Use Activity Name
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name"); // Use Member Name
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,AktivityId,RegistrationDate")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                // Automatically set the creator's ID
                registration.MemberId = HttpContext.Session.GetInt32("MemberId");

                _context.Add(registration);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AktivityId"] = new SelectList(_context.Activities, "Id", "Name", registration.AktivityId); // Use Activity Name
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", registration.MemberId); // Use Member Name
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

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }
    }
}
