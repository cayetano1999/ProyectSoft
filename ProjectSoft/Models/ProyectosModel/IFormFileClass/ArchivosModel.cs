using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.Models.ProyectosModel.IFormFileClass
{
    public class ArchivosModel
    {

        public IFormFile LogoProyecto { get; set; }

        public IFormFile Screen1 { get; set; }

        public IFormFile Screen2 { get; set; }

        public IFormFile Screen3 { get; set; }

        public IFormFile Rar { get; set; }

        public IFormFile Contrato { get; set; }

    }
}
