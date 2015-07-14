using UnityEngine;
using System.Collections;

public class LizardAnimationSelector : MonoBehaviour {
	private Animation animations;
	
	public bool dying;
	public bool attacking;
	
	// Use this for initialization
	void Awake () {
		animations = GetComponent<Animation>();
		attacking = false;
	}
	
	// Update is called once per frame
	public void walk () {
		if(!attacking){
			animations.CrossFade ("Walk");
		}
	}

	public void idle(){
		if(!attacking){
			animations.CrossFade ("Idle");
		}
	}
	
	public void die(){
		StartCoroutine(Die());
		animations.CrossFade ("Die");
	}
	
	public void attack(){
		StartCoroutine(Attack());
		animations.CrossFade ("Attack");
	}

	public void hit(){
		animations.CrossFade ("Hit");
	}
	
	IEnumerator Attack(){
		attacking = true;
		yield return new WaitForSeconds(animations["Attack"].length);
		attacking = false;
		yield break;
	}

	IEnumerator Die(){
		dying = true;
		yield return new WaitForSeconds(animations["Die"].length);
		dying = false;
		yield break;
	}
}
