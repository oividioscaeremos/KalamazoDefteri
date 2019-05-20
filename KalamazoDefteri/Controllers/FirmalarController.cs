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
        [Authorize(Roles ="user")]
        public ActionResult Index()
        {
            return View(new CompaniesIndex {
                ourCompanies = Database.Session.Query<Models.Companies>().ToList()
            });
        }

        [Authorize(Roles = "user")]
        public ActionResult CompanyView(int id)
        {
            return View(new CompaniesViewOne {
                ourCompany = Database.Session.Load<Models.Companies>(id)
            });
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult CompanyView(int id, CompaniesViewOne form)
        {
            var firma = Database.Session.Load<Models.Companies>(id);

            firma.Address = form.ourCompany.Address;
            firma.Balance = form.ourCompany.Balance;
            firma.Companyname = form.ourCompany.Companyname;
            firma.Faxnumber = form.ourCompany.Faxnumber;
            firma.Iban = form.ourCompany.Iban;
            firma.Phonenumber = form.ourCompany.Phonenumber;
            firma.Taxadministration = form.ourCompany.Taxadministration;

            Database.Session.Update(firma);
            Database.Session.Flush();

            return RedirectToRoute("Firmalar");
        }

        [Authorize(Roles = "user")]
        public ActionResult Income()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult Outgoing()
        {
            return View();
        }
    }
}