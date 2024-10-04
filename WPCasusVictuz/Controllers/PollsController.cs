using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WPCasusVictuz.Data;
using WPCasusVictuz.Models;

namespace WPCasusVictuz.Controllers
{
    public class PollsController : Controller
    {
        private readonly AppDBContext _context;

        public PollsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Polls
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Polls.Include(p => p.CreatedBy);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Polls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // GET: Polls/Create
        public IActionResult Create()
        {
            // Check if the logged-in user is a board member
            if (HttpContext.Session.GetString("IsBoardMember") != "true")
            {
                return Unauthorized(); // Or redirect to an error page
            }

            ViewData["CreatedByBoardMemberId"] = new SelectList(_context.BoardMembers, "Id", "Name");
            return View();
        }

        // POST: Polls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Polls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Options")] Poll poll)
        {
            if (ModelState.IsValid)
            {
                

                // Automatically set the creator's ID
                poll.CreatedByBoardMemberId = HttpContext.Session.GetInt32("BoardMemberId");

                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: Polls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            ViewData["CreatedByBoardMemberId"] = new SelectList(_context.BoardMembers, "Id", "Name", poll.CreatedByBoardMemberId);
            return View(poll);
        }

        // POST: Polls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Options,CreatedByBoardMemberId")] Poll poll)
        {
            if (id != poll.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Id))
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
            ViewData["CreatedByBoardMemberId"] = new SelectList(_context.BoardMembers, "Id", "Name", poll.CreatedByBoardMemberId);
            return View(poll);
        }

        // GET: Polls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // POST: Polls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }
    }
}
