using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.ViewModels
{
    public class AuthRegister
    {
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        [DisplayName("E-Mail Adresi")]
        public string email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Şifre")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Şifre Tekrar")]
        [Compare("password", ErrorMessage = "Girdiğiniz şifreler birbirleriyle uyuşmuyor.")]
        public string passwordconfirm { get; set; }

    }

    public class AuthLogin
    {
        [DataType(DataType.Text)]
        [Required]
        public string username { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }

    }
}