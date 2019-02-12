using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ProjectSoft.Models.Clientes
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Naturaleza { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 2)]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 5)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(10, ErrorMessage = "Se necesitan más caracteres.", MinimumLength = 10)]
        [Phone]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(10, ErrorMessage = "Se requieren minimo 10 caracteres", MinimumLength = 10)]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string TipoCliente { get; set; }

        public string Foto { get; set; }

        public string Estado { get; set; } = "Activo";

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaRegistro { get; set; } = Convert.ToDateTime(DateTime.Now.ToShortDateString());

    }
}
