using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KalamazoDefteri.Models;

namespace KalamazoDefteri.Areas.Admin.ViewModels
{
    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }

    }
}