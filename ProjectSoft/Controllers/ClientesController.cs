using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoft.Models;
using ProjectSoft.Models.Clientes;
using ProjectSoft.ViewModels;

namespace ProjectSoft.Controllers
{
    public class ClientesController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;

        public ClientesController(ApplicationDbContext db,  IHostingEnvironment he)
        {
            _he = he;
            _db = db;


            


        }

        public IActionResult Index(string busqueda)
        {
            
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }
            else
            {
                var clientes = _db.Cliente.OrderByDescending(u => u.Id).Where(c=>c.Estado!="Eliminado").ToList();

                if (busqueda == null)
                {
                    return View(clientes);
                }
                else
                {

                    var cliente = _db.Cliente.Where(c => c.Nombre == busqueda).Where(c => c.Estado != "Eliminado").ToList();
                    if (cliente.Count()<1)
                    {
                        var cliente2 = _db.Cliente.Where(c => c.Email == busqueda).Where(c => c.Estado != "Eliminado").ToList();
                        return View(cliente2);
                    }
                    return View(cliente);
                    
                }



                
                
            }

            

            
        }

        public IActionResult RegistrarCliente()
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }
            else
            {
                return View();
            }


            
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(Cliente cliente, IFormFile img)
        {

            

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (Funciones.VerificarSelect(cliente.Naturaleza)==true || Funciones.VerificarSelect(cliente.TipoCliente) == true)
            {

                Funciones.Alerta(
                    
                    "Debes seleccionar la naturaleza y el tipo de Cliente",
                    "danger",
                    "remove"
                    
                    );
                return View();

            }

            if (Funciones.VerificarExistenciaEmailCliente(cliente.Email, _db) == true){


                return View();

            }

            if (Funciones.VerificarExistenciaIdentificacion(cliente.Identificacion, _db) == true)
            {
                return View();
            }

            if (ModelState.IsValid)
            {

                if (Funciones.VeririficarPesodeArchivo(img, 3, "Imagen Cliente" ) == false && Funciones.VerificarExtencion(img, "png", "jpg", "jpeg", "una Imagen", "Foto Cliente") == false) {

                    if (img != null)
                    {
                       cliente.Foto = Funciones.SubirArchivo(img, "Cliente-"+cliente.Identificacion, "FotosPerfilClientes", "Perfil-Cliente", _he);
                    }

                    cliente.Nombre = cliente.Nombre.Trim();
                    cliente.Email = cliente.Email.ToLower();
                    cliente.Identificacion = cliente.Identificacion.ToUpper();
                    _db.Add(cliente);
                    await _db.SaveChangesAsync();
                    Funciones.Alerta(

                        "El cliente "+cliente.Nombre+" fue agregado correctamente",
                        "success",
                        "check"

                        );
                    return RedirectToAction(nameof(RegistrarCliente));


                }
                else
                {
                    return View();
                }
                

            }
            else
            {
                Funciones.Alerta(

                    "Hay campos vacios",
                    "danger",
                    "remove"

                    );

                return View();
            }

            
            }





        //detalles de los clientes
        public IActionResult Detalles(int? Id)
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (Id == null)
            {
                return View("NotFound");
            }

            else
            {
                var cliente = _db.Cliente.Where(c => c.Id == Id).Where(c=>c.Estado!="Eliminado").FirstOrDefault();


                var clienteproyecto = new ClienteProyectosViewModel
                {
                    Cliente = _db.Cliente.Where(c => c.Id == Id).FirstOrDefault(),
                    ProyectoInfor = _db.ProyectoInfo.Where(p => p.Id_Cliente == Id).Where(pr=>pr.Estado!="Eliminado"). ToList(),
                    Categorias = _db.Categoria.ToList()
                };

                if (cliente != null)
                {
                    return View(clienteproyecto);
                }
                else
                {
                    return View("NotFound");
                }
                
                

            }
            

        }

        //Vista de para editar
        public IActionResult Editar(int? Id)
        {
            

            if (Id == null)
            {

                return View("NotFound");
            }

            var cliente = _db.Cliente.Where(c => c.Id == Id).FirstOrDefault();

            if (cliente != null)
            {
                return View(cliente);
            }

            else
            {
                return View("NotFound");
            }

            

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente cliente, IFormFile img)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (Funciones.VerificarSelect(cliente.Naturaleza) == true || Funciones.VerificarSelect(cliente.TipoCliente) == true)
            {

                Funciones.Alerta(

                    "Debes seleccionar la naturaleza y el tipo de Cliente",
                    "danger",
                    "remove"

                    );
                return View(cliente);

            }

            if (Funciones.VerificarExistenciaEmailEditando(cliente.Email, _db, cliente.Id) == true)
            {


                return View(cliente);

            }

            if (Funciones.VEIeditando(cliente.Identificacion, _db,cliente.Id) == true)
            {
                return View(cliente);
            }

            if (ModelState.IsValid)
            {
                if (Funciones.VeririficarPesodeArchivo(img, 3, "Imagen Cliente") == false && Funciones.VerificarExtencion(img, "png", "jpg", "jpeg", "una Imagen", "Foto Cliente") == false)
                {


                    var clienteeditar = await _db.Cliente.SingleOrDefaultAsync(c => c.Id == cliente.Id);

                    if (clienteeditar != null)
                    {
                        clienteeditar.Naturaleza = cliente.Naturaleza;
                        clienteeditar.Nombre = cliente.Nombre;
                        clienteeditar.Direccion = cliente.Direccion;
                        clienteeditar.Email = cliente.Email;
                        clienteeditar.Telefono = cliente.Telefono;
                        clienteeditar.TipoCliente = cliente.TipoCliente;
                        


                        if (img != null)
                        {
                            clienteeditar.Foto = Funciones.SubirArchivo(img, "Cliente-" + cliente.Identificacion, "FotosPerfilClientes", "Perfil",  _he);
                        }



                        await _db.SaveChangesAsync();
                        Funciones.Alerta(

                            "Cliente editado correctamente",
                            "success",
                            "check"

                            );
                        return RedirectToAction("Editar", new { clienteeditar.Id });
                    }
                    else
                    {
                        return View("NotFound");
                    }
                }

            }

            else
            {
                return View();
            }

            return View(nameof(Index));
        }


        public IActionResult Eliminar (int? Id)
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }


            if (Id == null)
            {
                return View("NotFound");
            }

            var clienteeliminar = _db.Cliente.Where(c => c.Id == Id).FirstOrDefault();

            if (clienteeliminar == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(clienteeliminar);
            }
        }



        [HttpPost]
        public async Task <IActionResult> Eliminar(Cliente cliente)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            var clienteeliminar = _db.Cliente.SingleOrDefault(c => c.Id == cliente.Id);
            if (clienteeliminar != null) {

            cliente.Estado = "Eliminado";
            await _db.SaveChangesAsync();
            

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("NotFound");
            }

        }


       


    }
}