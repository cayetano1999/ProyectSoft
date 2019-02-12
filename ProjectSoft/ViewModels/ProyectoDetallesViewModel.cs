using ProjectSoft.Models.ProyectosModel;
using ProjectSoft.Models.ProyectosModel.IFormFileClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.ViewModels
{
    public class ProyectoDetallesViewModel
    {

      
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Creador { get; set; }
        public string Cliente { get; set; }
        public double Costo { get; set; }
        public string Categoria { get; set; }
        public string UpLoader { get; set; }
        public string Duracion { get; set; }
        public string Descripcion { get; set; }
        public string Identificacion { get; set; }

        //img

        public string Screen1 { get; set; }

        public string Screen2 { get; set; }

        public string Screen3 { get; set; }

        public string LogoProyecto { get; set; }

        //file

        public string ArchivoRar { get; set; }

        public string Contrato { get; set; }


    }
}
