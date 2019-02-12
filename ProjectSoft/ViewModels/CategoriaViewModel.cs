using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSoft.Models;
namespace ProjectSoft.ViewModels
{
    public class CategoriaViewModel
    {


        public Categoria categoriaobj { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }


    }
}
