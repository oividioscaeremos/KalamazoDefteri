using KalamazoDefteri.Infrastructures;
using KalamazoDefteri.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KalamazoDefteri.Controllers
{
    [Authorize(Roles = "user")]
    public class FirmalarController : Controller
    {
        private const int companiesPerPage = 10;

        // GET: Firmalar
        public ActionResult Index(int page = 1)
        {
            var usersCompanies = Database.Session.Query<Models.Companies>()
                .Skip((page - 1) * companiesPerPage)
                .Take(companiesPerPage)
                .ToList();


            var totalCompanies = Database.Session.Query<Models.Companies>().Count();

            return View(new CompaniesIndex {
                ourCompanies = new PagedData<Models.Companies>(usersCompanies, totalCompanies, page, companiesPerPage)
            });
        }

        public ActionResult NewCompany(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewCompany(CompaniesNew formData)
        {
            var newComp = new Models.Companies();
            var currentUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .ToList();
            newComp.Companyname = formData.companyMame;
            newComp.Address = formData.address;
            newComp.belongsToUser = currentUser[0];
            newComp.Faxnumber = formData.faxNumber;
            newComp.Phonenumber = formData.phoneNumber;
            newComp.Iban = formData.IBAN;
            newComp.Taxadministration = formData.TaxAdministration;
            newComp.Balance = formData.balance;

            Database.Session.Save(newComp);
            Database.Session.Flush();

            return RedirectToRoute("Firmalar");
        }

        public ActionResult CompanyView(int id)
        {
            // Farklı kullanıcıların, URL kısmında oynama yaparak kendilerine ait olmayan
            // bir şirketi görüntülemeleri engellendi.

                // Veritabanından şu anda authenticate olmuş olan user'ın bilgileri çekiliyor.
            var currentUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .Select(u=> u.Id);

            // Company id'den firmanın ait olduğu kullanıcının ID'si çekiliyor.
            //var belongsTo = Database.Session.Load<Models.Companies>(id).belongsToUser.Id;
            var belongsTo = Database.Session.Query<Models.Companies>()
                .Where(u => u.Companyid == id)
                .Select(u => u.belongsToUser.Id);
            if(belongsTo.ToString() == currentUser.ToString())
            {
                return View(new CompaniesViewOne
                {
                    ourCompany = Database.Session.Load<Models.Companies>(id)
                });                
            }
            else
            {
                return RedirectToAction("index");
            }            
        }

        [HttpPost]
        public ActionResult CompanyView(int id, CompaniesViewOne form)
        {
            // Firma görüntüleme ekranındaki update işlemi gerçekleştirilmekte.

            var firma = Database.Session.Load<Models.Companies>(id);

            firma.Address = form.ourCompany.Address;
            firma.Balance = form.ourCompany.Balance;
            firma.Companyname = form.ourCompany.Companyname;
            firma.Faxnumber = form.ourCompany.Faxnumber;
            firma.Iban = form.ourCompany.Iban;
            firma.Phonenumber = form.ourCompany.Phonenumber;
            firma.Taxadministration = form.ourCompany.Taxadministration;
            var abc = form.ourCompany.Balance;
            firma.Balance = form.ourCompany.Balance;

            Database.Session.Update(firma);
            Database.Session.Flush();

            return RedirectToRoute("Firmalar");
        }
        
        public ActionResult DeleteCompany(int id)
        {
            // Firmanın Firmalar sekmesinden silinmesi
            var company = Database.Session.Load<Models.Companies>(id);
            if (company == null)
                return View("index");
            Database.Session.Delete(company);
            Database.Session.Flush();
            return RedirectToAction("index");
        }
        
    }
}