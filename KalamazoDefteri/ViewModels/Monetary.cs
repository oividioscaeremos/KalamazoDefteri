using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KalamazoDefteri.Infrastructures;
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
        public PagedData<Models.Incomings> ourIncomings { get; set; }

        [Required]
        public int selectedSirketID { get; set; }
        public SelectList sirketler { get; set; }
        public int Incomeid { get; set; }
        public Companies Companies { get; set; }

        [Required(ErrorMessage ="Tarih alanı boş bırakılamaz.")]
        public DateTime Date { get; set; }

        [Required]
        public string Explanation { get; set; }

        [Required]
        public int Payment { get; set; }
        
        

    }

    public class Outgoing
    {
        public PagedData<Models.Outgoings> ourOutgoings { get; set; }

        [Required]
        public int selectedSirketID { get; set; }
        public SelectList sirketler { get; set; }

        [Display(Name ="ID")]
        public int OutgoingID { get; set; }
        [Display(Name = "Şirket")]
        public Companies Companies { get; set; }

        [Required(ErrorMessage = "Tarih alanı boş bırakılamaz.")]
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Required]
        [Display(Name = "Miktar")]
        public int Payment { get; set; }
        
    }

}