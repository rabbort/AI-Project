using UnityEngine;
using System.Collections;

public class AttachCat : MonoBehaviour {
	public GameObject lhand;
	
	// Use this for initialization
	void Start () {
		lhand = GameObject.Find ("Bip001 L Hand");
	}
	
	// Update is called once per frame
	void Update () {
		transform.parent = lhand.transform;
	}
}
