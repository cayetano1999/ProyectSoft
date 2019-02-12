using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProjectSoft.wwwroot.Alertas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSoft.Models
{
    public class Funciones
    {
        //Convierte la contrase;a a MD5
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


        //Obtiene la direccion mac del dispositivo
        public static string GetMacAdres()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return mac;

        }


        public static bool VerificarExistenciaEmail(string email, ApplicationDbContext _db)
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



        public static bool VerificarExistenciaEmailCliente(string email, ApplicationDbContext _db)
        {
            bool retornar = false;
            var user = _db.Cliente.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                retornar = false;
            }
            else
            {
                retornar = true;
                AlertaGeneral.msj = "Ya existe un cliente con el correo " + email;
                AlertaGeneral.tipo = "danger";
            }

            return retornar;
        }





        public static bool VerificarExistenciaEmailEditando(string email, ApplicationDbContext _db, int Id)
        {
            bool retornar = false;
            var user = _db.Cliente.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                retornar = false;

            }

            else if (user.Id == Id)
            {
                retornar = false;
            }

            else if (user.Id != Id)
            {

                retornar = true;
                AlertaGeneral.msj = "Ya existe un cliente con el correo " + email;
                AlertaGeneral.tipo = "danger";
            }



            return retornar;
        }


        public static bool VEIeditando(string identificacion, ApplicationDbContext _db, int Id)
        {
            bool retornar = false;
            var user = _db.Cliente.Where(u => u.Identificacion == identificacion).FirstOrDefault();
            if (user == null)
            {
                retornar = false;

            }

            else if (user.Id == Id)
            {
                retornar = false;
            }

            else if (user.Id != Id)
            {

                retornar = true;
                AlertaGeneral.msj = "El cliente [" + user.Nombre.ToUpper() + "] posee la idenntificación [ " + identificacion + "], Intenta con una diferente";
                AlertaGeneral.tipo = "danger";
            }

            return retornar;
        }

        public static bool VerificarExistenciaIdentificacion(string identificacion, ApplicationDbContext _db)
        {
            bool retornar = false;
            var user = _db.Cliente.Where(u => u.Identificacion == identificacion).FirstOrDefault();
            if (user == null)
            {
                retornar = false;
            }
            else
            {
                retornar = true;
                AlertaGeneral.msj = "El cliente [" + user.Nombre.ToUpper() + "] posee la idenntificación [ " + identificacion + "], Intenta con una diferente";
                AlertaGeneral.tipo = "danger";
            }

            return retornar;
        }


        public static bool VerificarIdentificacionProyecto(string identificacion, ApplicationDbContext _db)
        {
            bool retornar = false;
            var proyecto = _db.ProyectoInfo.Where(u => u.Identificacion == identificacion).FirstOrDefault();
            if (proyecto == null)
            {
                retornar = false;
            }
            else
            {
                retornar = true;
                AlertaGeneral.msj = "El proyecto [" + proyecto.Nombre.ToUpper() + "] posee la idenntificación [ " + identificacion + "], Intenta con una diferente";
                AlertaGeneral.tipo = "danger";
            }

            return retornar;
        }




        public static bool VeririficarPesodeArchivo(IFormFile file, long SizeMax, string archivo)
        {
            bool retornar = false;
            if (file != null)
            {
                long size = SizeMax * 1000000;
                var ruta = file.FileName;
                string[] e = ruta.Split('.');
                if (file.Length > size)
                {
                    retornar = true;
                    Alerta(
                        archivo +" excede el liminte de " + SizeMax + " MB",
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


        public static bool VerificarExtencion(IFormFile file, string tipo1, string tipo2, string tipo3, string tipodearchivo, string archivo)
        {
            bool retornar = false;
            if (file != null)
            {
                var ruta = file.FileName;
                string[] e = ruta.Split('.');
                string a = e[e.Length - 1].ToString().ToLower();

                if (a == tipo1 || a == tipo2 || a == tipo3)
                {
                    retornar = false;

                }

                else
                {
                    retornar = true;
                    Alerta(

                        "El formato [" + a + "] de "+archivo+" no corresponde a "+tipodearchivo,
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




        public static string SubirArchivo(IFormFile archivo, string nombre, string carpeta, string tipo, IHostingEnvironment _he)
        {
            string retornar = "Userdefault.png";
            if (archivo != null)
            {
                var nombrearchivo = nombre.ToLower(); ;
                var ruta = archivo.FileName;
                string[] extencion = ruta.Split('.');

                var filename = Path.Combine(_he.WebRootPath + "/" + carpeta, tipo + "-" + nombrearchivo + "." + extencion[1]);

                if (System.IO.File.Exists(filename))
                {

                    System.IO.File.Delete(filename);

                }

                archivo.CopyTo(new FileStream(filename, FileMode.Create));


                retornar = Path.GetFileName(filename);
            }

            return retornar;
        }






        public static void Alerta(string msj, string tipo, string icono)
        {
            AlertaGeneral.msj = msj;
            AlertaGeneral.tipo = tipo;
            AlertaGeneral.icono = icono;


        }

        public static bool VerificarSelect(string option)
        {
            bool retornar = false;
            if (option == "Seleccione...")
            {
                retornar = true;
            }

            return retornar;
        }



    }


}
