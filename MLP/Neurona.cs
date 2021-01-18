using System;
using System.Collections.Generic;
using System.Text;

namespace MLP
{
    [Serializable]
    class Neurona
    {
        public double[] pesos;
        public double bias;

        public double ultimaActivacion;

        public Neurona(int inputCount, Random r)// constructor -- inputCount = numero de entradas a la neurona
        {
            bias = 10 * r.NextDouble() - 5;//r.NextDouble();
            pesos = new double[inputCount];
            for(int i = 0; i < inputCount; i++)
            {
                pesos[i] = 10 * r.NextDouble() - 5;// r.NextDouble();              ///por cada entrada i, se le asigna un peso aleatorio
            }
        
        }

        public double activacion(double[] inputs)               ///sumatoria regresión lineal
        {
            double activacion1 = bias;
            for(int i = 0; i < inputs.Length; i++)
            {
                activacion1 += inputs[i] * pesos[i];       ///entrada por su peso
            }
            ultimaActivacion = activacion1;
            return Sigmoide(activacion1);
        }


        //Funcion de activacion sigmoidal
        public static double Sigmoide(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }
        //Derivada de la f.sigmoidal
        public static double SigmaideDerivada(double input)
        {
            double y = Sigmoide(input);
            return y * (1 - y);
        }

    }
}
