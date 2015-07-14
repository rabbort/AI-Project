using UnityEngine;
using System.Collections;

public class CatGun : MonoBehaviour {
	public GameObject bullet;
	public GameObject player;

	private float reloadTime;

	void Awake(){
		player = GameObject.Find ("Warrior");
		reloadTime = 0.6f;
	}

	void Fire(){
		GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.LookRotation (player.transform.forward)) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		reloadTime -= Time.deltaTime;

		if(Input.GetKeyDown (KeyCode.Mouse1) && reloadTime < 0){
			Fire();
			reloadTime = 0.6f;
		}
	}
}
