using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Yarmoolka.Models;

namespace Yarmoolka.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ////[Produces("application/json")]
        //public JsonResult GetTweets()
        //{
        //    string API_key = "jLTS4Uk1zGiZrZ3KHPbNXY9wE";
        //    string API_secret = "YH04O7WOdJEp06O0joojqOoK4PxU2UUSICcTgoycjfOW6Qxbt7";
        //    string Access_token_secret = "K7JizwmE3R3uQUuuXYpuJypDGuetwpG6yg3NefudpnreG";
        //    string Access_token = "635262181-HqMqCszScmTNOiUHQwa8bH30tNybl2OogvFE5PeU";
        //    string searhScreenName = "@TODAYYarmoolkas";

        //    var service = new TwitterService(API_key, API_secret);
        //    service.AuthenticateWith(Access_token, Access_token_secret);

        //    //ScreenName - twitter user. 

        //    IEnumerable<TwitterStatus> res = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = searhScreenName });
            

        //    return Json(res.Select(x => x.TextAsHtml));

        //    // return result;
        //}


    }
}
