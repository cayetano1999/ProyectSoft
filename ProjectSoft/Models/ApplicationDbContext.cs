using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectSoft.Models.Clientes;
using ProjectSoft.Models.ProyectosModel;

namespace ProjectSoft.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
            ): base (options)
        {
                
        }

        //Perenecen a UserAccount
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserSesion> UserSesions { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

        //Pertenecen a Clientes
        public DbSet<Cliente> Cliente { get; set; }


        //Pertenece a Proyectos

        public DbSet<ProyectoInfo> ProyectoInfo { get; set; }
        public DbSet< ProyectoImg> ProyectoImg { get; set; }
        public DbSet<ProyectoFile> ProyectoFile { get; set; }
        


    }
}
