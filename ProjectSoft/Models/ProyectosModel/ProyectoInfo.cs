using ProjectSoft.Models.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.Models.ProyectosModel
{
    public class ProyectoInfo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 2)]
        public string Creador { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Cliente { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        public double Costo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Categoria { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string  UpLoader { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Duracion { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 30)]
        public string  Descripcion { get; set; }

        
        [Required(ErrorMessage = "La identificación debe tener 10 caracteres")]
        [StringLength(10, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 10)]
        public string Identificacion { get; set; }


        public string Estado { get; set; } = "Activo";

        [ForeignKey("Id_Cliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("Id_Categoria")]
        public virtual Categoria Categoria { get; set; }

        public virtual ProyectoImg ProyectoImg { get; set; }

        public virtual ProyectoFile ProyectoFl { get; set; }
    }
}
