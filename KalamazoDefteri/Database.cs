using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalamazoDefteri
{
    public class Database
    {
        public static void Configure() {
            var config = new Configuration();

            config.Configure();
        }

        public static void OpenSession(){

        }

        public static void CloseSession(){

        }
    }
}