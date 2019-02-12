using ProjectSoft.Models;
using ProjectSoft.Models.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSoft.Models.ProyectosModel;
using ProjectSoft.Models.ProyectosModel.IFormFileClass;

namespace ProjectSoft.ViewModels
{
    public class ProyectoViewModel
    {

        public ProyectoInfo ProyectoInfo { get; set; }
        public ProyectoImg ProyectoImg { get; set; }
        public ProyectoFile ProyectoFile { get; set; }
        public ArchivosModel ArchivosModel { get; set; }


        public List<Categoria> Listacategorias { get; set; }
        public List<Cliente> Listaclientes { get; set; }
        public List<ProyectoImg> Listaimg { get; set; }



        public IEnumerable<ProyectoInfo> proyectos { get; set; }
        public IEnumerable<ProyectoFile> ListaProyectoFiles { get; set; }
        public IEnumerable<ProyectoImg> ListaProyectoImgs { get; set; }

        public string Categoria { get; set; }
        public string  Cliente { get; set; }

    }
}
