using ProjectSoft.Models;
using ProjectSoft.Models.Clientes;
using ProjectSoft.Models.ProyectosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.ViewModels
{
    public class ClienteProyectosViewModel
    {
        public Cliente Cliente { get; set; }
        
        public IEnumerable<ProyectoInfo> ProyectoInfor { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

    }
}
