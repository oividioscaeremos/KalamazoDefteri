using KalamazoDefteri.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KalamazoDefteri.Controllers
{
    public class FirmalarController : Controller
    {
        // GET: Firmalar
        public ActionResult Index()
        {
            return View(new CompaniesIndex {
                ourCompanies = Database.Session.Query<Models.Companies>().ToList()
            });
        }

        public ActionResult Income()
        {
            return View();
        }

        public ActionResult Outgoing()
        {
            return View();
        }
    }
}