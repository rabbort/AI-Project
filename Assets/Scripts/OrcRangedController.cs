using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OrcRangedController : MonoBehaviour {
	public int hp;
	public bool alive;
	public float fitness;
	public GameObject catWrath;
	public int damage;
	public AudioSource die;

	private float moveSpeed;	
	private float rotateSpeed;
	private GameObject player;
	private OrcAnimationSelector[] animSelector;
	private List<double> inputs;
	private List<double> outputs;
	private NeuralNet net;
	private Sensors rays;
	private RangedRadar radar;
	
	// Use this for initialization
	void Start () {
		fitness = 0.0f;
		player = GameObject.Find ("Warrior");
		moveSpeed = 10.0f;
		rotateSpeed = 100.0f;
		animSelector = GetComponentsInChildren<OrcAnimationSelector>();
		hp = player.GetComponent<Experience>().enemyHealth;
		damage = player.GetComponent<Experience>().enemyDamage;
		alive = true;
		net = GetComponent<NeuralNet>();
		rays = GetComponent<Sensors>();
		radar = GetComponent<RangedRadar>();
		inputs = new List<double>();
		outputs = new List<double>();
		catWrath = GameObject.FindGameObjectWithTag ("catslider");
		transform.Translate (Vector3.up);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(alive){
			inputs.Clear ();
			outputs.Clear ();

			inputs.Add (net.sigmoid (radar.getRadius () - radar.checkRadar (), net.activationResponse));
			inputs.Add (net.sigmoid(radar.angleToPlayer (), 1));
			inputs.Add (net.sigmoid (rays.playerRange - rays.checkPlayer (), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkCenter (), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkLeft(), net.activationResponse));
			inputs.Add (net.sigmoid (rays.wallRange - rays.checkRight (), net.activationResponse));

			outputs = net.updateNet (inputs);
			
			if(!animSelector[0].dying){
				if(hp <= 0){
					animSelector[0].die ();
					die.Play ();
					rigidbody.isKinematic = true;
					GetComponent<Collider>().isTrigger = true;
					catWrath.GetComponent<Slider>().value -= 10;
					alive = false;
				}
				else if(!player.GetComponent<PlayerController>().alive){
					animSelector[0].idle ();
				}
				else if(outputs[0] > 0.0){
					if(!animSelector[0].attacking)
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
