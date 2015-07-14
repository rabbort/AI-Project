using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CatLook : MonoBehaviour {
	public GameObject neck;
	public GameObject laser;
	public GameObject rEye;
	public GameObject lEye;

	private GameObject player;
	private Quaternion rotation;
	private GameObject rightLaser;
	private GameObject leftLaser;
	private GameObject catWrath;
	private bool madkitty;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Bip001");

		catWrath = GameObject.FindGameObjectWithTag ("catslider");
		madkitty = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(madkitty){
			neck.transform.LookAt(player.transform);
			rEye.transform.LookAt(player.transform);
			lEye.transform.LookAt(player.transform);
			rightLaser.transform.position = rEye.transform.position;
			rightLaser.transform.rotation = rEye.transform.rotation;
			leftLaser.transform.position = lEye.transform.position;
			leftLaser.transform.rotation = lEye.transform.rotation;
		}

		if(!madkitty && catWrath.GetComponent<Slider>().value == 100){
			rightLaser = Instantiate(laser, rEye.transform.position, rEye.transform.rotation) as GameObject;
			leftLaser = Instantiate(laser, lEye.transform.position, lEye.transform.rotation) as GameObject;
			madkitty = true;
		}
	}
}
