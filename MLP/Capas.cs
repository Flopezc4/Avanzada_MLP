using System;
using System.Collections.Generic;

namespace MLP
{
    [Serializable]
    class Capas
    {
        public List<Neurona> neuronas;  ///lista de neuronas
        public double[] output; ///salidas de cada neurona, se pasarana la sigiente capa

        public Capas(int inputCount, int neuronasCount, Random r)
        {
            neuronas = new List<Neurona>();
            for(int i = 0; i<neuronasCount; i++)
            {
                neuronas.Add(new Neurona(inputCount, r));   ///a la lista se le agrega un numero x de neurona         
            }
        }
        public double[] activacion(double[] inputs)
        {
            List<double> outputs = new List<double>();
            for(int i =0; i< neuronas.Count; i++)
            {
                outputs.Add(neuronas[i].activacion(inputs));
            }
            output = outputs.ToArray();
           return outputs.ToArray();
        }

    }
}
