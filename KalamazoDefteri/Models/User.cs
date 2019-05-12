using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace KalamazoDefteri.Models
{
    public class User
    {
        public virtual int ID { get; set; }
        public virtual string username { get; set; }
        public virtual string password { get; set; }
        public virtual string email { get; set; }
    }

    public class UsersMap : ClassMapping<User>
    {

        public UsersMap()
        {
            Table("users");
            Schema("kalamazodefteri");
            Lazy(true);
            Id(x => x.ID, map => map.Generator(Generators.Identity));
            Property(x => x.username, map => map.NotNullable(true));
            Property(x => x.email, map => map.NotNullable(true));
            Property(x => x.password, map => { map.Column("password_hash"); map.NotNullable(true); });
        }
    }
}
