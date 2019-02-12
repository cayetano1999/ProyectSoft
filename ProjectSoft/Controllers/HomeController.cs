using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSoft.Models;
using ProjectSoft.Models.UserAccount;

namespace ProjectSoft.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            var userlogueado = _db.UserSesions.Where(
                us => us.MacAdress == Funciones.GetMacAdres().ToString()).
                Where(u=>u.IsLogin=="yes").FirstOrDefault();

            if (userlogueado!=null){

                var IdUser = userlogueado.UserId;

                var userlogin = _db.Usuario.Where(u => u.Id == IdUser).FirstOrDefault();

                var rol = _db.UserRole.Where(u => u.UserId == userlogin.Id).FirstOrDefault();
                LoadDataUser.Id = userlogin.Id;
                LoadDataUser.Nombre = userlogin.Nombre;
                LoadDataUser.Apellido = userlogin.Apellido;
                LoadDataUser.UserAccount = userlogin.UserAccount;
                LoadDataUser.Email = userlogin.Email;
                LoadDataUser.Foto = userlogin.Foto;
                LoadDataUser.Rol = rol.Rol;
                UserLogin.Islogin = true;

                return RedirectToAction("Index","Clientes");

            }

            else
            {

                UserLogin.Islogin = false;
                return View();


            }

            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
