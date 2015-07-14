using UnityEngine;
using System.Collections;

public class Sensors : MonoBehaviour {
	public bool showDebug;
	public int wallRange;
	public int playerRange;
	public RaycastHit hit;
	public RaycastHit hitRight;
	public RaycastHit hitLeft;
	
	private int playerLayerMask;
	private int wallLayerMask;
	private RaycastHit2D hitBackRight;
	private RaycastHit2D hitBackLeft;
	private RaycastHit hitPlayer;
	private Vector3 rightRay;
	private Vector3 leftRay;
	private Vector3 rightBackRay;
	private Vector3 leftBackRay;
	private Vector3 rayHeight;
	private bool playerWasHit;
	private bool a;
	
	// Use this for initialization
	void Start () {
		wallLayerMask = 1 << 13;
		playerLayerMask = 1 << 8;
		wallRange = 3;
		playerRange = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if (showDebug == true) {
			Debug.DrawRay (rayHeight, this.transform.forward * wallRange, Color.red);
			Debug.DrawRay (rayHeight, rightRay * wallRange, Color.red);
			Debug.DrawRay (rayHeight, leftRay * wallRange, Color.red);
			Debug.DrawRay (transform.position, this.transform.forward * playerRange, Color.red);
			//Debug.DrawRay (rayHeight, rightBackRay * wallRange, Color.red);
			//Debug.DrawRay (rayHeight, leftBackRay * wallRange, Color.red);
		}
	}
	
	void FixedUpdate() {
		rayHeight = transform.position;
		rayHeight.y -= 0.4f;
		rightRay = Quaternion.AngleAxis (-45, transform.up) * this.transform.forward;
		leftRay = Quaternion.AngleAxis (45, transform.up) * this.transform.forward;
		//rightBackRay = Quaternion.AngleAxis (-135, transform.up) * -this.transform.forward;
		//leftBackRay = Quaternion.AngleAxis (135, transform.up) * -this.transform.forward;
		
		a = Physics.Raycast(rayHeight, this.transform.forward, out hit, wallRange, wallLayerMask);
		a = Physics.Raycast(rayHeight, rightRay, out hitRight, wallRange, wallLayerMask);
		a = Physics.Raycast(rayHeight, leftRay, out hitLeft, wallRange, wallLayerMask);
		playerWasHit = Physics.Raycast (transform.position, this.transform.forward, out hitPlayer, playerRange, playerLayerMask);
		//hitBackRight = Physics2D.Raycast (transform.position,rightBackRay, wallRange, wallLayerMask);
		//hitBackLeft = Physics2D.Raycast (transform.position, leftBackRay, wallRange, wallLayerMask);
	}
	
	public double checkCenter(){
		if(hit.collider != null){
			if(hit.collider.tag == "barrel")
				return (double)hit.distance;
		}
		
		return (double)wallRange;
	}
	
	public double checkRight(){
		if(hitRight.collider != null){
			return (double)hitRight.distance;
		}
		
		return (double)wallRange;
	}
	
	public double checkLeft(){
		if(hitLeft.collider != null){
			return (double)hitLeft.distance;
		}
		
		return (double)wallRange;
	}
	
	public double checkBackRight(){
		if(hitBackRight.collider != null){
			return (double)hitBackRight.distance;
		}
		
		return (double)wallRange;
	}
	
	public double checkBackLeft(){
		if(hitBackLeft.collider != null){
			return (double)hitBackLeft.distance;
		}
		
		return (double)wallRange;
	}
	
	public double checkPlayer(){
		if(hitPlayer.collider != null){
			return (double)hitPlayer.distance;
		}
		
		return 0;
	}
}
