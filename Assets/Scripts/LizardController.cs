using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LizardController : MonoBehaviour {
	public bool alive;
	public float fitness;
	public GameObject catWrath;
	public int damage;
	public AudioSource die;

	private int hp;
	private LizardAnimationSelector[] animSelector;
	private GameObject player;
	private float moveSpeed;
	private float rotateSpeed;
	private NeuralNet net;
	private Sensors rays;
	private Radar radar;
	private List<double> inputs;
	private List<double> outputs;

	// Use this for initialization
	void Start () {
		damage = 5;
		fitness = 0;
		moveSpeed = 15.0f;
		rotateSpeed = 100.0f;
		alive = true;
		animSelector = GetComponentsInChildren<LizardAnimationSelector>();
		player = GameObject.Find ("Warrior");
		hp = player.GetComponent<Experience>().enemyHealth;
		inputs = new List<double>();
		outputs = new List<double>();
		net = GetComponent<NeuralNet>();
		rays = GetComponent<Sensors>();
		radar = GetComponent<Radar>();
		catWrath = GameObject.FindGameObjectWithTag ("catslider");
		transform.Translate (Vector3.up);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(alive){
			inputs.Clear ();
			outputs.Clear ();

			inputs.Add (net.sigmoid (radar.getRadius () - radar.checkRadar (), net.activationResponse));
			inputs.Add (net.sigmoid(radar.angleToPlayer (), net.activationResponse));
			inputs.Add (net.sigmoid (rays.playerRange - rays.checkPlayer (), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkCenter (), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkLeft(), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkRight (), net.activationResponse));

			outputs = net.updateNet (inputs);

			if(!animSelector[0].dying){
				if(hp <= 0){
					catWrath.GetComponent<Slider>().value -= 10;
					animSelector[0].die ();
					die.Play();
					rigidbody.isKinematic = true;
					GetComponent<Collider>().isTrigger = true;
					alive = false;
				}
				else if(!player.GetComponent<PlayerController>().alive){
					animSelector[0].idle ();
				}
				else if(outputs[0] > 0.0f && Vector3.Distance (player.transform.position, transform.position) < 2.0f){
					animSelector[0].attack ();
				}
				else{
					animSelector[0].walk ();
					transform.Translate (-transform.forward * Time.deltaTime * moveSpeed *(float)outputs[1]);
					transform.Rotate(transform.up * Time.deltaTime * rotateSpeed * radar.checkDir () * (float)outputs[2]);
				}
	
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			}
		}
	}

	void OnCollisionEnter(Collision collision){
		if(alive){
			if((collision.collider.tag == "playerweapon" && collision.collider.GetComponentInParent<AnimationSelector>().attacking)){
				hp -= player.GetComponent<PlayerController>().melee;
				animSelector[0].hit();
				
				player.GetComponent<PlayerController> ().numMelee++;
			}
			if(collision.collider.tag == "catbullet"){
				hp -= player.GetComponent<PlayerController>().ranged;
				player.GetComponent<PlayerController> ().numRanged++;
			}
		}
	}
}
