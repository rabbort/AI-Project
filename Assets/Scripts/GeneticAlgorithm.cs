using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GeneticAlgorithm : MonoBehaviour {
	public List<Genome> population;	
	public int popSize;
	
	private double mutationRate;
	private double maxPerturbation;
	private double crossoverRate;
	private int numToReplace;
	private int numWeights;
	private List<double> weights;
	private List<double> rangedWeights;
	private List<double> meleeWeights;
	private NeuralNet n;
	private string rangedpath;
	private string meleepath;

	public class Genome{
		public List<double> weights;
		public double fitness;
		public int enemyType;
		
		public Genome(){
			fitness = 0;
			weights = new List<double>();
		}
		
		public Genome(List<double> w, double f, int type){
			weights = new List<double>();
			weights = w.GetRange (0, w.Count);
			fitness = f;
			enemyType = type;
		}
	}

	// When given a chromosome (list of a neurons weights), there is a chance to mutate each weight
	private void mutate(List<double> chromo){
		// traverse the weight list and mutate each weight dependent on the mutation rate
		for(int i = 0; i < chromo.Count; ++i){
			if(Random.value < mutationRate){
				// Mutate a weight by adding a random number from -maxPerturbation to maxPerturbation
				chromo[i] += (Random.Range (-1.0f, 1.0f) * maxPerturbation);
			}
		}
	}

	// Use this for initialization
	void Awake () {
		rangedpath = Application.dataPath+"/ranged.txt";
		meleepath = Application.dataPath+"/melee.txt";
		n = GameObject.Find ("ReferenceNet").GetComponent<NeuralNet>();
		numWeights = (n.numInputs + 1)*n.numNeurons + (n.numNeurons + 1)*n.numNeurons*(n.numHiddenLayers - 1) + (n.numNeurons + 1)*(n.numOutputs);
		popSize = 30;
		mutationRate = 0.2;
		maxPerturbation = 0.2;
		crossoverRate = 0.7;

		population = new List<Genome>();
		weights = new List<double>();
		rangedWeights = new List<double>();
		meleeWeights = new List<double>();

		TextReader reader = File.OpenText (rangedpath);
		TextReader mreader = File.OpenText (meleepath);
		string rline;
		string mline;
		string[] rweight;
		string[] mweight;
		double test;

		rline = reader.ReadLine();
		rweight = rline.Split (' ');

		mline = mreader.ReadLine();
		mweight = mline.Split (' ');
		
		foreach(string w in rweight){
			double.TryParse (w, out test);
			rangedWeights.Add (test);
		}
		
		while(rangedWeights.Count > numWeights){
			rangedWeights.RemoveAt (rangedWeights.Count - 1);
		}

		foreach(string w in mweight){
			double.TryParse (w, out test);
			meleeWeights.Add (test);
		}
		
		while(meleeWeights.Count > numWeights){
			meleeWeights.RemoveAt (meleeWeights.Count - 1);
		}
		
		
		// Set up the initial population with random weights
		for(int i = 0; i < popSize; i++){
			// Half melee, half ranged to start
			if(i < 15){
				initializeGenome(meleeWeights, 0);
			}
			else
				initializeGenome(rangedWeights, 1);
		}
	}
	
	// Add a genome to the population
	public void initializeGenome(List<double> weightList, int enemy){
		population.Add (new Genome(weightList, 0, enemy));
	}

	// Stores a fitness level at the given index
	public void storeFitness(double fitnessLevel, int index){
		population[index].fitness = fitnessLevel; 
	}

	// Sort the population from lowest fitness to highest fitness
	public void sortByFitness(){
		population.Sort ((x, y) => x.fitness.CompareTo (y.fitness));
	}

	public void shufflePopulation(){
		for (int i = 0; i < population.Count; i++) {
			Genome temp = population[i];
			int randomIndex = Random.Range(i, population.Count);
			population[i] = population[randomIndex];
			population[randomIndex] = temp;
		}
	}
	
	// Selects a genome for reproduction based on roulette wheel selection
	// Higher fitness gives a higher chance of being selected
	private Genome rouletteWheelSelection(){
		double totalFitness = 0;

		// Add up the total fitness of the whole population
		for(int i = 0; i < population.Count; ++i){
			totalFitness += population[i].fitness;
		}

		// If nobody has any fitness, just return a random individual
		if(totalFitness == 0){
			return population[Random.Range(0, population.Count)];
		}

		// This determines which "slice" of the wheel we want
		float slice = Random.value * (float)totalFitness;

		double total = 0;
		int selectedGenome = 0;

		// Add fitnesses until we reach the desired slice
		for(int i = 0; i < population.Count; ++i){
			total += population[i].fitness;

			if(total > slice){
				selectedGenome = i;
				break;
			}
		}

		return population[selectedGenome];
	}

	/*private List<int> calculateSplitPoints(){
		List<int> splitPoints = new List<int>();
		//GameObject player1 = GameObject.FindGameObjectWithTag ("melee");
		int weightCounter = 0;

		// for each layer
		for(int i = 0; i < player1.GetComponent<NeuralNet>().numHiddenLayers; ++i){
			// for each neuron
			for(int j = 0; j < player1.GetComponent<NeuralNet>().net[i].numNeurons; ++j){
				//for each weight
				for(int k = 0; k < player1.GetComponent<NeuralNet>().net[i].layerNeurons[j].numInputs + 1; ++k){
					++weightCounter;
				}

				splitPoints.Add (weightCounter - 1);
			}
		}

		return splitPoints;
	}

	private bool crossoverAtSplits(Genome parent1, Genome parent2, List<double> child1, List<double> child2){
		// Need the same "species"
		if(parent1.enemyType != parent2.enemyType){
			return false;
		}

		// Just skip this one if a parent was selected to be the child
		if(child1.SequenceEqual (parent1.weights) || child1.SequenceEqual (parent2.weights) || child2.SequenceEqual (parent1.weights) || child2.SequenceEqual (parent2.weights)){
			child1 = parent1.weights.GetRange (0, parent1.weights.Count);
			child2 = parent2.weights.GetRange (0, parent2.weights.Count);
			return true;
		}

		// If we do not crossover, just copy parents exactly
		if(Random.value > crossoverRate || parent1.weights.SequenceEqual (parent2.weights)){
			child1 = parent1.weights.GetRange (0, parent1.weights.Count);
			child2 = parent2.weights.GetRange (0, parent2.weights.Count);
			
			return true;
		}

		child1.Clear ();
		child2.Clear ();

		// determine two crossover points
		List<int> sPoints = calculateSplitPoints();
		int index1 = Random.Range (0, sPoints.Count - 2);
		int index2 = Random.Range (0, sPoints.Count - 1);

		int cp1 = sPoints[index1];
		int cp2 = sPoints[index2];

		for(int i = 0; i < parent1.weights.Count; ++i){
			if((i < cp1) || (i >= cp2)){
				child1.Add(parent1.weights[i]);
				child2.Add(parent2.weights[i]);
			}
			else{
				// switch over the belly block
				child1.Add (parent2.weights[i]);
				child2.Add (parent1.weights[i]);
			}
		}

		return true;
	}*/

	private int crossover(Genome parent1, Genome parent2, List<double> child1, List<double> child2){
		int i = 0;
		int type;

		if(parent1.enemyType != parent2.enemyType){
			return -1;
		}
		
		type = parent1.enemyType;
		
		// Just skip this one if a parent was selected to be the child
		if(child1.SequenceEqual (parent1.weights) || child1.SequenceEqual (parent2.weights) || child2.SequenceEqual (parent1.weights) || child2.SequenceEqual (parent2.weights))
			return -1;
		
		// If we do not crossover, just copy parents exactly
		if(Random.value > crossoverRate || parent1.weights.SequenceEqual (parent2.weights)){
			
			child1.Clear ();
			child2.Clear ();
			for(i = 0; i < parent1.weights.Count; i++){
				child1.Add (parent1.weights[i]);
				child2.Add (parent2.weights[i]);
			}
			//child1 = parent1.weights.GetRange (0, parent1.weights.Count);
			//child2 = parent2.weights.GetRange (0, parent2.weights.Count);
			
			return type;
		}
		
		// Otherwise, perform crossover at a random point
		int crossoverPoint = Random.Range(0, parent1.weights.Count - 1);
		
		child1.Clear ();
		child2.Clear ();
		
		for(i = 0; i < crossoverPoint; i++){
			child1.Add (parent1.weights[i]);
			child2.Add (parent2.weights[i]);
		}
		
		for(i = crossoverPoint; i < parent1.weights.Count; i++){
			child1.Add (parent2.weights[i]);
			child2.Add (parent1.weights[i]);
		}

		return type;
	}

	public void evolve(){
		int reproduced;
		popSize += 20; 

		while(population.Count < popSize){
			Genome child1 = new Genome();
			Genome child2 = new Genome();
			reproduced = crossover(rouletteWheelSelection(), rouletteWheelSelection(), child1.weights, child2.weights);
			if(reproduced != -1){
				if(reproduced == 0){
					child1.enemyType = 0;
					child2.enemyType = 0;
				}
				else if(reproduced == 1){
					child1.enemyType = 1;
					child2.enemyType = 1;
				}
				population.Add (child1);
				population.Add (child2);
			}
		}
	}
}
