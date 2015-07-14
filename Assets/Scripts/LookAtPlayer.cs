using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Warrior");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform);
	}
}
