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

    public class CurrentUserView
    {
        /*
         public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string adSoyad { get; set; }
        public virtual string addressMah { get; set; }
        public virtual string addRessCadSk { get; set; }
        public virtual string addressIl { get; set; }
        public virtual string addressIlce { get; set; }
        public virtual int Balance { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual IList<Roles> Roles { get; set; }
             */
        public int id;
        [Display(Name = "Kullanıcı Adı")]
        public string username { get; set; }

        [Display(Name = "E-Mail")]
        public string email { get; set; }

        [Display(Name = "Ad Soyad")]
        public string adsoyad { get; set; }

        [Display(Name = "Mahalle")]
        public string adresMah { get; set; }

        [Display(Name = "Cadde/Sokak")]
        public string adresCadSk { get; set; }

        [Display(Name = "ŞEHİR")]
        public string adresIl { get; set; }

        [Display(Name = "İLÇE")]
        public string adresIlce { get; set; }

        [Display(Name = "MEVCUT NET BAKİYE")]
        public int balance { get; set; }
    }

    public class PrintReportViewModel
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "ŞİRKET ADI")]
        public string CompanyName { get; set; }
        [Display(Name = "TARİH")]
        public DateTime Date { get; set; }
        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }
        [Display(Name = "ÖDEME")]
        public int Payment { get; set; }

        public IEnumerable<Incomings> allIncomings { get; set; }
        public IEnumerable<Outgoings> allOutGoings { get; set; }

        public User currUser { get; set; }
        [Display(Name = "BASLANGIC:")]
        public DateTime reportFrom { get; set; }
        [Display(Name = "BITIS:")]
        public DateTime reportTo { get; set; }
    }
}