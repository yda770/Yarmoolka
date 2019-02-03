using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookShop.Controllers
{
    public class BranchesController : Controller
    {
 

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllBranches()
        {
            return Json(new Yarmoolka.Models.YarmoolkaClass() { Name = "amit"});
        }
    }
}