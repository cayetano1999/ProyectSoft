using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.Models.ProyectosModel
{
    public class ProyectoImg
    {
        [Key]
        public int Id { get; set; }

        public int Id_Proyecto { get; set; }
        
        public string Screen1 { get; set; }
        
        public string Screen2 { get; set; }
        
        public string Screen3 { get; set; }

        public string LogoProyecto { get; set; }

        [ForeignKey("Id_Proyecto")]
        public virtual ProyectoInfo ProyectoInfo { get; set; }

        



    }
}
