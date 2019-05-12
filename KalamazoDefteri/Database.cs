using KalamazoDefteri.Models;
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
        private const string SessionKey = "KalamazoDefteri.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get
            {
                return (ISession)HttpContext.Current.Items[SessionKey];
            }
        }

        public static void Configure() {

            var config = new Configuration();

            var mapper = new ModelMapper();
            mapper.AddMapping<UsersMap>();
            mapper.AddMapping<RolesMap>();
            mapper.AddMapping<OutgoingsMap>();
            mapper.AddMapping<IncomeMap>();
            mapper.AddMapping<CompaniesMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            config.Configure();

            _sessionFactory = config.BuildSessionFactory();

        }

        public static void OpenSession(){
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();

        }

        public static void CloseSession(){
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
            {
                session.Close();
            }

            HttpContext.Current.Items.Remove("Session");
        }
    }
}