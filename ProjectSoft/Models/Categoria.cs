using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSoft.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre  { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Fecha { get; set; } = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        public string Usuario { get; set; }
        public string Estado { get; set; } = "Activa";
    }
}
