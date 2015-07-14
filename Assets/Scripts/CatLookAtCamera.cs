using UnityEngine;
using System.Collections;

public class CatLookAtCamera : MonoBehaviour {
	public GameObject head;
	public Camera camera;
	
	// Update is called once per frame
	void LateUpdate () {
		head.transform.LookAt (camera.transform);
	}
}
