using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatBullet : MonoBehaviour {
	public float speed;
	public float secondsUntilDestroyed = 10.0f;
	public List<GameObject> explosions;
	public OrcRangedController attacker; // who fired this cat...used to update fitness level

	private float startTime;
	private GameObject newExplosion;
	private int explosionNumber;
	private Quaternion rotation;

	void FixedUpdate(){
		transform.position += speed * transform.forward * Time.deltaTime;
		//transform.Translate (transform.forward * speed * Time.deltaTime);

		if(Time.time - startTime >= secondsUntilDestroyed){
			Destroy(this.gameObject);
		}

		if(Time.time - startTime + 2 >= secondsUntilDestroyed){
			Destroy(newExplosion);
		}
	}
	
	void OnCollisionEnter(Collision collision){
		Destroy(this.gameObject);
		newExplosion = Instantiate(explosions[explosionNumber], transform.position, transform.rotation) as GameObject;
		Destroy(newExplosion, 4.0f); // Destroy explosion object after a few seconds
	}

	void Awake(){
		speed = 5.0f;
		rotation = transform.rotation;
	}

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		explosionNumber = Random.Range(0, explosions.Count); // Pick a random explosion for this cat
	}

	void LateUpdate(){
		transform.rotation = rotation;
	}
}
