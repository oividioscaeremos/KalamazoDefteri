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
        public void CheckDatabase(int id)
        {
            var currUser = Database.Session.Query<Models.User>()
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            var allIncomes = Database.Session.Query<Models.Incomings>()
                .Where(i => i.isAdded == "0"); // ödenmesi alınmamış gelirleri çekiyoruz

            var allOutgoings = Database.Session.Query<Models.Outgoings>()
                .Where(o => o.isDecreased == "0"); // ödemesi yapılmamış giderleri çekiyoruz.

            /*
             income.beforeBalance = company.Balance;
                    company.Balance -= form.Payment;
                    income.didEffectCompany = "1";
                    income.isAdded = "0"; // ödeme elimize geçmediği için kullanıcımızın balance'ına etki etmeyecek. Ödeme tarihinde 
                                          // CheckDatabase() ile eklenecek.
             */

            foreach (var income in allIncomes)
            {
                if (income.Date.CompareTo(DateTime.Now) <= 0) // Gelirin planlanan tarihi geldiyse işlemlere tabi tutacağız.
                {
                    var company = Database.Session.Load<Models.Companies>(income.Companies.Companyid);

                    if (income.beforeBalance > 0) // şirkete bu gelir planlanmadan önce borcumuz vardı
                    {
                        if (income.beforeBalance < income.Payment) // Bizim firmaya olan borcumuz elde ettiğimiz gelirden düşükse
                        {
                            company.Balance = 0;
                            currUser.Balance += (income.Payment - income.beforeBalance);
                            income.isAdded = "1";
                        }
                        else if (income.beforeBalance > income.Payment) // Bizim firmaya olan borcumuz elde ettiğimiz gelirden daha fazla
                        {
                            currUser.Balance += income.Payment;
                            income.isAdded = "1";
                        }
                    }
                    else if (income.beforeBalance < 0) // şirketten bu gelir planlanmadan önce şirketin bize borcu vardı
                    {
                        income.isAdded = "1";

                        income.didEffectCompany = "0";
                    }
                    else // şirketten bu gelir planlanmadan önce şirketin durumu nötr ise
                    {
                        company.Balance += income.Payment;
                        currUser.Balance += income.Payment;
                        income.isAdded = "1";
                        income.didEffectCompany = "1";
                    }
                    Database.Session.Update(company);
                    Database.Session.Update(income);
                }
            }

            foreach (var outgoing in allOutgoings)
            {
                if (outgoing.Date.CompareTo(DateTime.Now) <= 0) // Giderin planlanan tarihi geldiyse işleme tabi tutacağız.
                {
                    var company = Database.Session.Load<Models.Companies>(outgoing.Companies.Companyid);

                    company.Balance -= outgoing.Payment; // firmaya ödeme yaptığımız için firmanın bakiyesini düzenliyoruz.
                    currUser.Balance -= outgoing.Payment; // firmaya ödememiz gittiği için kullanıcımızın bakiyesinden düşüyoruz.
                    outgoing.isDecreased = "1";
                    Database.Session.Update(outgoing);
                }
            }

            Database.Session.Update(currUser);
            Database.Session.Flush();
        }

        // Mevcut kullanıcının gelirlerinin listelenmesi için gerekli olan view'ı döndüren yordam
        public ActionResult IncomeIndex(int id, int page = 1)
        {
            CheckDatabase(id);

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

            company.Balance -= income.Payment;
            currUser.Balance += income.Payment;

            income.didEffectCompany = "1";
            income.isAdded = "1";
            income.isCharged = "1";

            Database.Session.Save(income);
            Database.Session.Update(currUser);
            Database.Session.Flush();

            return RedirectToAction("IncomeIndex", new { id = currUser.Id });
        }

        public ActionResult DeleteIncome(int id, int from = -1) // firma görüntüleme ekranından silinirse ona göre döndürme yapılacak
        {
            var income = Database.Session.Load<Models.Incomings>(id);

            var currUser = Database.Session.Query<Models.User>()                     // Kullanıcının balance değerinden düşürülebilmesi için
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name); // kullanıcıyı da update etmemiz gerekmekte.
                                                                                    // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
                                                                                    // (çift tıklama gibi şeyler yaşanabilir)
            var belongsToCompany = Database.Session.Load<Models.Companies>(income.Companies.Companyid);

            if (income == null)
                RedirectToAction("IncomeIndex", currUser.Id);

            belongsToCompany.Balance += income.Payment;
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
            newOutgoing.isDecreased = "1";    // giderin ödemesi yapıldığı için tekrar işleme tabi tutulmaması için ödendi olarak işaretliyoruz.
            newOutgoing.beforeBalance = company.Balance;

            company.Balance += form.Payment; // Firma bizden alacağı var durumunda olacağı için, ödeme yaptığımızı belirtmemiz gerekli.

            newOutgoing.didEffectCompany = "1";


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

            belongsToComp.Balance -= outgoing.Payment;
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

        //public ActionResult PayOutgoing(int id, int from = -1)
        //{
        //    var outgoing = Database.Session.Load<Models.Outgoings>(id);
        //    outgoing.Date = DateTime.Now;

        //    var currUser = Database.Session.Query<Models.User>()                     // Kullanıcının balance değerinden düşürülebilmesi için
        //        .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name); // kullanıcıyı da update etmemiz gerekmekte.
        //                                                                            // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
        //                                                                            // (çift tıklama gibi şeyler yaşanabilir)
        //    var belongsToComp = Database.Session.Query<Models.Companies>()
        //        .FirstOrDefault(o => o.Companyid == outgoing.Companies.Companyid);

        //    if (outgoing == null)
        //        RedirectToAction("OutgoingIndex", new { id = currUser.Id });

        //    if (outgoing.beforeBalance < 0)
        //    {
        //        belongsToComp.Balance += outgoing.Payment;      // Firmaya ödeme yapılmıştı, bunu geriye alıyoruz.
        //    }
        //    else
        //    {
        //        belongsToComp.Balance -= outgoing.Payment;  // Firmaya ödeme yapılmıştı, bunu geriye alıyoruz.
        //    }
        //    currUser.Balance += outgoing.Payment;         // Bizden para çıkmıştı, bunu geriye alıyoruz.

        //    Database.Session.Update(outgoing);
        //    Database.Session.Update(currUser);
        //    Database.Session.Update(belongsToComp);

        //    Database.Session.Flush();

        //    if (from != -1)
        //    {

        //        return RedirectToRoute("CompanyView", new { id = from });
        //    }
        //    else
        //    {
        //        return RedirectToAction("OutgoingIndex", new { id = currUser.Id });
        //    }
        //}

        //public ActionResult PayCompany(int id, int from = -1)
        //{
        //    var currUser = Database.Session.Query<Models.User>()                       // Kullanıcının balance değerinden düşürülebilmesi için
        //        .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);   // kullanıcıyı da update etmemiz gerekmekte.
        //                                                                              // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
        //                                                                              // (çift tıklama gibi şeyler yaşanabilir)
        //    var belongsToComp = Database.Session.Load<Models.Companies>(id);

        //    if (belongsToComp.Balance > 0)
        //    {
        //        // firmanın bizden alacağı para var
        //        Models.Outgoings outgoingFull = new Models.Outgoings();

        //        outgoingFull.beforeBalance = belongsToComp.Balance;
        //        outgoingFull.Companies = belongsToComp;
        //        outgoingFull.Date = DateTime.Now;
        //        outgoingFull.Explanation = belongsToComp.Companyname + " Firmasının tüm alacağı ödendi.";
        //        outgoingFull.Payment = belongsToComp.Balance;
        //        outgoingFull.Users = currUser;
        //        outgoingFull.isDecreased = "1";
        //        outgoingFull.didEffectCompany = "1";

        //        currUser.Balance -= belongsToComp.Balance;
        //        belongsToComp.Balance = 0;

        //        Database.Session.Update(currUser);
        //        Database.Session.Update(belongsToComp);
        //        Database.Session.Save(outgoingFull);

        //        Database.Session.Flush();
        //    }
        //    else
        //    {
        //        // firmadan alacağımız var
        //        Models.Incomings incomeFull = new Models.Incomings();

        //        incomeFull.beforeBalance = belongsToComp.Balance;
        //        incomeFull.Companies = belongsToComp;
        //        incomeFull.Date = DateTime.Now;
        //        incomeFull.Explanation = belongsToComp.Companyname + " Firmasından tüm alınacaklar temin edildi.";
        //        incomeFull.Payment = Math.Abs(belongsToComp.Balance);
        //        incomeFull.Users = currUser;
        //        incomeFull.isCharged = "1";
        //        incomeFull.isAdded = "1";
        //        incomeFull.didEffectCompany = "1";

        //        currUser.Balance += Math.Abs(belongsToComp.Balance);
        //        belongsToComp.Balance = 0;

        //        Database.Session.Update(currUser);
        //        Database.Session.Update(belongsToComp);
        //        Database.Session.Save(incomeFull);

        //        Database.Session.Flush();
        //    }


        //    return RedirectToRoute("Firmalar", new { id = from });
        //}

    }
}