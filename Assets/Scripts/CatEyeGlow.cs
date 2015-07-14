using UnityEngine;
using System.Collections;

public class CatEyeGlow : MonoBehaviour {
	public GameObject neck;
	public GameObject laser;
	public GameObject rEye;
	public GameObject lEye;

	private GameObject leftLaser;
	private GameObject rightLaser;

	// Use this for initialization
	void Start () {
		leftLaser = Instantiate(laser, lEye.transform.position, lEye.transform.rotation) as GameObject;
		rightLaser = Instantiate(laser, rEye.transform.position, rEye.transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		rightLaser.transform.position = rEye.transform.position;
		rightLaser.transform.rotation = rEye.transform.rotation;
		leftLaser.transform.position = lEye.transform.position;
		leftLaser.transform.rotation = lEye.transform.rotation;
	}
}
