using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjectSoft.Models
{
    public class Usuario
    {
        
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 2)]
        public string Nombre { get; set; }

         [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 2)]
        public string Apellido { get; set; }

         [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 10)]
        public string Direccion { get; set; }

         [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(10, ErrorMessage = "Debe tener 10 caracteres sin signos.", MinimumLength = 10)]
        public string Telefono { get; set; }

         [Required(ErrorMessage ="Este campo es obligatorio")]
        public string Email { get; set; }

        public string Estado { get; set; } = "Activo";

         [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 5)]
        public string UserAccount { get; set; } 

         [Required(ErrorMessage ="Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "La contraseña debe ser más larga (Minimo 6 Caracteres).", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public string Foto { get; set; }
    }
}
