using UnityEngine;
using System.Collections;

public class SetDetailDistance : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		Terrain.activeTerrain.detailObjectDistance = 5000;
	}
}
