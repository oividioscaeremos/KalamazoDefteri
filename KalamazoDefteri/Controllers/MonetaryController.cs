using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KalamazoDefteri.Infrastructures;
using KalamazoDefteri.ViewModels;

namespace KalamazoDefteri.Controllers
{
    [Authorize(Roles = "user")]
    public class MonetaryController : Controller
    {
        // Mevcut kullanıcının gelirlerinin listelenmesi için gerekli olan view'ı döndüren yordam
        public ActionResult IncomeIndex(int id, int page = 1)
        {
            var incomings = Database.Session.Query<Models.Incomings>()
                .Where(inc => inc.Users.Id == id)
                .OrderByDescending(inc => inc.Date)
                .ToList();

            return View(new ViewModels.Income
            {
                ourIncomings = new PagedData<Models.Incomings>(incomings, incomings.Count(), page, 10)
            });
        }

        public ActionResult NewIncome()
        {
            try
            {
                var currUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
                IEnumerable<Models.Companies> allComp = Database.Session.Query<Models.Companies>()
                    .Where(c => c.belongsToUser.Id == currUser.Id)
                    .OrderBy(c => c.Companyname);
                return View(new ViewModels.Income
                {
                    sirketler = new SelectList(allComp, "Lütfen bir firma seçiniz. (Boş Bırakılamaz.)")
                });
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult NewIncome(ViewModels.Income form)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            // SelectList'ten gelen CompanyID'e göre şirketimizin tüm bilgilerini çekiyoruz
            var company = Database.Session.Load<Models.Companies>(form.selectedSirketID);

            var income = new Models.Incomings();
            income.Users = currUser;
            income.Date = DateTime.Now;
            income.Companies = company;
            income.Payment = form.Payment;
            income.Explanation = form.Explanation;

            company.inBalance += income.Payment;
            currUser.Balance += income.Payment;

            Database.Session.Save(income);
            Database.Session.Update(currUser);
            Database.Session.Flush();

            return RedirectToAction("IncomeIndex", new { id = currUser.Id });
        }

        public ActionResult DeleteIncome(int id, int from = -1) // firma görüntüleme ekranından silinirse ona göre döndürme yapılacak
        {
            var income = Database.Session.Load<Models.Incomings>(id);

            var currUser = Database.Session.Query<Models.User>()                      // Kullanıcının balance değerinden düşürülebilmesi için
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);  // kullanıcıyı da update etmemiz gerekmekte.
                                                                                    // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
                                                                                   // (çift tıklama gibi şeyler yaşanabilir)
            var belongsToCompany = Database.Session.Load<Models.Companies>(income.Companies.Companyid);

            if (income == null)
                RedirectToAction("IncomeIndex", currUser.Id);

            belongsToCompany.inBalance -= income.Payment;
            currUser.Balance -= income.Payment;

            Database.Session.Delete(income);
            Database.Session.Update(currUser);
            Database.Session.Update(belongsToCompany);

            Database.Session.Flush();

            if (from != -1)
            {

                return RedirectToRoute("CompanyView", new { id = from });
            }
            else
            {
                return RedirectToAction("IncomeIndex", new { id = currUser.Id });
            }
        }

        public ActionResult OutgoingIndex(int id, int page = 1)
        {
            var outgoings = Database.Session.Query<Models.Outgoings>()
                .Where(o => o.Users.Id == id)
                .OrderByDescending(o => o.Date)
                .ToList();

            return View(new ViewModels.Outgoing
            {
                ourOutgoings = new PagedData<Models.Outgoings>(outgoings, outgoings.Count(), page, 10)
            });
        }

        public ActionResult NewOutgoing()
        {
            var currUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            IEnumerable<Models.Companies> allComp = Database.Session.Query<Models.Companies>()
                .Where(c => c.belongsToUser.Id == currUser.Id)
                .OrderBy(c => c.Companyname);

            return View(new ViewModels.Outgoing
            {
                sirketler = new SelectList(allComp, "Lütfen bir firma seçiniz.")
            });
        }

        [HttpPost]
        public ActionResult NewOutgoing(ViewModels.Outgoing form)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // mevcut kullanıcımızı bakiyesinde değişiklik yapma ihtimalimiz olduğu için çekiyoruz.
            var currUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            // SelectList'ten gelen CompanyID'e göre şirketimizin tüm bilgilerini çekiyoruz
            var company = Database.Session.Load<Models.Companies>(form.selectedSirketID);

            // yeni bir gider oluşturuyoruz
            var newOutgoing = new Models.Outgoings();


            currUser.Balance -= form.Payment;  // Para kullanıcıdan çıktığı için kullanıcı bakiyesini düşürüyoruz.
            company.outBalance += form.Payment; // Firma bizden alacağı var durumunda olacağı için, ödeme yaptığımızı belirtmemiz gerekli.

            newOutgoing.Users = currUser;
            newOutgoing.Companies = company;
            newOutgoing.Date = DateTime.Now;
            newOutgoing.Explanation = form.Explanation;
            newOutgoing.Payment = form.Payment;

            Database.Session.Save(newOutgoing);
            Database.Session.Update(currUser);
            Database.Session.Flush();

            return RedirectToAction("OutgoingIndex", new { id = currUser.Id });
        }

        public ActionResult DeleteOutgoing(int id, int from = -1)
        {
            var outgoing = Database.Session.Load<Models.Outgoings>(id);


            var currUser = Database.Session.Query<Models.User>()                     // Kullanıcının balance değerinden düşürülebilmesi için
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name); // kullanıcıyı da update etmemiz gerekmekte.
                                                                                    // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
                                                                                    // (çift tıklama gibi şeyler yaşanabilir)
            var belongsToComp = Database.Session.Query<Models.Companies>()
                .FirstOrDefault(o => o.Companyid == outgoing.Companies.Companyid);

            if (outgoing == null)
                RedirectToAction("OutgoingIndex", new { id = currUser.Id });

            belongsToComp.outBalance -= outgoing.Payment;
            currUser.Balance += outgoing.Payment;

            Database.Session.Delete(outgoing);
            Database.Session.Update(currUser);
            Database.Session.Update(belongsToComp);

            Database.Session.Flush();

            if (from != -1)
            {

                return RedirectToRoute("CompanyView", new { id = from });
            }
            else
            {
                return RedirectToAction("OutgoingIndex", new { id = currUser.Id });
            }
        }

        

    }
}