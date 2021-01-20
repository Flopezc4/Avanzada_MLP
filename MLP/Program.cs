using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;

namespace MLP
{
    class Program
    {
        static List<double[]> entradas = new List<double[]>();
        static List<double[]> salidas = new List<double[]>();

        static int NumEntradas = 16;
        static int NumSalidas = 3;
        static string Path = @"D:\DESCARGAS\Puntajes.csv";               
        static string NormPath =@"D:\DESCARGAS\MaxMin.csv";
        static string MlpPath = @"D:\DESCARGAS\ParametrosMlp51.bin";      

        static bool CargarMlp = false;
        static bool GuardarMlp = true;
        static bool acceso;

        static List<double> min = new List<double>();
        static List<double> max = new List<double>();  
        static string[] titulos = new string[] { "PSU Lenguaje", "PSU Historia", "Ponderado", "Años", "Asignaturas reprobadas", "Rendimiento Mat", "Rendimiento Ing","Rendimiento Otras", "Bias","Femenino", "Masculino" ,"Privado","Subencionado","Municipal","Pertenece a la Región", "No pertenece a la región" };

        static void LeerData()
        {
            string data = System.IO.File.ReadAllText(Path).Replace("\r", "");

            string[] row = data.Split(Environment.NewLine.ToCharArray());

            for (int i = 0; i < row.Length; i++)
            {
                Console.WriteLine("[");
                string[] rowData = row[i].Split(';');
                double[] inputs = new double[NumEntradas];
                double[] outputs = new double[NumSalidas];

                for (int j = 0; j < rowData.Length; j++)
                {
                    if (j < NumEntradas)
                    {
                        inputs[j] = Normalizar(double.Parse(rowData[j]),min[j],max[j]);
                        Console.Write(double.Parse(rowData[j]) + ";");
                    }
                    else
                    {
                        outputs[j - NumEntradas] = Normalizar(double.Parse(rowData[j]), min[j], max[j]);
                        Console.Write(double.Parse(rowData[j]) + ";" );
                    }
               
                }
                Console.WriteLine("]");
                entradas.Add(inputs);
                salidas.Add(outputs);
            }
            Console.Write("Precione una tecla para entrenar......");
            Console.ReadLine();

        }
        static void LeerDataMaxMin()
        {
            string data = System.IO.File.ReadAllText(NormPath).Replace("\r", "");

            string[] row = data.Split(Environment.NewLine.ToCharArray());


            for (int i = 0; i < row.Length; i++)
            {
                string[] rowData = row[i].Split(';');
                //Console.WriteLine(rowData.Length+"-------------------------------------" );
                //Console.WriteLine();

                for (int j = 0; j < rowData.Length; j++)
                {
                    if (i==0)
                    {
                        max.Add(double.Parse(rowData[j]));
                        //Console.Write(min[j]+"---");
                    }
                    if(i==1)
                    {
                        min.Add(double.Parse(rowData[j]));
                       // Console.Write(max[j]+"-" ) ;
                    }

                }
                //Console.WriteLine();
            }

        }

        static double Normalizar(double valor, double min, double max)
        {
            return (valor - min) / (max - min);
        }
        static double NormaInversa(double value, double min, double max)
        {
            return value * (max - min) + min;
        }
        static void Main(string[] args)
        {
            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Por favor, seleccione una de las siguientes opciones: ");
                Console.WriteLine("");
                Console.WriteLine("1º) Registrar Nuevo Usuario");
                Console.WriteLine("2º) Ingresar Usuario");
                Console.WriteLine("3º) Contacto");
                Console.WriteLine("4º) Manual de Usuario");
                Console.WriteLine("5º) Salir");
                Console.WriteLine("");
                Console.WriteLine("Seleccione una opción: ");
                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Ha seleccionado Registrar un nuevo usuario");
                        Login.Crearusuario();
                        break;
                    case 2:
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Ha seleccionado Acceder");
                        acceso = Login.Ingreso();
                        if (acceso)
                        {
                            Console.WriteLine("Ingrese valores del estudiante para la predicción ");
                            red();
                        }
                        break;
                    case 3:
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Ha seleccionado la opción de contacto");
                        Console.WriteLine("");
                        Console.WriteLine("Integrantes: Rodrigo Mamani - Fernando López");
                        Console.WriteLine("Problematica Predicción Académica");
                        Console.WriteLine("Proyecto para Programación Avanzada");
                        Console.WriteLine("Con fecha 20/01/2021");
                        Console.WriteLine("--------------------------------------------------");
                        break;
                    case 4:
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Ha seleccionado la opción de Manual de Usuario:");
                        Console.WriteLine("Paso 1) Para realizar la predicción estudiantil de un estudiante debera registrarse primeramente" + "\n" +
                            "Paso 2) Una vez realizado este proceso. Debera ingresar con su correo electronico junto con su contraseña" + "\n" +
                            "Paso 3) Finalmente, debera ir ingresando los valores del alumno para realizar la predicción de los " + "\n" +
                            "        fenomenos de Retención y Deserción Voluntaria e Involuntaria");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                        
                        break;
                    case 5:
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Hasta luego...");
                        Console.WriteLine("--------------------------------------------------");
                        Environment.Exit(0);
                        break;

                        
                }
                Console.ReadKey();
            } while (opcion !=5);
        }
        static void red()
        {

            LeerDataMaxMin();
            Mlp p;
             if (!CargarMlp)
             {
                 LeerData();
                 p = new Mlp(new int[] { entradas[0].Length,8, salidas[0].Length });// cuantas neuronas hay por capa -- una capa oculta con 3 neuronas

                 while (!p.Aprender(entradas, salidas, 0.05, 0.9, 70000))  //entrada a la red - salida esperada - aprendizaje - error maximo permitido - iteraciones
                 {
                     p = new Mlp(new int[] { entradas[0].Length,8, salidas[0].Length });
                 }
                 if (GuardarMlp)
                 {
                     FileStream fs = new FileStream(MlpPath, FileMode.Create);
                     BinaryFormatter formatter = new BinaryFormatter();
                     try
                     {
                         formatter.Serialize(fs, p);
                     }
                     catch (SerializationException e)
                     {
                         Console.WriteLine("No se pudo serializar, motivo: " + e.Message);
                         throw;
                     }
                     finally
                     {
                         fs.Close();
                     }
                 }

             }
             else
             {
                 FileStream fs = new FileStream(MlpPath, FileMode.Open);
                 try
                 {
                     BinaryFormatter formatter = new BinaryFormatter();
                     p = (Mlp)formatter.Deserialize(fs);
                 }
                 catch(SerializationException e)
                 {
                     Console.WriteLine("Error en deserializacion. motivo :" + e.Message);
                     throw;
                 }
                 finally
                 {
                     fs.Close();
                 }
             }

             // pregunta
             while (true)
             {
                 double[] val = new double[NumEntradas];
                 double x;
                 for (int i = 0; i < NumEntradas; i++)
                 {
                     Console.Write(titulos[i] + ": ");
                     x = double.Parse(Console.ReadLine());
                     val[i] = Normalizar( x,min[i],max[i]);
                    // Console.WriteLine(Normalizar(x, min[i], max[i]));
                 }
                 double[] sal = p.Activacion(val);
                 Console.WriteLine("Retención : " +NormaInversa( sal[0], min[16],max[16]) + " ");
                 Console.WriteLine("Deserción Voluntaria : " + NormaInversa(sal[1], min[17], max[17]) + " ");
                 Console.WriteLine("Deserción Involuntaria : " + NormaInversa(sal[2], min[18], max[18]) + " ");
                 Console.WriteLine("");
             }
        }
    }
}
