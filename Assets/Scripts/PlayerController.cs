using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public int hp;
	public bool alive;
	public Slider healthSlider;
	public int melee;
	public int ranged;
	public float numMelee;
	public float numRanged;
	
	private AnimationSelector animSelector;
	private GameObject rEnemy;
	private GameObject mEnemy;

	// Use this for initialization
	void Start () {
		animSelector = GetComponent<AnimationSelector>();
		hp = 150;
		alive = true;
		melee = 10;
		ranged = 10;
		numMelee = 0.0F;
		numRanged = 0.0F;
		rEnemy = GameObject.FindGameObjectWithTag ("ranged");
		mEnemy = GameObject.FindGameObjectWithTag ("melee");
	}

	void GameOver(){
		Application.LoadLevel ("gameover");
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		if(alive){
			if(hp <= 0){
				animSelector.die ();
				alive = false;
				GetComponent<CharacterController>().enabled = false;
				Invoke("GameOver", 4.0f);
			}
			else
				animSelector.animate ();
		}

		healthSlider.value = hp;
	}


	void OnCollisionEnter(Collision collision){
		if(collision.collider.tag == "catarrow"){
			hp -= collision.collider.GetComponent<CatBullet>().attacker.damage;
			collision.collider.GetComponent<CatBullet>().attacker.fitness += 10;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "enemyweapon" && other.GetComponentInParent<LizardAnimationSelector>().attacking){
			hp -= other.GetComponentInParent<LizardController>().damage;
			other.GetComponentInParent<LizardController>().fitness += 10;
		}
	}
}