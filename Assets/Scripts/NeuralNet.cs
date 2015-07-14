using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNet : MonoBehaviour {
	public List<NeuronLayer> net;
	public int numHiddenLayers;
	public int activationResponse;
	public int populationIndex;	
	public int numNeurons;
	public int numInputs;
	public int numOutputs;
	
	private int bias;
	private double netInput;

	// Use this for initialization
	void Awake () {
		numNeurons = 10;
		numHiddenLayers = 5;
		numInputs = 6; 
		numOutputs = 3; 
		bias = -1;
		activationResponse = 1;

		net = new List<NeuronLayer>();
		
		// Create the hidden layers
		if(numHiddenLayers > 0){
			// Add the initial layer
			net.Add (new NeuronLayer(numNeurons, numInputs));
			
			for (int i = 0; i < numHiddenLayers - 1; ++i) {
				net.Add (new NeuronLayer (numNeurons, numNeurons));
			}
			
			// Create the output layer
			net.Add (new NeuronLayer(numOutputs, numNeurons));
		}
		else{
			net.Add (new NeuronLayer(numOutputs, numInputs));
		}		
	}

	public class Neuron{
		public int numInputs; // Number of inputs to this neuron
		public List<double> weights; // Weights of each input to this neuron
		
		// Constructor
		public Neuron(int inputs){
			numInputs = inputs + 1;
			weights = new List<double>();
			
			// Randomize the weights to a number between -1 and 1
			for(int i = 0; i < numInputs; ++i){
				weights.Add (Random.Range (-1.0f, 1.0f));
			}
		}
	}

	// Represents a layer of neurons
	public class NeuronLayer{
		public int numNeurons;
		public List<Neuron> layerNeurons;
		
		// Constructor
		public NeuronLayer(int n, int inputsPerNeuron){
			numNeurons = n;
			layerNeurons = new List<Neuron>();
			
			// Fill the array with new neurons
			for(int i = 0; i < numNeurons; i++){
				layerNeurons.Add(new Neuron(inputsPerNeuron));
			}
		}
	}

	// Get all the weights in the net
	public List<double> getWeights(){
		List<double> allWeights = new List<double>();
		
		for(int i = 0; i < numHiddenLayers + 1; i++){
			for(int j = 0; j < net[i].numNeurons; j++){
				for(int k = 0; k < net[i].layerNeurons[0].numInputs; k++){
					allWeights.Add (net[i].layerNeurons[j].weights[k]);
				}
			}
		}
		
		return allWeights;
	}
	
	// Update all the weights
	public void setWeights(List<double> weights){
		int weightCount = 0;
		
		for(int i = 0; i < numHiddenLayers + 1; i++){
			for(int j = 0; j < net[i].numNeurons; j++){
				for(int k = 0; k < net[i].layerNeurons[0].numInputs; k++){
					net[i].layerNeurons[j].weights[k] = weights[weightCount++];
				}
			}
		}
	}

	public List<double> updateNet(List<double> inputs){
		List<double> outputs = new List<double>();
		int cWeight = 0;
		int numIn = numInputs;
		
		if(inputs.Count != numInputs)
			return outputs;
		
		for(int i = 0; i < numHiddenLayers + 1; ++i){
			if(i > 0){
				inputs.Clear ();
				//inputs.AddRange (outputs);
				for(int a = 0; a < outputs.Count; a++){
					inputs.Add (outputs[a]);
				}
			}
			
			outputs.Clear ();
			
			cWeight = 0;	
			
			// sum input*weight for each neuron, then plug into sigmoid function
			for(int j = 0; j < net[i].numNeurons; ++j){
				netInput = 0;
				numIn = net[i].layerNeurons[j].numInputs;
				
				// for each weight
				for(int k = 0; k < numIn - 1; ++k){
					// sum input*weight
					netInput += net[i].layerNeurons[j].weights[k] * inputs[cWeight++];
				}
				
				// Add in the bias
				netInput += net[i].layerNeurons[j].weights[numIn - 1] * bias;	
				
				outputs.Add (sigmoid(netInput, activationResponse));
				
				cWeight = 0;
			}
		}
		
		return outputs;
	}

	public double sigmoid(double activation, double response){
		double sigma = (((1 / (1 + Mathf.Exp ((float)((-activation/response))))) - 0.5f) * 2.0f);
		return sigma;
	}
}
