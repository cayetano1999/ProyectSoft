using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSoft.Models;
using ProjectSoft.wwwroot.Alertas;

namespace ProjectSoft.Controllers
{
    public class FuncionesGeneralesController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;

        public FuncionesGeneralesController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public  IActionResult NotLogin()
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser","Usuarios");
            }

            return View();
        }


       




        public  bool VerificarExistenciaUsuario(string usuario)
        {
            bool retornar = false;
            var user = _db.Usuario.Where(u => u.UserAccount == usuario).FirstOrDefault();
            if (user == null)
            {
                retornar = false;
            }
            else
            {
                retornar = true;
                Alerta(

                    "El usuario " + usuario + " ya existe, intenta con uno diferente",
                    "danger",
                    "remove"

                );
            }

            

            return retornar;
        }


        public bool VeririficarPesodeArchivo(IFormFile file)
        {
            bool retornar = false;
            if (file != null)
            {

                var ruta = file.FileName;
                string[] e = ruta.Split('.');
                if (file.Length > 3000000)
                {
                    retornar = true;
                    Alerta(
                        "La imagen excede el liminte de 3 MB",
                        "danger",
                        "remove"
                        );
                }

                else
                {
                    retornar = false;
                }


            }
            else
            {
                retornar = false;
            }
            return retornar;

        }


        public bool VerificarExtencion(IFormFile file)
        {
            bool retornar = false;
            if (file != null)
            {
                var ruta = file.FileName;
                string[] e = ruta.Split('.');
                string a = e[e.Length - 1].ToString().ToLower();

                if (a == "png" || a == "jpg" || a == "jpeg")
                {
                    retornar = false;

                }

                else
                {
                    retornar = true;
                    Alerta(

                        "El formato de la imagen no es valido",
                        "danger",
                        "remove"

                        );
                }

            }

            else
            {
                retornar = false;
            }
            return retornar;

        }

        public bool VerificarExistenciaEmail(string email)
        {
            bool retornar = false;
            var user = _db.Usuario.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                retornar = false;
            }
            else
            {
                retornar = true;
                AlertaGeneral.msj = "Ya existe un usuario con el correo " + email;
                AlertaGeneral.tipo = "danger";
            }

            return retornar;
        }


        public string SubirArchivo(IFormFile foto, string nombre)
        {
            string retornar = "LOGO.PNG";
            if (foto != null)
            {
                var nombrearchivo = nombre.ToLower(); ;
                var ruta = foto.FileName;
                string[] extencion = ruta.Split('.');

                var filename = Path.Combine(_he.WebRootPath + "/FotosPerfil", "Perfil" + "-" + nombrearchivo + "." + extencion[1]);

                if (System.IO.File.Exists(filename))
                {

                    System.IO.File.Delete(filename);

                }

                foto.CopyTo(new FileStream(filename, FileMode.Create));


                retornar = Path.GetFileName(filename);
            }

            return retornar;
        }

        public bool VerificarEspacio(string user)
        {
            bool retornar = false;
            string[] cadena = user.Split(' ');

            if (cadena.Length > 1)
            {
                retornar = true;
                Alerta(

                    "El usuario no puede contener espacios.",
                    "danger",
                    "remove"

                    );
            }

            return retornar;

        }


        public void Alerta(string msj, string tipo, string icono)
        {
            AlertaGeneral.msj = msj;
            AlertaGeneral.tipo = tipo;
            AlertaGeneral.icono = icono;


        }

    }
}