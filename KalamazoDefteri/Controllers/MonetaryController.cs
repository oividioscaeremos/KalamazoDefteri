using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KalamazoDefteri.ViewModels;

namespace KalamazoDefteri.Controllers
{
    [Authorize(Roles = "user")]
    public class MonetaryController : Controller
    {
        // Mevcut kullanıcının gelirlerinin listelenmesi için gerekli olan view'ı döndüren yordam
        public ActionResult IncomeIndex(int id)
        {
            var incomings = Database.Session.Query<Models.Incomings>()
                .Where(inc => inc.Users.Id == id)
                .OrderByDescending(inc => inc.Date)
                .ToList();

            return View(new ViewModels.Monetary
            {
                ourCompanies = incomings
            });
        }

        public ActionResult NewIncome()
        {
            IEnumerable<Models.Companies> allComp = Database.Session.Query<Models.Companies>().OrderBy(c => c.Companyname);
            return View(new ViewModels.Income {
                sirketler = new SelectList(allComp,"Lütfen bir firma seçiniz.")
            });
        }

        [HttpPost]
        public ActionResult NewIncome(ViewModels.Income form)
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .ToList();
            currUser[0].Balance += form.Payment;
            var income = new Models.Incomings();

            // SelectList'ten gelen CompanyID'e göre şirketimizin tüm bilgilerini çekiyoruz
            var company = Database.Session.Load<Models.Companies>(form.selectedSirketID);


            income.Users = currUser[0];
            income.Date = form.Date;
            income.Companies = company;
            income.Payment = form.Payment;
            income.Explanation = form.Explanation;

            Database.Session.Save(income);
            Database.Session.Update(currUser[0]);
            Database.Session.Flush();

            return RedirectToAction("IncomeIndex", new { currUser[0].Id });
        }

        public ActionResult OutgoingIndex()
        {
            return View();
        }
        
    }
}