using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yarmoolka.Models;

namespace Yarmoolka.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class StoreBranchesController : Controller
    {
        private readonly YarmoolkaContext _context;

        public StoreBranchesController(YarmoolkaContext context)
        {
            _context = context;
        }

        // GET: StoreBranches
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoreBranch.ToListAsync());
        }

        // GET: StoreBranches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeBranch = await _context.StoreBranch
                .SingleOrDefaultAsync(m => m.ID == id);
            if (storeBranch == null)
            {
                return NotFound();
            }

            return View(storeBranch);
        }

        // GET: StoreBranches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoreBranches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BrnachName,OpeningHours,Longitude,Latitude")] StoreBranch storeBranch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storeBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storeBranch);
        }

        // GET: StoreBranches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeBranch = await _context.StoreBranch.SingleOrDefaultAsync(m => m.ID == id);
            if (storeBranch == null)
            {
                return NotFound();
            }
            return View(storeBranch);
        }

        // POST: StoreBranches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BrnachName,OpeningHours,Longitude,Latitude")] StoreBranch storeBranch)
        {
            if (id != storeBranch.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreBranchExists(storeBranch.ID))
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
            return View(storeBranch);
        }

        // GET: StoreBranches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeBranch = await _context.StoreBranch
                .SingleOrDefaultAsync(m => m.ID == id);
            if (storeBranch == null)
            {
                return NotFound();
            }

            return View(storeBranch);
        }

        // POST: StoreBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeBranch = await _context.StoreBranch.SingleOrDefaultAsync(m => m.ID == id);
            _context.StoreBranch.Remove(storeBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreBranchExists(int id)
        {
            return _context.StoreBranch.Any(e => e.ID == id);
        }

        [AllowAnonymous]
        public IActionResult MapView()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetAllBranches()
        {
            return Json(_context.StoreBranch.ToList());

        }

    }
    }
