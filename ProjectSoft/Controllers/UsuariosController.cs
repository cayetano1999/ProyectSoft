using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSoft.Models;
using ProjectSoft.Models.UserAccount;
using ProjectSoft.ViewModels;
using ProjectSoft.wwwroot.Alertas;

namespace ProjectSoft.Controllers
{
    public class UsuariosController : Controller
    {

        private ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;
        
        public UsuariosController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _he = he;
            _db = db;
            



        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AdmUsuarios() {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            else if (UserLogin.Islogin == true && LoadDataUser.Rol !="Admin")
            {
                return RedirectToAction("Index", "Proyectos");
            }

            else if (UserLogin.Islogin == true && LoadDataUser.Rol == "Admin")
            {
                var usuarios = new UsuariosViewModel
                {

                    Usuarios = _db.Usuario.ToList()

                };


                return View(usuarios);
            }
            else
            {
                return View();
            }




              
        }


        public IActionResult Registrar()
        {
            if(UserLogin.Islogin==true && LoadDataUser.Rol == "Admin")
            {
                return View(nameof(Registrar));
            }
            else
            {
                return View(nameof(Registrar));
            }

            
        }

        
        public IActionResult LoginUser(LoginCredentials lc)
        {
            if (UserLogin.Islogin == true )
            {
                return RedirectToAction("Index", "Proyectos");
            }
            


            if (ModelState.IsValid)
            {
                var lc2 = new LoginCredentials
                {
                    UserAccount = lc.UserAccount
                };

                return View(lc2);
            }
            else
            {
                return View();
            }
            
        }

        //me queded aqui... tengo que solo pasar el email 

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario, IFormFile img, string rol)

        {
            

            if (ModelState.IsValid)
            {
                
                var userpass = usuario.Password;

                if (VerificarExistenciaUsuario(usuario.UserAccount) == false &&
                    Funciones.VerificarExistenciaEmail(usuario.Email, _db) == false &&
                    Funciones.VeririficarPesodeArchivo(img, 3, "Imagen usuario") == false &&
                    Funciones.VerificarExtencion(img, "png", "jpg", "jpeg", "una Imagen", "Foto Usuario") == false &&
                    VerificarEspacio(usuario.UserAccount) == false

                    ) 
                {
                    if (img != null)
                    {
                        usuario.Foto = Funciones.SubirArchivo(img, usuario.UserAccount, "FotosPerfil", "screen", _he);
                    }
                    else
                    {
                        usuario.Foto = "Userdefault.png";
                    }

                    usuario.Password = Funciones.CreateMD5(usuario.Password);
                    usuario.UserAccount = usuario.UserAccount.ToLower();
                    usuario.UserAccount = usuario.UserAccount.Replace(' ','-');
                    _db.Usuario.Add(usuario);
                    await _db.SaveChangesAsync();
                    var userdata = _db.Usuario.Where(u => u.UserAccount == usuario.UserAccount).FirstOrDefault();
                    var Rol = new UserRole
                    {
                         UserId = userdata.Id,
                         Rol = rol
                    };

                    _db.Add(Rol);
                    await _db.SaveChangesAsync();

                    var Usersesion = new UserSesion
                    {
                        IsLogin = "yes",
                        UserId = userdata.Id,
                        MacAdress = Funciones.GetMacAdres().ToString()
                    };
                    _db.Add(Usersesion);
                    await _db.SaveChangesAsync();

                    Funciones.Alerta(
                        "Usuario registrado exitosamente.",
                        "success",
                        "check"
                        );

                    if (UserLogin.Islogin == false) { 

                    return RedirectToAction("LoginUser", 
                        new LoginCredentials{ UserAccount = usuario.UserAccount
                        

                        });
                    }
                    else
                    {
                        return RedirectToAction(nameof(AdmUsuarios));
                    }
                }
                else
                {
                    
                }
            }

            else
            {

               return View(usuario);
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginCredentials lc)
        {

            if (ModelState.IsValid)
            {
                //convirtiendo la contrase;a a md5
                var passmd5 = Funciones.CreateMD5(lc.Password);

                //consulta para saber si el usuario existe
                var userlogin = _db.Usuario.Where(u => u.UserAccount == lc.UserAccount).
                    Where(u => u.Password == passmd5).FirstOrDefault();

                //condicion si el usuario existe
                if (userlogin!=null)
                {
                    //cargando la informacion del usuario en la clase estatica
                   var rol= _db.UserRole.Where(u => u.UserId == userlogin.Id).FirstOrDefault();
                    LoadDataUser.Id = userlogin.Id;
                    LoadDataUser.Nombre = userlogin.Nombre;
                    LoadDataUser.Apellido = userlogin.Apellido;
                    LoadDataUser.UserAccount = userlogin.UserAccount;
                    LoadDataUser.Email = userlogin.Email;
                    LoadDataUser.Foto = userlogin.Foto;
                    LoadDataUser.Rol = rol.Rol;
                    LoadDataUser.Estado = userlogin.Estado;
                    UserLogin.Islogin = true;
                    
                    //asigando valor a las propiedades de inicio de sesion
                    var usersesion = new UserSesion
                    {
                        MacAdress = Funciones.GetMacAdres(),
                        UserId = userlogin.Id,
                        IsLogin = "yes"
                    };

                    //comprobando si ya el usuario esta registrado en la tabla de sesiones
                    var us = _db.UserSesions.Where(s => s.UserId == userlogin.Id).FirstOrDefault();

                    //si esta registrado en la tabla sesiones
                    if (us != null)
                    {
                        us.MacAdress = Funciones.GetMacAdres();
                        us.UserId = userlogin.Id;
                        us.IsLogin = "yes";

                        _db.Update(us);
                        await _db.SaveChangesAsync();
                    }
                    //si no esta registrado en la tabla sesionens
                    else
                    {
                        //agregalo a la tabla sesiones
                        _db.Add(usersesion);
                       await _db.SaveChangesAsync();
                    }

                    //redireccionado al inicio
                    return RedirectToAction("Index","Proyectos");
                }

                //si el usuario no existe
                else
                {
                    Funciones.Alerta(

                        "Usuario o Contraseña no validos",
                        "danger",
                        "remove"
                        
                        );

                    //retornando el modelo
                    return RedirectToAction("LoginUser",
                        new LoginCredentials
                        {
                            UserAccount = lc.UserAccount


                        });

                }

               
                

            }

            //si el modelo no es valido
            else
            {

                Funciones.Alerta(

                    "Introduce el usuario y la contraseña",
                    "danger",
                    "remove"

                    );

                return RedirectToAction("LoginUser",
                        new LoginCredentials
                        {
                            UserAccount = lc.UserAccount


                        });
            }

            

        }
        
        public async Task<IActionResult> Logout(int? Id)
        {

            try
            {
                if (UserLogin.Islogin == true && LoadDataUser.Id == Id)
                {

                    if (Id == null)
                    {
                        return RedirectToAction(nameof(Registrar));
                    }

                    var userlogueado = _db.UserSesions.Where(
                    us => us.MacAdress == Funciones.GetMacAdres().ToString()).
                    Where(u => u.IsLogin == "yes").FirstOrDefault();


                    if (userlogueado != null)
                    {
                        userlogueado.IsLogin = "no";
                        UserLogin.Islogin = false;
                        _db.Update(userlogueado);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(LoginUser));

                    }
                }

                else
                {
                    return RedirectToAction("Index", "Home");

                }

                UserLogin.Islogin = false;
                return RedirectToAction("LoginUser", "Usuarios");
            }
            catch(InvalidOperationException x)
            {
                UserLogin.Islogin = false;
                return View("LoginUser", "Usuarios");
                
            }

        }


