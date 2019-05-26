using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KalamazoDefteri.Models;

namespace KalamazoDefteri.ViewModels
{

    /*
     Create.Table("income")
            .WithColumn("userID").AsInt32().ForeignKey("users", "ID").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("incomeID").AsInt32().Identity().PrimaryKey()
            .WithColumn("date").AsDate()
            .WithColumn("companyID").AsInt32().ForeignKey("companies", "companyID").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("explanation").AsString(512)
            .WithColumn("payment").AsInt32();             
         */

    public class Monetary
    {
        public IEnumerable<Models.Incomings> ourCompanies { get; set; }
    }

    public class View
    {
        public int id { get; set; }
        public int userID { get; set; }
        public DateTime date { get; set; }
        public int companyID { get; set; }
        public string explanation { get; set; }
        public int payment { get; set; }
    }

    public class Income
    {
        public int selectedSirketID { get; set; }
        public SelectList sirketler { get; set; }
        public int Incomeid { get; set; }
        public User Owner { get; set; }
        public Companies Companies { get; set; }
        public DateTime Date { get; set; }
        public string Explanation { get; set; }
        public int Payment { get; set; }
        public IEnumerable<Companies> allCompanies { get; set; }

    }
    
}