using UnityEngine;
using System.Collections;

public class RangedRadar : MonoBehaviour {
	
	public bool showDebug;
	
	private int radius;
	private int rRadius;
	private int playerLayerMask;
	private Collider[] hit;
	private Collider[] rotation;
	
	// Use this for initialization
	void Start () {
		playerLayerMask = 1 << 8;
		radius = 5;
		rRadius = 15;
		hit = Physics.OverlapSphere (transform.position, radius, playerLayerMask);
		rotation = Physics.OverlapSphere (transform.position, rRadius, playerLayerMask);
	}
	
	void FixedUpdate(){
		hit = Physics.OverlapSphere (transform.position, radius, playerLayerMask);
		rotation = Physics.OverlapSphere (transform.position, rRadius, playerLayerMask);
	}
	
	void OnDrawGizmos() {
		if (showDebug == true) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, radius);
		}
	}
	
	public double checkRadar(){
		if(hit != null){
			if(hit.Length > 0){
				return (double)Vector3.Distance (transform.position, hit[0].transform.position);
			}
		}
		
		return (double)radius;
	}
	
	public double angleToPlayer(){
		if(rotation != null){
			if(rotation.Length > 0){
				Vector3 playerDir = rotation[0].transform.position - transform.position;
				float angle = Vector3.Angle (transform.forward, playerDir);
				return (double)angle;
			}
		}
		
		return (double)Random.Range (0, 180);
	}
	
	public float checkDir(){
		if(rotation != null){
			if(rotation.Length > 0){
				Vector3 playerDir = rotation[0].transform.position - transform.position;
				float angleDir = AngleDir(transform.forward, playerDir, Vector3.up);
				return angleDir;
			}
		}
		
		return 0;
	}
	
	public double getRadius(){
		return (double)radius;
	}
	
	// Returns -1 if left of player heading, 1 if right	
	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
	}
}
