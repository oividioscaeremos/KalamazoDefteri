using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual IList<Roles> Roles { get; set; }

        public User()
        {
            Roles = new List<Roles>();
        }

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        public virtual bool CheckPassword(string password)
        {
            var pass = BCrypt.Net.BCrypt.Verify(password, PasswordHash);
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }      
    }

    public class UsersMap : ClassMapping<User>
    {

        public UsersMap()
        {
            Table("users");
            Schema("kalamazodefteri");
            
            Id(x => x.Id, map => map.Generator(Generators.Identity));
            Property(x => x.Username, map => map.NotNullable(true));
            Property(x => x.Email, map => map.NotNullable(true));
            Property(x => x.PasswordHash, map => { map.Column("password_hash"); map.NotNullable(true); });

            Bag(x => x.Roles, x => {
                x.Table("role_users");
                x.Key(k => k.Column("userid"));
            }, x => x.ManyToMany(k => k.Column("roleid")));

        }
        
    }
}
