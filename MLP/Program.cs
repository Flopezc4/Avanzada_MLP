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
        static string Path = @"D:\DESCARGAS\datos50.csv";                //@"C:\Users\feran\Downloads\prueba.csv";
        static string MlpPath = @"D:\DESCARGAS\ParametrosMlp51.bin";      //@"C:\Users\feran\Downloads\ParametrosMlp.bin";

        static bool CargarMlp = false;
        static bool GuardarMlp = true;

        static void LeerData()
        {
            string data = System.IO.File.ReadAllText(Path).Replace("\r", "");

            string[] row = data.Split(Environment.NewLine.ToCharArray());

            for (int i = 0; i < row.Length; i++)
            {
                string[] rowData = row[i].Split(';');
               Console.WriteLine(rowData.Length+"-------------------------------------" );
                Console.WriteLine();
                double[] inputs = new double[NumEntradas];
                double[] outputs = new double[NumSalidas];

                for (int j = 0; j < rowData.Length; j++)
                {
                    if (j < NumEntradas)
                    {
                        inputs[j] = double.Parse(rowData[j]);
                        Console.Write(inputs[j]+"-");
                    }
                    else
                    {
                        outputs[j - NumEntradas] = double.Parse(rowData[j]);
                        Console.Write(outputs[j - NumEntradas]+"-" );
                    }
               
                }
                Console.WriteLine();
                entradas.Add(inputs);
                salidas.Add(outputs);
            }

        }

        static void Main(string[] args)
        {

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
                for (int i = 0; i < NumEntradas; i++)
                {
                    Console.WriteLine("Inserte valor " + i + ": ");
                    val[i] = double.Parse(Console.ReadLine());
                }
                double[] sal = p.Activacion(val);
                for (int i = 0; i < NumSalidas; i++)
                {
                    Console.Write("Respuesta " + i + ": " + sal[i] + " ");
                }
                Console.WriteLine("");
            }


        }
    }
}
