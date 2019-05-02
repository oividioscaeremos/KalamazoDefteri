using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.Models
{
    public class User
    {
        public virtual int ID { get; set; }
        public virtual string username { get; set; }
        public virtual string password { get; set; }
        public virtual string email { get; set; }

    }
}