        public IActionResult Detalles(int? Id)
        {

            
            if (Id == null)
            {
                return View("NotFound");
            }
            else {

                var user = _db.Usuario.Where(u => u.Id == Id).FirstOrDefault();

                if (user == null)
                {
                    return View("NotFound");
                }

                var detallesuser = new UsuariosProyectosViewModel
                {
                    Usuario = _db.Usuario.Where(u => u.Id == Id).FirstOrDefault(),
                    ProyectoInfos = _db.ProyectoInfo.Where(p => p.UpLoader == user.UserAccount).ToList(),
                    Listacategorias = _db.Categoria.ToList()
                };

                return View(detallesuser);
            }
        }

        public IActionResult Editar (int? Id)
        {
            if (UserLogin.Islogin == true && LoadDataUser.Rol !="Admin")
            {
                return RedirectToAction("Index","Proyectos");
            }

            if (Id == null)
            {
                return View("NotFound");
            }

            else
            {
                var usuario = _db.Usuario.Where(u=>u.Id== Id).FirstOrDefault();
                if (usuario != null)
                {
                    return View(usuario);
                }

                else
                {
                    return View("NotFound");
                }
            }



        }

        private bool VerificarExistenciaUsuario(string usuario)
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
                Funciones.Alerta(

                    "El usuario " + usuario + " ya existe, intenta con uno diferente",
                    "danger",
                    "remove"

                );
            }

            return retornar;
        }

        private bool VerificarEspacio(string user)
        {
            bool retornar = false;
            string[] cadena = user.Split(' ');

            if (cadena.Length > 1)
            {
                retornar = true;
               Funciones.Alerta(

                    "El usuario no puede contener espacios.",
                    "danger",
                    "remove"

                    );
            }

            return retornar;

        }






    }



}