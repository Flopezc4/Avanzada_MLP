using System;
using System.Collections.Generic;

namespace MLP
{
    class Program
    {
        static List<double[]> entradas = new List<double[]>();
        static List<double[]> salidas = new List<double[]>();

        static int NumEntradas = 7;
        static int NumSalidas = 4;
        static string Path = @"D:\DESCARGAS\prueba.csv";

        static void LeerData()
        {
            string data = System.IO.File.ReadAllText(Path).Replace("\r", "");

            string[] row = data.Split(Environment.NewLine.ToCharArray());

            for (int i = 0; i < row.Length; i++)
            {
                string[] rowData = row[i].Split(';');

                double[] inputs = new double[NumEntradas];
                double[] outputs = new double[NumSalidas];

                for (int j = 0; j < rowData.Length; j++)
                {
                    if (j < NumEntradas)
                    {
                        inputs[j] = double.Parse(rowData[j]);
                    }
                    else
                    {
                        outputs[j - NumEntradas] = double.Parse(rowData[j]);
                    }
                }
                entradas.Add(inputs);
                salidas.Add(outputs);
            }

        }

        static void Main(string[] args)
        {
           

            LeerData();
            Mlp p = new Mlp(new int[] { entradas[0].Length, 3, salidas[0].Length });// cuantas neuronas hay por capa -- una capa oculta con 3 neuronas
           
            while (!p.Aprender(entradas, salidas, 0.09, 0.15, 20000))  //entrada a la red - salida esperada - aprendizaje - error maximo permitido - iteraciones
            {
                p = new Mlp(new int[] { entradas[0].Length, 3, salidas[0].Length });
            }
            
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
