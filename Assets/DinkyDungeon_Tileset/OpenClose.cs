using UnityEngine;
using System.Collections;

public class OpenClose : MonoBehaviour {
	public bool locked;

	private Animation anim;
	private GameObject player;
	private LockGates locker;
	private RoomSpawner roomspawner;
	private SpawnEnemies enemies;
	private Experience exp;
	private GeneticAlgorithm ga;
	private SpawnEnemies enemyspawner;

	// Use this for initialization
	void Start () {
		locker = GetComponentInParent<LockGates>();
		locked = true;
		anim = GetComponent<Animation>();
		player = GameObject.Find ("Warrior");
		roomspawner = GameObject.Find("Room Spawner").GetComponent<RoomSpawner>();
		enemies = GameObject.Find ("Spawner").GetComponent<SpawnEnemies>();
		exp = GameObject.Find ("Warrior").GetComponent<Experience>();
		ga = GameObject.Find ("Spawner").GetComponent<GeneticAlgorithm>();
		enemyspawner = GameObject.Find ("Spawner").GetComponent<SpawnEnemies>();
	}

	void OnTriggerEnter(Collider other){
		if(!locked && other.collider.tag == "Player"){
			anim.Play ("Open");
		}
	}

	void OnTriggerExit(Collider other){
		bool doorEntered = false;

		if(other.collider.tag == "Player"){
			if(!locked){
				if(player.transform.position.z < 7.0f && player.transform.position.z >= -7.0f){
					if(player.transform.position.x < -10.0f){
						anim.Play ("Close");
						player.transform.position = new Vector3(7,0,player.transform.position.z);
						doorEntered = true;
					}	
					else if(player.transform.position.x > 10.0f){
						anim.Play ("Close");
						player.transform.position = new Vector3(-7,0,player.transform.position.z);
						doorEntered = true;
					}
				}
				else if(player.transform.position.z < -10.0f){
					anim.Play ("Close");
					player.transform.position = new Vector3(player.transform.position.x,0,7);
					doorEntered = true;
				}
				else if(player.transform.position.z > 10.0f){
					anim.Play ("Close");
					player.transform.position = new Vector3(player.transform.position.x,0,-7);
					doorEntered = true;
				}
			}

			if(doorEntered){
				exp.roomClear ();
				enemies.destroyDead ();
				roomspawner.destroyRoom();
				roomspawner.spawnRoom ();
				enemyspawner.spawnWave (exp.numRooms + 1);
			}
		}
		
		locker.lockGates ();
	}
}
