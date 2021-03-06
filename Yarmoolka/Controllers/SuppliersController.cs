﻿using System;
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
    public class SuppliersController : Controller
    {
        private readonly YarmoolkaContext _context;

        public SuppliersController(YarmoolkaContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Supplier.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,PhoneNumber,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,PhoneNumber,Address")] Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.ID))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.ID == id);
        }
        public IActionResult Search(int? supplierId = null, string Name = null, string Address = null, string PhoneNumber = null)
        {            
            return View(this.GetSuppliersBySearchParams(supplierId,Name,PhoneNumber,Address));
        }
        public List<Supplier> GetSuppliersBySearchParams(int? p_supplierId = null, string p_supplierName = null, string p_phoneNumber = null, string p_address = null)
        {
            var queryOver = this._context.Supplier.AsQueryable();

            if (!string.IsNullOrEmpty(p_supplierName))
            {
                queryOver = queryOver.Where(x => x.Name == (p_supplierName));
            }
            if (p_supplierId.HasValue)
            {
                queryOver = queryOver.Where(x => x.ID == p_supplierId.Value);
            }
            if (!string.IsNullOrEmpty(p_address))
            {
                queryOver = queryOver.Where(x => x.Address == p_address);
            }
            if (!string.IsNullOrEmpty(p_phoneNumber))
            {
                queryOver = queryOver.Where(x => x.PhoneNumber == p_phoneNumber);
            }
            var result = queryOver.Select(x => new Supplier { ID = x.ID, Name = x.Name, PhoneNumber = x.PhoneNumber, Address = x.Address}).ToList();

            // return this._jsonSerializer.Serialize(result); // Run the query and avoid context dispose
            return result;

        }
    }
}
