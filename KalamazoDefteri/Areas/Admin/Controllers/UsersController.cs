using KalamazoDefteri.Areas.Admin.ViewModels;
using KalamazoDefteri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace KalamazoDefteri.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            return View(new UsersIndex()
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }

        private void SyncRoles(IList<RoleCheckBox> checkBoxes, IList<Roles> roles)
        {
            var selectedRoles = new List<Roles>();

            foreach (var role in Database.Session.Query<Roles>())
            {
                var checkbox = checkBoxes.Single(c => c.Id == role.id);
                checkbox.Name = role.name;

                if (checkbox.IsChecked)
                {
                    selectedRoles.Add(role);
                }
            }

            foreach (var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
            {
                roles.Add(toAdd);
            }

            foreach (var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
            {
                roles.Remove(toRemove);
            }

        }

        public ActionResult EditUser(int id) {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            return View(new UsersEditUser()
            {
                username = user.Username,
                email = user.Email,
                Roles = Database.Session.Query<Roles>().Select(role => new RoleCheckBox()
                {
                    Id = role.id,
                    Name = role.name,
                    IsChecked = user.Roles.Contains(role)
                }).ToList()
            });
        }
        [HttpPost]
        public ActionResult EditUser(int id, UsersEditUser form) {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            if (Database.Session.Query<User>().Any(u => u.Username == form.username && u.Id != id))
                ModelState.AddModelError("Username", "Kullanıcı adı zaten kullanımda.");

            if (!ModelState.IsValid)
                return View(form);

            user.Username = form.username;
            user.Email = form.email;
            SyncRoles(form.Roles,user.Roles);
            Database.Session.Update(user);
            Database.Session.Flush();

            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            return View(new UsersResetPassword {
                username = user.Username
            });
        }

        [HttpPost]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();            

            if (!ModelState.IsValid)
                return View(form);

            user.SetPassword(form.Password);
            var asd = Guid.NewGuid();
            Database.Session.Update(user);
            Database.Session.Flush();

            return RedirectToAction("index");
        }
        
        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();
            Database.Session.Delete(user);
            Database.Session.Flush();

            return RedirectToAction("index");
        }
    }
}