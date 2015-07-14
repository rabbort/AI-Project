using UnityEngine;
using System.Collections;

public class LaserCollision : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Warrior");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other){
		player.GetComponent<PlayerController>().hp -= 10;
	}
}
