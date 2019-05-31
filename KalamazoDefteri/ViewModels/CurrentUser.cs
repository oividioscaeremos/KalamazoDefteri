using KalamazoDefteri.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.ViewModels
{
    public class CurrentUser
    {
        public int id;
    }

    public class PrintReportViewModel
    {
        [Display(Name ="ID")]
        public int id { get; set; }
        [Display(Name ="ŞİRKET ADI")]
        public string CompanyName { get; set; }
        [Display(Name ="TARİH")]
        public DateTime Date { get; set; }
        [Display(Name ="Açıklama")]
        public string Explanation { get; set; }
        [Display(Name ="ÖDEME")]
        public int Payment { get; set; }

        public IEnumerable<Incomings> allIncomings { get; set; }
        public IEnumerable<Outgoings> allOutGoings { get; set; }

        public User currUser { get; set; }
        [Display(Name ="BASLANGIC:")]
        public DateTime reportFrom { get; set; }
        [Display(Name = "BITIS:")]
        public DateTime reportTo { get; set; }
    }
}