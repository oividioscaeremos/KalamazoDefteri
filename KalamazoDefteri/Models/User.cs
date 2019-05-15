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
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual IList<Roles> Roles { get; set; }

    }

    public class UsersMap : ClassMapping<User>
    {

        public UsersMap()
        {
            Table("users");
            Schema("kalamazodefteri");
            
            Lazy(true);
            Id(x => x.Id, map => map.Generator(Generators.Assigned));
            Property(x => x.Username, map => map.NotNullable(true));
            Property(x => x.Email, map => map.NotNullable(true));
            Property(x => x.PasswordHash, map => { map.Column("password_hash"); map.NotNullable(true); });

        }

        public class RoleCheckBox
        {
            public int Id { get; set; }
            public bool IsChecked { get; set; }
            public string Name { get; set; }
        }
    }
}
