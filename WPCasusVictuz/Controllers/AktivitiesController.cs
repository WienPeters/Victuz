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

        // GET: Aktivities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activities.ToListAsync());
        }

        // GET: Aktivities/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Aktivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aktivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,MaxParticipants,Description")] Aktivity aktivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aktivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
