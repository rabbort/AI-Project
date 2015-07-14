using UnityEngine;
using System.Collections;

public class LockGates : MonoBehaviour {
	private OpenClose[] gatelist;

	// Use this for initialization
	void Start () {
		gatelist = GetComponentsInChildren<OpenClose>();
	}
	
	public void lockGates(){
		for(int i = 0; i < gatelist.Length; i++){
			gatelist[i].locked = true;
		}
	}

	public void unlockGates(){
		for(int i = 0; i < gatelist.Length; i++){
			gatelist[i].locked = false;
		}
	}
}
