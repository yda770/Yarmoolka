using System.Collections.Generic;
using System.Linq;
//using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Mvc;
using Yarmoolka.Models;
//using Yarmoolka.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yarmoolka.Controllers
{
    public class QuereiesController : Controller
    {
        private  YarmoolkaContext _context;
        //private readonly JavaScriptSerializer _jsonSerializer;

        public QuereiesController(YarmoolkaContext context)
        {
            this._context = context;
            //this._jsonSerializer = new JavaScriptSerializer();
        }

        public List<Models.YarmoolkaClass> GetMostExpensiveYarmoolkas(int p_topToTake)
        {
            var Yarmoolkas = this._context.Yarmoolka.OrderByDescending(x => x.Price).Take(p_topToTake).ToList();
            return Yarmoolkas;
        }
        public List<Models.YarmoolkaClass> GetChippestYarmoolkas(int p_topToTake)
        {
            var Yarmoolkas = this._context.Yarmoolka.OrderBy(x => x.Price).Take(p_topToTake).ToList();
            return Yarmoolkas;
        }
        
        public object GetYarmoolkasGroupedBySuppliers()
        {
            var Yarmoolkas = this._context.Yarmoolka
               .Join(this._context.Supplier,
                     m => m.SupplierId,
                     s => s.ID,
                     (m, s) => new {
                         Name = m.Name,
                         Supplier = s.Name
                     }
                     ).GroupBy(x=>x.Supplier).ToArray();
           
            return Yarmoolkas;
        }

        public object  GetYarmoolkasGroupByReleaseYear()
        {
           var Yarmoolkas = this._context.Yarmoolka.GroupBy(x=> x.ModelDate.Year)
                                          .OrderByDescending(x => x.Count())
                                          .ToArray();
            return Yarmoolkas;
        }

        public object GetYarmoolkasByStyle()
        {
            var Yarmoolkas = this._context.Yarmoolka.GroupBy(x => x.Style)
                                          .OrderByDescending(x => x.Count()
                                          );
            return Yarmoolkas;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(this.GetYarmoolkasGroupedBySuppliers());
        }
    }
}
