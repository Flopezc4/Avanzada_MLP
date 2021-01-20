using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLP
{
    class Login
    {
        static string Pathlogin = @"D:\DESCARGAS\login.txt";

        public static void Crearusuario()
        {
            Console.WriteLine("Para crear un usuario, debes ingresar tu usuario (correo institucional) luego tu contraseña...");
            Console.WriteLine("--------------------------------------------------");
            string username, password = string.Empty;


            Console.WriteLine("Ingresa correo institucional: ");
            username = Console.ReadLine();

            Console.WriteLine("Ingresa una contraseña: ");
            password = Console.ReadLine();


            using (StreamWriter sv = new StreamWriter(File.Create(Pathlogin)))
            {
                sv.WriteLine(username);
                sv.WriteLine(password);
                sv.Close();

            }
            Console.WriteLine("Felicidades, usuario registrado con exito...");
            Console.WriteLine("--------------------------------------------------");

        }


        public static bool Ingreso()
        {
            String username, password, username1, password1 = string.Empty;
            bool var = true;

            Console.WriteLine("Debes ingresar tu correo institucional junto a tu contraseña...");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Correo institucional: ");
            username = Console.ReadLine();

            Console.Write("Contraseña: ");
            password = Console.ReadLine();


            using (StreamReader sr = new StreamReader(File.Open(Pathlogin, FileMode.Open)))
            {
                username1 = sr.ReadLine();
                password1 = sr.ReadLine();
                sr.Close();
            }
            if (username == username1 && password == password1)
            {
                Console.WriteLine("Hola, Bienvenido..." + username);
                Console.WriteLine("--------------------------------------------------");
                return var;
            }
            else
            {
                Console.WriteLine("Acceso Denegado, porfavor ingresa nuevamente los datos");
                Console.WriteLine("--------------------------------------------------");

                return !var;
            }
        }

    }
}

