using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.ViewModels
{
    public class AuthLogin
    {
        [DataType(DataType.Text)]
        [Required]
        
        public string username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }

    }
}