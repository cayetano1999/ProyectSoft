using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ProjectSoft.Models
{
    public class UserSesion
    {
        [Key]
        public int Id { get; set; } //Id de la sesion
        public int UserId { get; set; } //Id del usuario
        public string MacAdress { get; set; } //Direccion Mac
        public string IsLogin { get; set; } //Esta logueado?

    }

}
