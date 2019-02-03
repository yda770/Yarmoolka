using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Yarmoolka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using IMDBCore;

namespace Yarmoolka.Controllers

{
    public class YarmoolkasController : Controller
    {
        private readonly YarmoolkaContext _context;
        private readonly IConfiguration _configuration;
        
        public YarmoolkasController(YarmoolkaContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }

        public List<Models.YarmoolkaClass> GetYarmoolkasByStyle(string p_Style)
        {
            var Yarmoolkas = this._context.Yarmoolka.Where(x => x.Style == p_Style).ToList();
            return Yarmoolkas;
        }

        public List<Models.YarmoolkaClass> GetYarmoolkasByCompany(string p_Company)
        {
            var Yarmoolkas = this._context.Yarmoolka.Where(x => x.Company == p_Company).ToList();
            return Yarmoolkas;
        }
        public List<Models.YarmoolkaClass> GetYarmoolkasByReleaseYear(int p_releaseYear)
        {
            var Yarmoolkas = this._context.Yarmoolka.Where(x => x.ModelDate.Year == p_releaseYear).ToList();
            return Yarmoolkas;
        }
        // GET: Yarmoolkas
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Yarmoolka.Include(x=>x.YarmoolkaSupplier).ToListAsync());
        }
        public IActionResult Search(string title = null, int? year = null, string genere = null, string Company = null,int? priceFrom=null,int? priceTo=null,int? SupplierId = null)
        {
            PopulateSuppliersDropDownList();
            return View(this.GetYarmoolkasBySearchParams(title, year, genere, Company,priceFrom,priceTo, SupplierId));
        }
        public List<Models.YarmoolkaClass> GetYarmoolkasBySearchParams(string p_YarmoolkaTitle = null, int? p_releaseYear = null, string p_genere = null, string p_Company = null ,int? p_priceFrom=null, int? p_priceTo = null,int? p_supplierId=null)
        {
            var queryOver = this._context.Yarmoolka.AsQueryable();

            if (!string.IsNullOrEmpty(p_YarmoolkaTitle))
            {
                queryOver = queryOver.Where(x => x.Name == (p_YarmoolkaTitle));
            }
            if (p_releaseYear.HasValue)
            {
                queryOver = queryOver.Where(x => x.ModelDate.Year == p_releaseYear.Value);
            }
            if (!string.IsNullOrEmpty(p_genere))
            {
                queryOver = queryOver.Where(x => x.Style == p_genere);
            }
            if (!string.IsNullOrEmpty(p_Company))
            {
                queryOver = queryOver.Where(x => x.Company == p_Company);
            }
            if (p_priceFrom.HasValue)
            {
                queryOver = queryOver.Where(x => x.Price >= p_priceFrom);
            }
            if (p_priceTo.HasValue)
            {
                queryOver = queryOver.Where(x => x.Price <= p_priceTo);
            }
            if (p_supplierId.HasValue)
            {
                // join query with suppliers              
                queryOver = queryOver.Join(this._context.Supplier,
                     m => m.SupplierId,
                     s => s.ID,
                 (m, s) => new YarmoolkaClass { ID = m.ID, Name = m.Name, ModelDate = m.ModelDate, Style = m.Style, Price = m.Price, Company = m.Company, Size = m.Size, Color = m.Color, YarmoolkaSupplier = s })
                .Where(x=>x.YarmoolkaSupplier.ID == p_supplierId.Value);
               
            }
            var result = queryOver.Select(x => new YarmoolkaClass { ID=x.ID, Name = x.Name, ModelDate = x.ModelDate, Style = x.Style, Price = x.Price,Company =x.Company, Size=x.Size,Color=x.Color,YarmoolkaSupplier = x.YarmoolkaSupplier }).ToList();

            // return this._jsonSerializer.Serialize(result); // Run the query and avoid context dispose
            return result;

        }

        // GET: Yarmoolkas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Yarmoolka = await _context.Yarmoolka
                .SingleOrDefaultAsync(m => m.ID == id);

            if (Yarmoolka == null)
            {
                return NotFound();
            }

            ImdbMovie YarmoolkaRev= this.GetYarmoolkaData(Yarmoolka.Name);
    

            return View("details",YarmoolkaRev);
        }

        // GET: Yarmoolkas/Create
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public IActionResult Create()
        {
            PopulateSuppliersDropDownList();
            return View();
        }

        // POST: Yarmoolkas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Create([Bind("ID,Title,ModelDate,Style,Price,Company,Length,Color,SupplierId")] Models.YarmoolkaClass Yarmoolka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Yarmoolka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthorId = new SelectList(_context.Supplier, "ID", "Name", Yarmoolka.SupplierId);
            return View(Yarmoolka);
        }
        
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Yarmoolka = await _context.Yarmoolka.SingleOrDefaultAsync(m => m.ID == id);
            if (Yarmoolka == null)
            {
                return NotFound();
            }
            PopulateSuppliersDropDownList(Yarmoolka.YarmoolkaSupplier.ID);
            return View(Yarmoolka);
        }

        // POST: Yarmoolkas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ModelDate,Style,Price,Company,Length,Color")] Models.YarmoolkaClass Yarmoolka)
        {
            if (id != Yarmoolka.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Yarmoolka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YarmoolkaExists(Yarmoolka.ID))
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
            PopulateSuppliersDropDownList(Yarmoolka.YarmoolkaSupplier.ID);
            return View(Yarmoolka);
        }


        // GET: Yarmoolkas/Delete/5
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Yarmoolka = await _context.Yarmoolka
                .SingleOrDefaultAsync(m => m.ID == id);
            if (Yarmoolka == null)
            {
                return NotFound();
            }

            return View(Yarmoolka);
        }

        // POST: Yarmoolkas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Yarmoolka = await _context.Yarmoolka.SingleOrDefaultAsync(m => m.ID == id);
            _context.Yarmoolka.Remove(Yarmoolka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ImdbMovie GetYarmoolkaData(string YarmoolkaName)
        {
            string  API_KEY =  _configuration.GetSection("AppSettings")["ImdbApiKey"];

            var imdb = new Imdb(API_KEY);
            var Yarmoolka =  imdb.GetMovieAsync(YarmoolkaName);

           return Yarmoolka.Result;

            
        }

        private bool YarmoolkaExists(int id)
        {
            return _context.Yarmoolka.Any(e => e.ID == id);
        }
        private void PopulateSuppliersDropDownList(object selectedSupplier = null)
        {
            var suppliersQuery = from d in _context.Supplier
                                   orderby d.Name
                                   select d;
            
            ViewBag.SupplierId = new SelectList(suppliersQuery, "ID", "Name", selectedSupplier);
            //new SelectList()
        }
    }
}

