using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainingSpawner : MonoBehaviour {
	public float timer;
	public GameObject meleePrefab;
	public GameObject rangedPrefab;
	
	private OrcRangedController[] rangedEnemies;
	private LizardController[] meleeEnemies;
	private bool rangedDead, meleeDead;
	private TrainingAlgorithm ga;
	
	void Start(){
		ga = GetComponent<TrainingAlgorithm>();
		spawnWave(1);
		timer = 30.0f;
	}
	
	void Update(){
		rangedEnemies = GetComponentsInChildren<OrcRangedController>();
		meleeEnemies = GetComponentsInChildren<LizardController>();
		timer -= Time.deltaTime;

		// Do this every 10 seconds
		if(timer < 0){
			destroyDead();
			ga.evolve ();
			spawnWave(1);
			timer = 30.0f;
		}
	}
	
	public void destroyDead(){
		for(int i = 0; i < rangedEnemies.Length; i++){
			ga.storeFitness (rangedEnemies[i].fitness, rangedEnemies[i].GetComponent<NeuralNet>().populationIndex);
			Destroy(rangedEnemies[i].transform.gameObject);
		}
		
		for(int i = 0; i < meleeEnemies.Length; i++){
			ga.storeFitness (meleeEnemies[i].fitness, meleeEnemies[i].GetComponent<NeuralNet>().populationIndex);
			Destroy(meleeEnemies[i].transform.gameObject);
		}
	}
	
	public void spawnWave(int wave){
		// Spawn ten enemies, give them the correct weights for the network
		for(int i = 0; i < 50; i++){
			if(ga.population[i].enemyType == 0){
				//GameObject newMelee = Instantiate(meleePrefab, new Vector3(Random.Range (-7.0f, 7.0f), 1.5f, Random.Range (-7.0f, 7.0f)), Quaternion.identity) as GameObject;
				GameObject newMelee = Instantiate(meleePrefab, new Vector3(-3.0f, 1.5f, -3.0f), Quaternion.identity) as GameObject;
				newMelee.transform.parent = this.transform;
				newMelee.GetComponent<NeuralNet>().populationIndex = i;
				newMelee.GetComponent<NeuralNet>().setWeights (ga.population[i].weights);
			}
			else if(ga.population[i].enemyType == 1){
				//GameObject newRanged = Instantiate(rangedPrefab, new Vector3(Random.Range (-7.0f, 7.0f), 0.2f, Random.Range (-7.0f, 7.0f)), Quaternion.identity) as GameObject;
				GameObject newRanged = Instantiate(rangedPrefab, new Vector3(-3.0f, 0.2f, -3.0f), Quaternion.identity) as GameObject;
				newRanged.transform.parent = this.transform;
				newRanged.GetComponent<NeuralNet>().populationIndex = i;
				newRanged.GetComponent<NeuralNet>().setWeights (ga.population[i].weights);
				List<double> theseWeights = newRanged.GetComponent<NeuralNet>().getWeights ();

			}
		}
	}
}
