using UnityEngine;
using System.Collections;

public class AttachSword : MonoBehaviour {
	public GameObject rhand;

	// Use this for initialization
	void Start () {
		rhand = GameObject.Find ("Bip001 R Hand");
	}
	
	// Update is called once per frame
	void Update () {
		transform.parent = rhand.transform;
	}
}
