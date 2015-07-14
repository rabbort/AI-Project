using UnityEngine;
using System.Collections;

public class RoomSpawner : MonoBehaviour {
	public GameObject[] rooms;

	private GameObject newRoom;

	// Use this for initialization
	void Awake () {
		int roomNum = Random.Range (0,rooms.Length);
		newRoom = Instantiate(rooms[roomNum], transform.position, Quaternion.identity) as GameObject;
	}

	public void destroyRoom(){
		Destroy(newRoom);
	}
	
	public void spawnRoom(){
		int roomNum = Random.Range (0,4);
		newRoom = Instantiate(rooms[roomNum], transform.position, Quaternion.identity) as GameObject;
	}
}
