using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.ViewModels
{
    public class Userprueba
    {


         
        public int Id { get; set; }
        
        public string Nombre { get; set; }
         
        public string Apellido { get; set; }
         
        public string Direccion { get; set; }
         
        public string Telefono { get; set; }
         
        public string Email { get; set; }
         
        public string User { get; set; }
         
        public string Password { get; set; }

        public IFormFile Foto { get; set; }

    }
}
