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
    public class BoardMembersController : Controller
    {
        private readonly AppDBContext _context;

        public BoardMembersController(AppDBContext context)
        {
            _context = context;
        }

        // GET: BoardMembers
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.BoardMembers.Include(b => b.Member);
            return View(await appDBContext.ToListAsync());
        }

        // GET: BoardMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardMember = await _context.BoardMembers
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boardMember == null)
            {
                return NotFound();
            }

            return View(boardMember);
        }

        // GET: BoardMembers/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Name", "Name");
            return View();
        }

        // POST: BoardMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Password,MemberId")] BoardMember boardMember)
        {
            if (ModelState.IsValid)
            {
                
                _context.BoardMembers.Add(boardMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id", boardMember.MemberId);
            return View(boardMember);
        }

        // GET: BoardMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardMember = await _context.BoardMembers.FindAsync(id);
            if (boardMember == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id", boardMember.MemberId);
            return View(boardMember);
        }

        // POST: BoardMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,MemberId")] BoardMember boardMember)
        {
            if (id != boardMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boardMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardMemberExists(boardMember.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id", boardMember.MemberId);
            return View(boardMember);
        }

        // GET: BoardMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardMember = await _context.BoardMembers
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boardMember == null)
            {
                return NotFound();
            }

            return View(boardMember);
        }

        // POST: BoardMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boardMember = await _context.BoardMembers.FindAsync(id);
            if (boardMember != null)
            {
                _context.BoardMembers.Remove(boardMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardMemberExists(int id)
        {
            return _context.BoardMembers.Any(e => e.Id == id);
        }
    }
}
