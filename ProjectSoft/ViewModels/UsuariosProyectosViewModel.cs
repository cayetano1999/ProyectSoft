using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSoft.Models;
using ProjectSoft.Models.ProyectosModel;

namespace ProjectSoft.ViewModels
{
    public class UsuariosProyectosViewModel
    {
        public Usuario Usuario { get; set; }
        public List<Categoria> Listacategorias { get; set; }
        public IEnumerable<ProyectoInfo> ProyectoInfos { get; set; }

    }
}
