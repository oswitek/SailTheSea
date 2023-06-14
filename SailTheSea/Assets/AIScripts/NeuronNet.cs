using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

using Random = UnityEngine.Random;

public class NeuronNet : MonoBehaviour
{
    //Macierz dla sensor�w
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 5);

    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();

    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);

    //Macierz dla wag
    public List<Matrix<float>> weights = new List<Matrix<float>>();
    
    //Macierz dla Bias�w 
    public List<float> biases = new List<float>();

    public float fitness;

    public void Initialise (int hiddenLayerCount , int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();

        for (int i = 0; i < hiddenLayerCount + 1; i++) 
        {

            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);

            hiddenLayers.Add(f);

            //Biasy
            biases.Add(Random.Range(-1f, 1f));

            //Wagi
            if (i == 0)
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(5, hiddenNeuronCount);
                weights.Add(inputToH1);
            }
            else
            {
                Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
                weights.Add(HiddenToHidden);
            }

        }

        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 2);

        weights.Add(OutputWeight);
        biases.Add(Random.Range(-1f, 1f));

        RandomiseWeights();
    }
    public void RandomiseWeights()
    {
        for(int i = 0; i < weights.Count; i++)
        {
            for(int x = 0; x < weights[i].RowCount; x++)
            {
                for(int y = 0; y < weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = Random.Range(-1f, 1f);
                }
            }
        }
    }

    public (float, float) RunNetwork (float a, float b, float c, float d, float e)
    {
        inputLayer[0, 0] = a;
        inputLayer[0, 1] = b;
        inputLayer[0, 2] = c;
        inputLayer[0, 3] = d;
        inputLayer[0, 4] = e;

        inputLayer = inputLayer.PointwiseTanh();

        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();  

        //Pierwszy output to up a drugi output to turn
        return ((float)Math.Tanh(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }
}
