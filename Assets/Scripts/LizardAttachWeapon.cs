using UnityEngine;
using System.Collections;

public class LizardAttachWeapon : MonoBehaviour {
	public GameObject hand;
	
	// Update is called once per frame
	void Update () {
		transform.parent = hand.transform;
	}

}
