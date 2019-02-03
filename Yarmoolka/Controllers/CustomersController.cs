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
    public class CustomersController : Controller
    {
        private readonly YarmoolkaContext _context;

        public CustomersController(YarmoolkaContext context)
        {
            _context = context;
        }

        // GET: Customers
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,ID,Name,Age,City,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        [AllowAnonymous]
        public IActionResult Search(string CustomerId, string Name = null, int? Age = null, string City = null, string PhoneNumber = null)
        {
            return View(this.GetCustomersBySearchParams(CustomerId, Name, Age, City, PhoneNumber));
        }
        [AllowAnonymous]
        public List<Customer> GetCustomersBySearchParams(string p_CustomerId,string p_Name = null, int? p_Age = null, string p_City = null, string p_PhoneNumber = null)
        {

            var queryOver = this._context.Customer.AsQueryable();

            if (!string.IsNullOrEmpty(p_CustomerId))
            {
                queryOver = queryOver.Where(x => x.CustomerId == (p_CustomerId));
            }
            if (!string.IsNullOrEmpty(p_Name))
            {
                queryOver = queryOver.Where(x => x.Name == p_Name);
            }
            if (p_Age.HasValue)
            {
                queryOver = queryOver.Where(x => x.Age == p_Age.Value);
            }
            if (!string.IsNullOrEmpty(p_City))
            {
                queryOver = queryOver.Where(x => x.City == p_City);
            }
            if (!string.IsNullOrEmpty(p_PhoneNumber))
            {
                queryOver = queryOver.Where(x => x.PhoneNumber == p_PhoneNumber);
            }
            
            var result = queryOver.Select(x => new Customer { ID = x.ID,  CustomerId= x.CustomerId, Name = x.Name, City = x.City, Age = x.Age, PhoneNumber = x.PhoneNumber }).ToList();         

            return result;

        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,ID,Name,Age,City,PhoneNumber")] Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customer.Any(e => e.ID == id);
        }
    }
}
