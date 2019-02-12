using ProjectSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.ViewModels
{
    public class UsuariosViewModel
    {

        public Usuario Usuario { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }
        


    }
}
