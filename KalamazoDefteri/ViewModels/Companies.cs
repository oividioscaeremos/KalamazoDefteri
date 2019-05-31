using KalamazoDefteri.Infrastructures;
using KalamazoDefteri.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.ViewModels
{
    public class CompaniesIndex
    {
        public PagedData<Companies> ourCompanies { get; set; }

        [Display(Name = "ID")]
        public string id { get; set; }
        [Display(Name = "Firma Adı")]
        public string companyName { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string phoneNumber { get; set; }

        [Display(Name = "Mevcut Bakiye")]
        public string balance { get; set; }
    }

    public class CompaniesViewOne
    {
        public Models.Companies ourCompany { get; set; }
    }

    public class CompaniesNew
    {
        [Required(ErrorMessage = "Firma adı boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string companyMame { get; set; }

        [Required(ErrorMessage = "Firma adresi boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefon Numarası boş bırakılamaz. (boş bırakmak için '-' ile doldurabilirsiniz.)")]
        public string phoneNumber { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Fax Numarası boş bırakılamaz. (boş bırakmak için '-' ile doldurabilirsiniz.)")]
        public string faxNumber { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "V.D. boş bırakılamaz. (boş bırakmak için '-' ile doldurabilirsiniz.)")]
        public string TaxAdministration { get; set; }

        [Required(ErrorMessage = "IBAN boş bırakılamaz. (boş bırakmak için '-' ile doldurabilirsiniz.)")]
        [DataType(DataType.Text)]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "Başlangıç için firmaya bir bakiye girmeniz gerekmekte. (Yok ise 0 (sıfır) girebilirsiniz.")]
        public int balance { get; set; }
    }
}