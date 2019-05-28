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
        public void CheckDatabase(int id)
        {
            var currUser = Database.Session.Query<Models.User>()
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            var allIncomes = Database.Session.Query<Models.Incomings>()
                .Where(i => i.isAdded == "0"); // ödenmesi alınmamış gelirleri çekiyoruz

            var allOutgoings = Database.Session.Query<Models.Outgoings>()
                .Where(o => o.isDecreased == "0"); // ödemesi yapılmamış giderleri çekiyoruz.

            foreach (var income in allIncomes)
            {
                if(income.Date.CompareTo(DateTime.Now) <= 0) // Gelirin planlanan tarihi geldiyse işlemlere tabi tutacağız.
                {
                    var company = Database.Session.Load<Models.Companies>(income.Companies.Companyid);
                    
                    company.Balance += income.Payment; // firmadan para artık elimize geçtiği için firma bakiyesini de düzenlememiz gerekmekte.
                    currUser.Balance += income.Payment; // Para elimize geçtiği için kendi user'ımızın balance'ını arttırıyoruz.
                    income.isAdded = "1"; // ödendiği için ödendi ibaresini 1 yapıyoruz ve tekrardan işlemden geçmesini engelliyoruz.

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
        public ActionResult IncomeIndex(int id)
        {
            CheckDatabase(id);

            var incomings = Database.Session.Query<Models.Incomings>()
                .Where(inc => inc.Users.Id == id)
                .OrderByDescending(inc => inc.Date)
                .ToList();

            return View(new ViewModels.Income
            {
                allIncomings = incomings
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
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            var income = new Models.Incomings();

            // SelectList'ten gelen CompanyID'e göre şirketimizin tüm bilgilerini çekiyoruz
            var company = Database.Session.Load<Models.Companies>(form.selectedSirketID);

            if(form.Date.CompareTo(DateTime.Now) > 0) // Gelir tarihi ileri bir tarihse firma bize borçlanmış olacak.
            {
                company.Balance -= form.Payment;
                income.isAdded = "0"; // ödeme elimize geçmediği için kullanıcımızın balance'ına etki etmeyecek. Ödeme tarihinde 
                                     // CheckDatabase() ile eklenecek.
            }
            else
            {
                currUser.Balance += form.Payment; // mevcut kullanıcıya gelir olduğu için balance'ı arttırıyoruz.
                income.isAdded = "1";            // gelir elimize geçtiyse balance'a eklendiğini belirtiyoruz.
            }

            income.Users = currUser;
            income.Date = form.Date;
            income.Companies = company;
            income.Payment = form.Payment;
            income.Explanation = form.Explanation;

            Database.Session.Save(income);
            Database.Session.Update(currUser);
            Database.Session.Flush();

            return RedirectToAction("IncomeIndex", new { currUser.Id });
        }

        public ActionResult DeleteIncome(int id, int from = -1)
        {
            var income = Database.Session.Load<Models.Incomings>(id);

            var currUser = Database.Session.Query<Models.User>()                     // Kullanıcının balance değerinden düşürülebilmesi için
                .FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name); // kullanıcıyı da update etmemiz gerekmekte.
                                                                         // ID'den çekmememin nedeni, ID'nin boş gelebilme ihtimalinin olması.
                                                                        // (çift tıklama gibi şeyler yaşanabilir)
            if (income == null)
                RedirectToAction("IncomeIndex", currUser.Id);

            currUser.Balance -= income.Payment;

            Database.Session.Delete(income);
            Database.Session.Update(currUser);

            Database.Session.Flush();

            if(from != -1)
            {
                
                return RedirectToRoute("CompanyView", new { id = from });
            }
            else
            {
                return RedirectToAction("IncomeIndex", new { currUser.Id });
            }
        }        

        public ActionResult OutgoingIndex(int id)
        {
            var outgoings = Database.Session.Query<Models.Outgoings>()
                .Where(o => o.Users.Id == id)
                .OrderByDescending(o => o.Date)
                .ToList();

            return View(new ViewModels.Outgoing {
                allOutgoings = outgoings
            });
        }

        public ActionResult NewOutgoing()
        {
            IEnumerable<Models.Companies> allComp = Database.Session.Query<Models.Companies>().OrderBy(c => c.Companyname);

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

            if (form.Date.CompareTo(DateTime.Now) > 0) // gider eğer ileri tarihliyse bu if'ten içeri girecek
            {
                company.Balance += form.Payment; // firma bizden alacaklı duruma geçeceği için firmanın balance'ını arttırıyoruz.
                newOutgoing.isDecreased = "0";  // Ödemeyi yapmadığımızı belirtiyoruz. Ödeme tarihinde 
                                               // CheckDatabase() ile eklenecek.
            }
            else
            {
                currUser.Balance -= form.Payment; // Para kullanıcıdan çıktığı için kullanıcı bakiyesini düşürüyoruz.
                newOutgoing.isDecreased = "1";   // giderin ödemesi yapıldığı için tekrar işleme tabi tutulmaması için ödendi olarak işaretliyoruz.
            }

            newOutgoing.Users = currUser;
            newOutgoing.Companies = company;
            newOutgoing.Date = form.Date;
            newOutgoing.Explanation = form.Explanation;
            newOutgoing.Payment = form.Payment;

            Database.Session.Save(newOutgoing);
            Database.Session.Update(currUser);
            Database.Session.Flush();

            return RedirectToAction("OutgoingIndex", new { id = currUser.Id });
        }
    }
}