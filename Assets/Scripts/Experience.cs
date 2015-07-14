using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Experience : MonoBehaviour {
	public int currLevel;
	public int numRooms;
	public int toLevel;
	public int enemyHealth;
	public int enemyDamage;

	public Slider xpSlider;
	public Text levelText;
	public Slider healthSlider;
	public Text meleeDamage;
	public Animator[] anim;
	public Text rangedDamage;

	private PlayerController player;
	private GeneticAlgorithm ga;
	private int cat;

	// Use this for initialization
	void Start () {
		enemyHealth = 50;
		enemyDamage = 5;
		currLevel = 0;
		numRooms = 0;
		toLevel = 3;
		levelText.text = "" + (currLevel + 1);
		xpSlider.value = numRooms;
		xpSlider.maxValue = toLevel;
		healthSlider.maxValue = 150;
		player = GameObject.Find ("Warrior").GetComponent<PlayerController> ();
		ga = GameObject.Find ("Spawner").GetComponent<GeneticAlgorithm>();
	}
	
	// Update is called once per frame
	void Update () {
		meleeDamage.text = "" + player.GetComponent<PlayerController> ().melee;
		rangedDamage.text = "" + player.GetComponent<PlayerController> ().ranged;
	}

	public void levelUp(){
		cat = Random.Range (0,anim.Length);
		anim[cat].SetTrigger("LevelUp");

		enemyHealth += 20;
		enemyDamage += 2;	

		numRooms = 0;
		currLevel++;
		toLevel = (currLevel * 2) + 3;
		
		xpSlider.value = numRooms;
		xpSlider.maxValue = toLevel;
		
		levelText.text = "" + (currLevel + 1);
		
		changeStats ();
	}

	//Function called when cleared a room.
	public void roomClear(){
		numRooms++;
		xpSlider.value = numRooms;
		if (numRooms >= toLevel) {
			levelUp();	
			ga.sortByFitness ();
			ga.evolve ();
			ga.shufflePopulation();
		}
	}

	void changeStats(){
		int currMelee = player.GetComponent<PlayerController> ().melee;/*Read in character damage to int*/
		int currRanged = player.GetComponent<PlayerController> ().ranged;/*Read in character damage to int*/
		
		float numMeleeHits = player.GetComponent<PlayerController> ().numMelee++;
		float numRangedHits = player.GetComponent<PlayerController> ().numRanged++;
		
		int currHealth = 150+(20*currLevel);
		player.hp = currHealth;//Get max hp from player.
		
		int changeMelee = (int)Mathf.Floor(15 * (numMeleeHits/(numMeleeHits + numRangedHits)));
		currMelee += 5 + changeMelee;
		currRanged += 5 + (15 - changeMelee);
		
		healthSlider.maxValue = currHealth;
		healthSlider.value = currHealth;
		meleeDamage.text = "" + currMelee;
		player.GetComponent<PlayerController> ().melee = currMelee;
		rangedDamage.text = "" + currRanged;
		player.GetComponent<PlayerController> ().ranged = currRanged;
		player.GetComponent<PlayerController> ().numMelee = 0;
		player.GetComponent<PlayerController> ().numRanged = 0;
	}
}
