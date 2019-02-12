using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.Models
{
    public class LoginCredentials
    {

        [Required]
        public string UserAccount { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
