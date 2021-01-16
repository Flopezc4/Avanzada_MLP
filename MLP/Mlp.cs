using System;
using System.Collections.Generic;
using System.Text;

namespace MLP
{
    class Mlp
    {
        public List<Capas> capas;
        public List<double[]> sigma;
        public List<double[,]> deltas;
        
        public Mlp(int [] numeroNeuronasporCapa)
        {
            capas = new List<Capas>();
            Random r = new Random();

            for(int i = 0; i < numeroNeuronasporCapa.Length; i++)       ///por cada numeroNeuronasporCapa,se agregara una nueva capa
            {
                capas.Add(new Capas(i == 0 ? numeroNeuronasporCapa[i] : numeroNeuronasporCapa[i - 1], numeroNeuronasporCapa[i], r));
            }

        }
        public double [] Activacion(double[] inputs)
        {
            double[] outputs = new double[0];   ///array de doubles, por cada capa se iran activando cada capa y guardando su contenido en la variable outputs
            for(int i=0 ; i < capas.Count; i++)
            {
                outputs = capas[i].activacion(inputs);
                inputs = outputs;
            }
            return outputs;
        }

        public double ErrorIndividual(double [] realOutput, double[] output)      ///cuantificar el error 
        {
            double error = 0;
            for(int i =0; i< realOutput.Length; i++)
            {
                error += Math.Pow(realOutput[i] - output[i], 2);  /// el error cuadratico medio
            }
            return error;
        }

        public double ErrorGeneral(List<double[]> inputs, List<double[]> outputs)
        {
            double error = 0;
            for(int i =0; i < inputs.Count; i++)
            {
                error += ErrorIndividual(Activacion(inputs[i]), outputs[i]);
            }
            return error;
        } 
        
        public bool Aprender(List<double[]> ejemplosInput, List<double[]> ejemplosOutputs, double alfa, double maxError , int maxIterations)   ///revisar
        {
            double error = 99999;

            while(error > maxError)
            {
                maxIterations--;
                if (maxIterations <= 0)
                {
                    Console.WriteLine("---------------------Minimo local-------------------------");
                    Console.WriteLine(error);
                    return false;
                }

                if (Console.KeyAvailable)
                {
                    Console.WriteLine("---------------------Boton de escape-------------------------");
                    Console.WriteLine(error);
                    return true;
                }
               
                Backpropagation(ejemplosInput,ejemplosOutputs,alfa);
                error = ErrorGeneral(ejemplosInput, ejemplosOutputs);
               // Console.WriteLine(error);
            }
            Console.WriteLine(error);
            return true;
        }

        void ResetearDeltas()                       ///a cada peso le corresponde un delta
        {
            deltas = new List<double[,]>(); 
            for (int i = 0; i < capas.Count; i++)
            {
                deltas.Add(new double[capas[i].neuronas.Count, capas[i].neuronas[0].pesos.Length]);
                
            }
        }
       
        void SetPesos(double alfa)
        {
            for (int i = 0; i < capas.Count; i++)
            {
                for (int j = 0; j < capas[i].neuronas.Count; j++)
                {
                    for (int k = 0; k < capas[i].neuronas[j].pesos.Length; k++)
                    {
                        capas[i].neuronas[j].pesos[k] -= alfa * deltas[i][j,k];
                    }
                }
            }


        }
        void SetBias(double alfa)
        {
            for(int i =0; i< capas.Count; i++)
            {
                for(int j =0; j< capas[i].neuronas.Count;j++)
                {
                    capas[i].neuronas[j].bias -= alfa * sigma[i][j];
                }
            }
        }
        void SetSigmas(double[] outputs)
        {
            sigma = new List<double[]>();
            for(int i = 0; i<capas.Count; i++)
            {
                sigma.Add(new double[capas[i].neuronas.Count]);
            }
            for (int i = capas.Count-1; i>=0; i--)
            {
                for (int j = 0; j <capas[i].neuronas.Count ; j++)
                {
                    if (i == capas.Count - 1)
                    {
                        double y = capas[i].neuronas[j].ultimaActivacion;
                        sigma[i][j] = (Neurona.Sigmoide(y)-outputs[j]) * Neurona.SigmaideDerivada(y);
                    }
                    else
                    {
                        double sum = 0;
                        for (int k = 0; k < capas[i + 1].neuronas.Count; k++)
                        {
                            sum += capas[i + 1].neuronas[k].pesos[j] * sigma[i + 1][k];
                        }
                        sigma[i][j] = Neurona.SigmaideDerivada(capas[i].neuronas[j].ultimaActivacion) * sum;
                    }
                }
            }
        }
        void AddDelta()
        {
            for(int i =1; i<capas.Count; i++)
            {
                for(int j = 0; j< capas[i].neuronas.Count; j++)
                {
                    for(int k = 0; k< capas[i].neuronas[j].pesos.Length; k++)
                    {
                        deltas[i][j, k] += sigma[i][j] * Neurona.Sigmoide(capas[i-1].neuronas[k].ultimaActivacion); 
                    }
                }
            }
        }
        public void Backpropagation(List<double[]> inputs, List<double[]> outputs, double alfa)
        {
            ResetearDeltas();
            for(int i = 0; i < inputs.Count; i++)
            {
                Activacion(inputs[i]);
                SetSigmas(outputs[i]);
                
                SetBias(alfa);
                AddDelta();        ///van a ser modificados por cada vez que introduzcamos un valor de input y sea comparado con outputs
            }
            SetPesos(alfa);         ///van a ser modificados por cada vez que aprendamos
        }


    }
}
