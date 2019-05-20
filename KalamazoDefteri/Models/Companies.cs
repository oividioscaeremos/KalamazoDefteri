using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using System.Web;

namespace KalamazoDefteri.Models
{
    public class Companies
    {
        public virtual int Companyid { get; set; }
        public virtual User belongsToUser { get; set; }
        public virtual string Companyname { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phonenumber { get; set; }
        public virtual string Faxnumber { get; set; }
        public virtual string Taxadministration { get; set; }
        public virtual string Iban { get; set; }
        public virtual int Balance { get; set; }
    }

    public class CompaniesMap : ClassMapping<Models.Companies>
    {

        public CompaniesMap()
        {
            Table("companies");
            Schema("kalamazodefteri");
            Lazy(true);
            
            ManyToOne(x => x.belongsToUser, x => {
                x.Column("userID");
                x.NotNullable(true);
            });

            Id(x => x.Companyid, map => map.Generator(Generators.Identity));
            Property(x => x.Companyname, map => map.NotNullable(true));
            Property(x => x.Address, map => map.NotNullable(true));
            Property(x => x.Phonenumber, map => map.NotNullable(true));
            Property(x => x.Faxnumber, map => map.NotNullable(true));
            Property(x => x.Taxadministration, map => map.NotNullable(true));
            Property(x => x.Iban, map => map.NotNullable(true));
            Property(x => x.Balance, map => map.NotNullable(true));


            
        }
    }
}