using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Encodings.Web;


namespace Mvc.Movie.AddControllersWithViews
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }
        //
        // GET: /HelloWorld/Welcome/
        public string Welcome(string name, int yearOfBorn = 1912, int ID = 1)
        {
            return HtmlEncoder.Default.Encode(
                $"Hello {name}, you've born at {yearOfBorn}. Your ID is {ID}");
                                        // "... you've ..." - after encoding it gives
                                        //   "... you&#x27;ve ..."
        }
    }
}