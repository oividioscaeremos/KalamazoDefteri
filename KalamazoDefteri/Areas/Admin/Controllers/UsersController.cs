using KalamazoDefteri.Areas.Admin.ViewModels;
using KalamazoDefteri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KalamazoDefteri.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        //[Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            return View(new UsersIndex()
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }
    }
}