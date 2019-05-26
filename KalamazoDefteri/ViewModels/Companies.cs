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
        public string phoneNumber { get; set; }
        [DataType(DataType.Text)]
        public string faxNumber { get; set; }
        [DataType(DataType.Text)]
        public string TaxAdministration { get; set; }
        [DataType(DataType.Text)]
        public string IBAN { get; set; }
        [Required(ErrorMessage = "Başlangıç için firmaya bir bakiye girmeniz gerekmekte. (Yok ise 0 (sıfır) girebilirsiniz.")]
        public int balance { get; set; }
    }
}