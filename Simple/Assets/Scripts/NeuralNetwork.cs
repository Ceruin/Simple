using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    public class NeuralNetwork
    {
        private int inputCount;
        private int outputCount;
        private int hiddenCount;
        private float[][] inputWeights;
        private float[][] hiddenWeights;

        public NeuralNetwork(int inputs, int outputs, int hidden)
        {
            inputCount = inputs;
            outputCount = outputs;
            hiddenCount = hidden;

            inputWeights = new float[inputCount][];
            for (int i = 0; i < inputCount; i++)
            {
                inputWeights[i] = new float[hiddenCount];
            }

            hiddenWeights = new float[hiddenCount][];
            for (int i = 0; i < hiddenCount; i++)
            {
                hiddenWeights[i] = new float[outputCount];
            }

            RandomizeWeights();
        }

        private void RandomizeWeights()
        {
            System.Random rand = new System.Random();
            for (int i = 0; i < inputCount; i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    inputWeights[i][j] = (float)rand.NextDouble();
                }
            }
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < outputCount; j++)
                {
                    hiddenWeights[i][j] = (float)rand.NextDouble();
                }
            }
        }

        public float[] FeedForward(float[] inputs)
        {
            float[] hiddenOutput = new float[hiddenCount];
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < inputCount; j++)
                {
                    hiddenOutput[i] += inputs[j] * inputWeights[j][i];
                }
                hiddenOutput[i] = (float)System.Math.Tanh(hiddenOutput[i]);
            }

            float[] output = new float[outputCount];
            for (int i = 0; i < outputCount; i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    output[i] += hiddenOutput[j] * hiddenWeights[j][i];
                }
                output[i] = (float)System.Math.Tanh(output[i]);
            }
            return output;
        }

        public void BackPropagation(float[] inputs, float[] target, float learningRate)
        {
            float[] hiddenOutput = new float[hiddenCount];
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < inputCount; j++)
                {
                    hiddenOutput[i] += inputs[j] * inputWeights[j][i];
                }
                hiddenOutput[i] = (float)System.Math.Tanh(hiddenOutput[i]);
            }

            float[] output = new float[outputCount];
            for (int i = 0; i < outputCount; i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    output[i] += hiddenOutput[j] * hiddenWeights[j][i];
                }
                output[i] = (float)System.Math.Tanh(output[i]);
            }

            float[] outputErrors = new float[outputCount];
            for (int i = 0; i < outputCount; i++)
            {
                outputErrors[i] = target[i] - output[i];
            }

            float[] hiddenErrors = new float[hiddenCount];
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < outputCount; j++)
                {
                    hiddenErrors[i] += outputErrors[j] * hiddenWeights[i][j];
                }
                hiddenErrors[i] *= (1 - (float)System.Math.Pow(hiddenOutput[i], 2));
            }

            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < outputCount; j++)
                {
                    hiddenWeights[i][j] += learningRate * outputErrors[j] * hiddenOutput[i];
                }
            }

            for (int i = 0; i < inputCount; i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    inputWeights[i][j] += learningRate * hiddenErrors[j] * inputs[i];
                }
            }
        }
    }