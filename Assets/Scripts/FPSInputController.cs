using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;
	private Vector3 up;
	private Vector3 upLeft;
	private Vector3 upRight;
	private Vector3 down;
	private Vector3 downLeft;
	private Vector3 downRight;
	private Vector3 left;
	private Vector3 right;
	private Vector3 idle;
	private AnimationSelector animSelector;
	private PlayerController player;

	public Vector3 directionVector;

    // Use this for initialization
    void Awake()
    {	
		animSelector = GetComponent<AnimationSelector>();
		player = GetComponent<PlayerController>();
        motor = GetComponent<CharacterMotor>();
		up = new Vector3(0,0,1);
		upLeft = new Vector3(-1,0,1);
		upRight = new Vector3(1,0,1);
		down = new Vector3(0,0,-1);
		downLeft = new Vector3(-1,0,-1);
		downRight = new Vector3(1,0,-1);
		left = new Vector3(-1,0,0);
		right = new Vector3(1,0,0);
		idle = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
		if(player.alive){
			if(animSelector.attacking){
				directionVector = idle;
			}
	        // Get the input vector from keyboard
			else if(Input.GetKey(KeyCode.W)){
				if(Input.GetKey(KeyCode.A)){
					directionVector = upLeft;
				}
				else if(Input.GetKey(KeyCode.D)){
					directionVector = upRight;
				}
				else
					directionVector = up;
			}
			else if(Input.GetKey(KeyCode.S)){
				if(Input.GetKey(KeyCode.A)){
					directionVector = downLeft;
				}
				else if(Input.GetKey(KeyCode.D)){
					directionVector = downRight;
				}
				else
					directionVector = down;
			}
			else if(Input.GetKey(KeyCode.A)){
				directionVector = left;
			}
			else if(Input.GetKey(KeyCode.D)){
				directionVector = right;
			}
			else
				directionVector = idle;
	
	        if (directionVector != Vector3.zero)
	        {
	            // Get the length of the directon vector and then normalize it
	            // Dividing by the length is cheaper than normalizing when we already have the length anyway
	            float directionLength = directionVector.magnitude;
	            directionVector = directionVector / directionLength;
	
	            // Make sure the length is no bigger than 1
	            directionLength = Mathf.Min(1.0f, directionLength);
	
	            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
	            // This makes it easier to control slow speeds when using analog sticks
	            directionLength = directionLength * directionLength;
	
	            // Multiply the normalized direction vector by the modified length
	            directionVector = directionVector * directionLength;
	        }
	
	        // Apply the direction to the CharacterMotor
	        motor.inputMoveDirection = directionVector;
	        motor.inputJump = Input.GetButton("Jump");
		}
    }
}