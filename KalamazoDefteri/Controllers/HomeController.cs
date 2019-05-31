using Rotativa;
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
            var currentUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            ViewModels.PrintReportViewModel customer = new ViewModels.PrintReportViewModel();
            customer.allIncomings = Database.Session.Query<Models.Incomings>().Where(i => i.Users.Id == currentUser.Id);
            customer.allOutGoings = Database.Session.Query<Models.Outgoings>().Where(i => i.Users.Id == currentUser.Id);
            customer.currUser = currentUser;
            return View(customer);
        }

        //[Authorize(Roles = "user")]
        //public ActionResult PrintReport(int id)
        //{
        //    var currentUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

        //    var incomings = Database.Session.Query<Models.Incomings>().Where(i => i.Users.Id == id).ToList();
        //    var outgoings = Database.Session.Query<Models.Outgoings>().Where(i => i.Users.Id == id).ToList();

        //    //var userid = Database.Session.Load<Models.User>(userData[0].Id);
        //    return View(new ViewModels.PrintReportViewModel
        //    {
        //        allIncomings = incomings,
        //        allOutGoings = outgoings,
        //        currUser = currentUser
        //    });
        //}

        [HttpPost]
        public ActionResult PrintPartialViewToPdf(ViewModels.PrintReportViewModel form)
        {
            if (form.reportFrom.CompareTo(form.reportTo) > 0 || form.reportFrom.CompareTo(DateTime.Now) > 0 || form.reportTo.CompareTo(DateTime.Now) > 0)
                return RedirectToAction("Index");  // Rapor aralığının kontrollerini yapıyoruz ki geçersiz aralıklar için rapor hazırlanamasın.

            var currentUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            ViewModels.PrintReportViewModel customer = new ViewModels.PrintReportViewModel();
            customer.allIncomings = Database.Session.Query<Models.Incomings>()
                .Where(i => i.Users.Id == currentUser.Id && i.Date.CompareTo(form.reportFrom) >= 0 && i.Date.CompareTo(form.reportTo) <= 0);
            customer.allOutGoings = Database.Session.Query<Models.Outgoings>()
                .Where(i => i.Users.Id == currentUser.Id && i.Date.CompareTo(form.reportFrom) >= 0 && i.Date.CompareTo(form.reportTo) <= 0);
            customer.currUser = currentUser;
            customer.reportFrom = form.reportFrom;
            customer.reportTo = form.reportTo;
            var report = new PartialViewAsPdf("~/Views/Shared/_PrintReport.cshtml", customer);
            return report;
        }
    }
}