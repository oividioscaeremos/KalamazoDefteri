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
        [Authorize(Roles = "user")]
        public ActionResult Index()
        {
            //var userid = Database.Session.Load<Models.User>(userData[0].Id);
            return View();
        }
    }
}