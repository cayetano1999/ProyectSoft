using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoft.Models;
using ProjectSoft.Models.ProyectosModel;
using ProjectSoft.Models.UserAccount;
using ProjectSoft.ViewModels;

namespace ProjectSoft.Controllers
{
    public class ProyectosController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;


        public ProyectosController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        
        public IActionResult Index(string busqueda)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }



            //Objeto del viewModelo Proyecto
            var proyectos = new ProyectoViewModel
            {
                //asignando datos al view model

                proyectos = _db.ProyectoInfo.ToList(), //IEnumerable de proyectos
                Listaclientes = _db.Cliente.ToList(), //Lista de clientes que aparecen en el dropdown
                Listacategorias = _db.Categoria.ToList(),//Lista de las categorias que aparecen en el dropdown
                ListaProyectoImgs = _db.ProyectoImg.ToList(),// Lista de imagenes del los proyectos
                ListaProyectoFiles = _db.ProyectoFile.ToList()// Lista de archivos de los proyectos



            };

            //Si hay una busqueda el se retorna el proyecto a buscar 
            if (busqueda != null)
            {
                var proyecto = new ProyectoViewModel
                {
                    
                    proyectos = _db.ProyectoInfo.Where(p => p.Nombre == busqueda).ToList(),//Buscando el proyecto
                    Listaclientes = _db.Cliente.ToList(),
                    Listacategorias = _db.Categoria.ToList(),
                    ListaProyectoImgs = _db.ProyectoImg.ToList(),
                    ListaProyectoFiles = _db.ProyectoFile.ToList()



                };

                return View(proyecto); //retornando el proyecto

            }
            else
            {
                return View(proyectos);
            }
        }




        //Muestra las categorias de los proyectos
        public IActionResult Categoria()
        {
            //si el usuario esta logueado
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            //Objeto de categoria
            var categoria = new CategoriaViewModel
            {

                Categorias = _db.Categoria.Where(c=>c.Estado!="Eliminada").ToList()
            };

            return View(categoria);
        }




        //Agregando las categorias a la DB
        [HttpPost]
        public async Task<IActionResult> Categoria(CategoriaViewModel categoria)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            //Si el modelo es valido
            if (ModelState.IsValid)
            {

                categoria.categoriaobj.Usuario = LoadDataUser.UserAccount; //Asignado valor a la UpLoader con el usuario logueado
                _db.Categoria.Add(categoria.categoriaobj);//Agregando la categoria
                await _db.SaveChangesAsync(); //Guardando los cambios
                Funciones.Alerta(
                    "Categoria agregada",
                    "success",
                    "check"

                    ); //Preparando la alerta
                return RedirectToAction(nameof(Categoria)); //redireccionando a las categorias

            }
            else
            {
                //Si el modelo no es valido 
                Funciones.Alerta(
                   "Debes introducir el nombre de la categoria",
                   "danger",
                   "remove"

                   );

                return View();
            }



        }



        public async Task<IActionResult> EliminarCategoria(int? Id)
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
                var categoriaeliminar = _db.Categoria.SingleOrDefault(c => c.Id == Id);
                if (categoriaeliminar == null)
                {
                    return View("NotFound");
                }
                else
                {
                    categoriaeliminar.Estado = "Eliminada";
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Categoria));
                }

            }

        }


        public IActionResult EditarCategoria(int? Id)
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
                var categoriaeditar = _db.Categoria.Where(c => c.Id == Id).FirstOrDefault();

                if (categoriaeditar == null)
                {
                    return View("NotFound");
                }
                else
                {
                    return View(categoriaeditar);
                }

            }




        }


        [HttpPost]
        public async Task<IActionResult> EditarCategoria(Categoria categoria)
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("UserLogin", "Usuarios");
            }


            if (ModelState.IsValid)
            {

                var categoriaeditar = _db.Categoria.SingleOrDefault(c => c.Id == categoria.Id);

                categoriaeditar.Nombre = categoria.Nombre;
                categoriaeditar.Usuario = LoadDataUser.UserAccount;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Categoria));


            }

            else
            {
                return View();
            }



        }


        //Proyectos



        //Crear Proyecto View

        public IActionResult CrearProyecto()
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }


            var model = new ProyectoViewModel
            {
                Listacategorias = _db.Categoria.Where(cat=>cat.Estado!="Eliminada").ToList(),
                Listaclientes = _db.Cliente.Where(c=>c.Estado!="Eliminado").ToList()



            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> CrearProyecto(ProyectoViewModel proyecto,
            IFormFile LogoProyecto, IFormFile Screen1, IFormFile Screen2, IFormFile Screen3,
            IFormFile ArchivoRar, IFormFile Contrato)
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }




            if (proyecto.ProyectoInfo.Costo < 1000)
            {

                Funciones.Alerta(
                    
                    "El costo minimo es de 1000 RD$",
                    "danger",
                    "remove"
                    
                    );

                var model = new ProyectoViewModel
                {
                    Listacategorias = _db.Categoria.ToList(),
                    Listaclientes = _db.Cliente.ToList()



                };

                return View(model);
                
            }

            if (VerificadorProyecto(proyecto.ProyectoInfo.Id_Categoria, proyecto.ProyectoInfo.Id_Cliente, proyecto.ProyectoInfo.Identificacion,
                Screen1, Screen2, Screen3, LogoProyecto, ArchivoRar, Contrato) == false)
            {

                var model = new ProyectoViewModel
                {
                    Listacategorias = _db.Categoria.ToList(),
                    Listaclientes = _db.Cliente.ToList()



                };

                return View(model);

                
            }



            if (ModelState.IsValid)
            {

                var infoproyecto = new ProyectoInfo
                {
                    Nombre = proyecto.ProyectoInfo.Nombre,
                    Creador = proyecto.ProyectoInfo.Creador,
                    Id_Cliente = proyecto.ProyectoInfo.Id_Cliente,
                    Duracion = proyecto.ProyectoInfo.Duracion,
                    Costo = proyecto.ProyectoInfo.Costo,
                    Id_Categoria = proyecto.ProyectoInfo.Id_Categoria,
                    UpLoader = LoadDataUser.UserAccount,
                    Descripcion = proyecto.ProyectoInfo.Descripcion,
                    Identificacion = proyecto.ProyectoInfo.Identificacion





                };

                _db.ProyectoInfo.Add(infoproyecto);
                await _db.SaveChangesAsync();
                var Idproyecto = _db.ProyectoInfo.Where(p => p.Identificacion == proyecto.ProyectoInfo.Identificacion).FirstOrDefault();
                var imgproyecto = new ProyectoImg
                {
                    Id_Proyecto = Idproyecto.Id,
                    LogoProyecto = Funciones.SubirArchivo(LogoProyecto, "logo-" + proyecto.ProyectoInfo.Identificacion, "FotosProyectos", "Logo", _he),
                    Screen1 = Funciones.SubirArchivo(Screen1, "screen1-" + proyecto.ProyectoInfo.Identificacion, "FotosProyectos", "screen", _he),
                    Screen2 = Funciones.SubirArchivo(Screen2, "screen2-" + proyecto.ProyectoInfo.Identificacion, "FotosProyectos", "screen", _he),
                    Screen3 = Funciones.SubirArchivo(Screen3, "screen3-" + proyecto.ProyectoInfo.Identificacion, "FotosProyectos", "screen", _he),

                };

                _db.ProyectoImg.Add(imgproyecto);
                await _db.SaveChangesAsync();

                var archivoproyecto = new ProyectoFile
                {

                    Id_Proyecto = Idproyecto.Id,
                    ArchivoRar = Funciones.SubirArchivo(ArchivoRar, "rarproyecto-" + proyecto.ProyectoInfo.Identificacion, "Proyectos", "RarFile", _he),
                    Contrato = Funciones.SubirArchivo(Contrato, "contrato-" + proyecto.ProyectoInfo.Identificacion, "Contratos", "Contrato", _he),

                };

                _db.ProyectoFile.Add(archivoproyecto);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            else
            {
                var model = new ProyectoViewModel
                {
                    Listacategorias = _db.Categoria.ToList(),
                    Listaclientes = _db.Cliente.ToList()
                };



                return View(model);
            }


        }



       public IActionResult Editar(int? Id)
        {
            if(UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }
            
            if (Id == null)
            {
                return View("NotFound");
            }

            else
            {

                var proyecto = new ProyectoViewModel
                {

                    ProyectoInfo = _db.ProyectoInfo.Where(p => p.Id == Id).FirstOrDefault(),
                    Listacategorias = _db.Categoria.ToList(),
                    Listaclientes = _db.Cliente.ToList()


                };

                return View(proyecto);

            }

            

        } 




        [HttpPost]
    
        public async Task<IActionResult> Editar(ProyectoViewModel proyecto, IFormFile LogoProyecto, IFormFile Screen1, IFormFile Screen2, IFormFile Screen3,
            IFormFile ArchivoRar, IFormFile Contrato)
        {

            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (!ModelState.IsValid)
            {

                var proyectomodel = new ProyectoViewModel
                {

                    ProyectoInfo = proyecto.ProyectoInfo,
                    Listacategorias = _db.Categoria.ToList(),
                    Listaclientes = _db.Cliente.ToList()


                };

                return View(proyectomodel);
            }
            else
            {

                if (LogoProyecto != null)
                {
                    

                }


            }

            return View();


        }


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



                var proyectoinfo = _db.ProyectoInfo.Where(p => p.Id == Id).FirstOrDefault(); 
                var proyectoimg = _db.ProyectoImg.Where(p => p.Id_Proyecto == Id).FirstOrDefault();
                var proyectofile = _db.ProyectoFile.Where(p => p.Id_Proyecto == Id).FirstOrDefault();

                if(proyectoinfo==null || proyectoimg == null || proyectofile == null || proyectoinfo.Estado=="Eliminado")
                {
                    return View("NotFound");
                }

                else
                {
                    var categ = _db.ProyectoInfo.Where(c=> c.Id == Id).FirstOrDefault();
                    var categoria = _db.Categoria.Where(ca => ca.Id == categ.Id_Categoria).FirstOrDefault();

                    var cliente = _db.ProyectoInfo.Where(c => c.Id == Id).FirstOrDefault();
                    var clientename = _db.Cliente.Where(cl => cl.Id == cliente.Id_Cliente).FirstOrDefault();
                    var detalle = new ProyectoViewModel
                    {
                        ProyectoInfo = _db.ProyectoInfo.Where(p => p.Id == Id).FirstOrDefault(),
                        ProyectoImg = _db.ProyectoImg.Where(p => p.Id_Proyecto == Id).FirstOrDefault(),
                        ProyectoFile = _db.ProyectoFile.Where(p => p.Id_Proyecto == Id).FirstOrDefault(),
                        Categoria = categoria.Nombre,
                        Cliente = clientename.Nombre

                        

                        
                        

                    };

                    return View(detalle);

                }


            }


            
        }


        public async Task <IActionResult> Eliminar(int? Id)
        {
            //Los proyectos no se eliminan completamente, se coloca el estado ELIMINADO y no se muestra en la tabla de proyectos

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

                var proyectoeliminar = _db.ProyectoInfo.SingleOrDefault(p => p.Id == Id);
                if(proyectoeliminar == null)
                {
                    return View("NotFound");
                }

                else
                {
                    proyectoeliminar.Estado = "Eliminado";
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));


                }

            }


        }
        

        public IActionResult ProyectosEliminados()
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

           
            var eliminados = _db.ProyectoInfo.Where(p => p.Estado == "Eliminado").ToList();

            return View(eliminados);

        }

        public async Task<IActionResult>Restaurar(int? Id)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }
            try
            {
                var proyectoseliminados = _db.ProyectoInfo.Where(p => p.Estado == "Eliminado").ToList();
                if (Id == null)
                {
                    return View("NotFound");
                }
                else
                {

                    var proyectorestaurar = _db.ProyectoInfo.SingleOrDefault(p => p.Id == Id);
                    if (proyectorestaurar != null)
                    {

                        if (proyectorestaurar.Estado == "Eliminado")
                        {
                            proyectorestaurar.Estado = "Activo";
                            await _db.SaveChangesAsync();
                            Funciones.Alerta("El proyecto -" + proyectorestaurar.Nombre + "- ha sido restaurado", "success", "check");
                            
                            return RedirectToAction("ProyectosEliminados", proyectoseliminados);

                        }

                    }
                    else
                    {
                        return View("NotFoud");
                    }

                }

                return View("NotFound");
            }
            catch (NullReferenceException x)
            {
               
                return View("NotFoud");
            }

        }


        public IActionResult DescargarProyecto(string archivo)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (archivo == null)
            {
                return View("NotFound");
            }
            else
            {

                var proyectoadescargar = new ProyectoFile
                {
                    ArchivoRar = archivo

                };

                return View(proyectoadescargar);


            }

        }


        public IActionResult Vercontrato(string archivo)
        {
            if (UserLogin.Islogin == false)
            {
                return RedirectToAction("LoginUser", "Usuarios");
            }

            if (archivo == null)
            {
                return View("NotFound");
            }
            else
            {

                var contrato = new ProyectoFile
                {
                    Contrato = archivo

                };

                return View(contrato);


            }

        }

        

        

      
        private bool VerificadorProyecto(
            int? categoria, int? cliente, string identificacion,
            IFormFile scren1, IFormFile scren2, IFormFile scren3, IFormFile Logo,
            IFormFile rar, IFormFile contrato


            )

        {
            bool retornar = true;

            if(scren1 == null || scren2 == null || scren3 == null || Logo == null || rar ==null || contrato == null)
            {

                Funciones.Alerta(
                    
                    
                    "Todos los archivos son obligatorios, por favor, coloque los archivos correspondientes",
                    "danger",
                    "remove"


                    );

                retornar = false;
            }


            if (categoria==0 || cliente==0)
            {

                Funciones.Alerta(

                    "Debes seleccionar el cliente y la categoria",
                    "danger",
                    "remove"

                    );
                retornar = false;

            }

            if (Funciones.VerificarIdentificacionProyecto(identificacion, _db) == true)
            {
                retornar = false;

            }

            if (Funciones.VeririficarPesodeArchivo(scren1, 3, "Screen 1") == true ||
                Funciones.VeririficarPesodeArchivo(scren2, 3, "Screen 2") == true ||
                Funciones.VeririficarPesodeArchivo(scren3, 3, "Screen 3") == true ||
                Funciones.VeririficarPesodeArchivo(Logo, 3, "Logo") == true ||
                Funciones.VeririficarPesodeArchivo(contrato, 10, "Contrato") == true ||
                Funciones.VeririficarPesodeArchivo(rar, 50, "Archivo rar") == true

                )

            {
                retornar = false;
            }

            

           

            if (Funciones.VerificarExtencion(scren1, "png", "jpg", "jpeg", "una Imagen", "Screen 1") == true ||
                Funciones.VerificarExtencion(Logo, "png", "jpg", "jpeg", "una Imagen", "Logo Proyecto") == true ||
                Funciones.VerificarExtencion(scren2, "png", "jpg", "jpeg", "una Imagen", "Screen 2") == true ||
                Funciones.VerificarExtencion(scren3, "png", "jpg", "jpeg", "una Imagen", "Screen 3") == true ||
                Funciones.VerificarExtencion(rar, "rar", "zip", "rar", " un archivo .rar", "Archivo .rar") == true ||
                Funciones.VerificarExtencion(contrato, "pdf", "pdf", "pdf", "un .pdf","Contrato") == true 
               


                )
            {
                retornar = false;
            }




            return retornar;






        }


    }
}