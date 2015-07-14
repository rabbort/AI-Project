using UnityEngine;
using System.Collections;

public class AnimationSelector : MonoBehaviour {
	private Animation animations;
	private Vector3 moveDirection;
	private Vector3 up;
	private Vector3 idle;
	private float angleFacing;
	private float angleOffset;
	private float angleDirection;
	private Vector3 y;
	private int death;
	
	public bool attacking;
	public AudioSource swing;
	
	// Use this for initialization
	void Start () {
		animations = GetComponent<Animation>();
		up = new Vector3(0,0,1);
		idle = new Vector3(0,0,0);
		y = new Vector3(0,1,0);
		attacking = false;
	}
	
	// Update is called once per frame
	public void animate () {
		moveDirection = GetComponent<FPSInputController>().directionVector;
		angleOffset = Vector3.Angle (up, moveDirection);
	
		angleDirection = AngleDir(y, moveDirection, up);
		if(angleDirection == 1)
			angleOffset = 360.0f - angleOffset;
		angleFacing = transform.eulerAngles.y;

		// Select the appropriate animation based on movement direction and angle player is facing
		if(!attacking){
			if(Input.GetKey (KeyCode.Mouse0)){
				swing.PlayDelayed (0.2f);
				StartCoroutine(Attack());
			}
		}
		if(attacking){
			animations.CrossFade ("Strike8");
		}
		else if(moveDirection != idle){
			if((angleFacing <= 10.0f || angleFacing >= 350.0f) ?  
					angleFacing <= (10.0f + angleOffset) % 360.0f || angleFacing >= (350.0f + angleOffset) % 360.0f :
			   		angleFacing <= (10.0f + angleOffset) % 360.0f && angleFacing >= (350.0f + angleOffset) % 360.0f){
				animations["Run_carry"].speed = 1;
				animations.CrossFade ("Run_carry");
			}
			else if(angleFacing <= (90.0f + angleOffset) % 360.0f){
				animations["Strafe_run_left_carry"].speed = 1;
				animations.CrossFade ("Strafe_run_left_carry");
			}
			else if(angleFacing <= (170.0f + angleOffset) % 360.0f){
				animations["Strafe_run_left_carry"].speed = -1;
				animations.CrossFade ("Strafe_run_left_carry");
			}
			else if(angleFacing <= (190.0f + angleOffset) % 360.0f){
				animations["Run_carry"].speed = -1;
				animations.CrossFade ("Run_carry");
			}
			else if(angleFacing <= (270.0f + angleOffset) % 360.0f){
				animations["Strafe_run_right_carry"].speed = -1;
				animations.CrossFade ("Strafe_run_right_carry");
			}
			else{
				animations["Strafe_run_right_carry"].speed = 1;
				animations.CrossFade ("Strafe_run_right_carry");
			}
		}
		else
			animations.CrossFade ("Idle_carry 1");
	}

	public void die(){
		animations.CrossFade ("Dead2");
	}

	IEnumerator Attack(){
		attacking = true;
		yield return new WaitForSeconds(animations["Strike8"].length);
		attacking = false;
		yield break;
	}
	
	// Returns -1 if angle is right of positive Z axis, 1 if left	
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


