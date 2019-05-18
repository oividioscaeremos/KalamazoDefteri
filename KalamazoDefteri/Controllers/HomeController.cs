using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KalamazoDefteri.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[Authorize(Roles = "user")]
        public ActionResult Index()
        {
            if (HttpContext.User.IsInRole("admin"))
                return RedirectToAction("Index", "Users", new { area = "Admin" });
            else
                return View();
        }
    }
}