using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour {

	public GameObject meleePrefab;
	public GameObject rangedPrefab;
	public Slider catWrath;

	private OrcRangedController[] rangedEnemies;
	private LizardController[] meleeEnemies;
	private bool rangedDead, meleeDead;
	private GameObject doors;
	private GeneticAlgorithm ga;

	void Start(){
		doors = GameObject.Find ("Gates");
		ga = GetComponent<GeneticAlgorithm>();
		ga.shufflePopulation ();
		spawnWave(1);
	}

	void Update(){
		rangedEnemies = GetComponentsInChildren<OrcRangedController>();
		meleeEnemies = GetComponentsInChildren<LizardController>();

		catWrath.value += Time.deltaTime * 1.6f;

		if(rangedEnemies.Length > 0){
			for(int i = 0; i < rangedEnemies.Length; i++){
				if(rangedEnemies[i].alive){
					rangedDead = false;
					doors.GetComponent<LockGates>().lockGates ();
					break;
				}
	
				rangedDead = true;
			}
		}
		else
			rangedDead = true;

		if(meleeEnemies.Length > 0){
			for(int i = 0; i < meleeEnemies.Length; i++){
				if(meleeEnemies[i].alive){
					meleeDead = false;
					doors.GetComponent<LockGates>().lockGates ();
					break;
				}
				
				meleeDead = true;
			}
		}
		else
			meleeDead = true;

		if(meleeDead && rangedDead){
			doors.GetComponent<LockGates>().unlockGates ();
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
		for(int i = 0; i < 10; i++){
			if(ga.population[i + (wave - 1) * 10].enemyType == 0){
				GameObject newMelee = Instantiate(meleePrefab, new Vector3(Random.Range (-7.0f, 7.0f), 1.5f, Random.Range (-7.0f, 7.0f)), Quaternion.identity) as GameObject;
				newMelee.transform.parent = this.transform;
				newMelee.GetComponent<NeuralNet>().populationIndex = i + (wave - 1) * 10;
				newMelee.GetComponent<NeuralNet>().setWeights (ga.population[i + (wave - 1) * 10].weights);
			}
			else if(ga.population[i + (wave - 1) * 10].enemyType == 1){
				GameObject newRanged = Instantiate(rangedPrefab, new Vector3(Random.Range (-7.0f, 7.0f), 0.2f, Random.Range (-7.0f, 7.0f)), Quaternion.identity) as GameObject;
				newRanged.transform.parent = this.transform;
				newRanged.GetComponent<NeuralNet>().populationIndex = i + (wave - 1) * 10;
				newRanged.GetComponent<NeuralNet>().setWeights (ga.population[i + (wave - 1) * 10].weights);
			}
		}
	}
}