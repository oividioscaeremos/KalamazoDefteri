using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using System.Web;

namespace KalamazoDefteri.Models
{
    public class Incomings
    {
        public virtual int Incomeid { get; set; }
        public virtual User Users { get; set; }
        public virtual Companies Companies { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Explanation { get; set; }
        public virtual int Payment { get; set; }

    }

    public class IncomeMap : ClassMapping<Incomings>
    {

        public IncomeMap()
        {
            Table("income");
            Schema("kalamazodefteri");
            Lazy(true);
            Id(x => x.Incomeid, map => map.Generator(Generators.Identity));
            Property(x => x.Date, map => map.NotNullable(true));
            Property(x => x.Explanation, map => map.NotNullable(true));
            Property(x => x.Payment, map => map.NotNullable(true));
            ManyToOne(x => x.Users, map => { map.Column("userID"); map.Cascade(Cascade.None); });

            ManyToOne(x => x.Companies, map => { map.Column("companyID"); map.Cascade(Cascade.None); });


        }
    }
}