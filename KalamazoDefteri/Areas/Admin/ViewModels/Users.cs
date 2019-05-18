using KalamazoDefteri.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.Areas.Admin.ViewModels
{
    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }

    }
    public class UsersEditUser
    {
        [DataType(DataType.Text)]
        [Display(Prompt = "Username")]
        public string username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "Email")]
        public string email { get; set; }

        public IList<RoleCheckBox> Roles { get; set; }

    }

    public class RoleCheckBox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }
}