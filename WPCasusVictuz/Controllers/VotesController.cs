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
    public class VotesController : Controller
    {
        private readonly AppDBContext _context;

        public VotesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Votes
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Votes.Include(v => v.Member).Include(v => v.Poll);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Member)
                .Include(v => v.Poll)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name");
            ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question");
            return View();
        }

        // POST: Votes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PollId,MemberId,SelectedOption")] Vote vote)
        {
            // Controleer of de gebruiker en poll bestaan
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == vote.PollId);
            var memberId = HttpContext.Session.GetInt32("MemberId");

            if (poll == null || memberId == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid poll or user.");
                ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", vote.MemberId);
                ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question", vote.PollId);
                return View(vote);
            }

            // Controleer of de optie geldig is
            if (!poll.Options.Contains(vote.SelectedOption))
            {
                ModelState.AddModelError("SelectedOption", "Invalid option.");
                ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", vote.MemberId);
                ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question", vote.PollId);
                return View(vote);
            }

            // Controleer of de gebruiker al gestemd heeft op deze poll
            bool hasVoted = await _context.Votes.AnyAsync(v => v.MemberId == memberId && v.PollId == vote.PollId);
            if (hasVoted)
            {
                ModelState.AddModelError("", "Je hebt al gestemd op deze poll.");
                ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", vote.MemberId);
                ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question", vote.PollId);
                return View(vote);
            }

            // Indien geldig, sla de stem op
            vote.MemberId = memberId;
            _context.Add(vote);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // This action returns poll options in JSON format for the selected poll
        public IActionResult GetPollOptions(int pollId)
        {
            var poll = _context.Polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null)
            {
                return Json(new List<string>());  // Return an empty list if no poll found
            }

            return Json(poll.Options);  // Return the poll options
        }
        // GET: Votes/Create
        public IActionResult CreateIdea()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name");
            //only the member is required for a idea suggestion
            return View();
        }

        // POST: Votes/CreateIdea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIdea([Bind("Id,SelectedOption")] Vote vote)
        {
            // Markeer het als een suggestie
            vote.IsSuggestion = true;
            vote.PollId = null; // Geen poll gekoppeld aan een suggestie
            vote.MemberId = HttpContext.Session.GetInt32("MemberId").GetValueOrDefault();

            _context.Add(vote);
            await _context.SaveChangesAsync();

            // Redirect naar een bedankpagina voor suggesties
            return RedirectToAction(nameof(ThankYou));
        }

        // Bedankpagina voor suggesties
        public IActionResult ThankYou()
        {
            return View();
        }

        public async Task<IActionResult> Suggestions()
        {
            // Check of de gebruiker een bestuurslid is
            var isBoardMember = HttpContext.Session.GetString("IsBoardMember") == "true";
            if (!isBoardMember)
            {
                return Unauthorized();
            }

            // Haal alleen suggesties op
            var suggestions = await _context.Votes
                .Include(v => v.Member)
                .Where(v => v.IsSuggestion)
                .ToListAsync();

            return View(suggestions);
        }



        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", vote.MemberId);
            ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question", vote.PollId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PollId,MemberId,SelectedOption")] Vote vote)
        {
            if (id != vote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", vote.MemberId);
            ViewData["PollId"] = new SelectList(_context.Polls, "Id", "Question", vote.PollId);
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Member)
                .Include(v => v.Poll)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote != null)
            {
                _context.Votes.Remove(vote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.Id == id);
        }
    }
}
