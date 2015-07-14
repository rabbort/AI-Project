using UnityEngine;
using System.Collections;

public class OrcAnimationSelector : MonoBehaviour {
	private Animation animations;
	private Vector3 fireHeight;

	public bool dying;
	public bool attacking;
	public GameObject arrow;

	// Use this for initialization
	void Awake () {
		animations = GetComponent<Animation>();
		attacking = false;
		dying = false;
	}
	
	// Update is called once per frame
	public void walk () {
		if(!attacking){
			animations.CrossFade ("Walk");
		}
	}

	public void die(){
		StartCoroutine(Die());
		animations.CrossFade ("Death");
	}

	public void attack(){
		StartCoroutine(Attack());
		animations.CrossFade ("Fire");
		GameObject newBullet = Instantiate(arrow, transform.parent.position, Quaternion.LookRotation (-transform.forward)) as GameObject;
		newBullet.GetComponent<CatBullet>().attacker = GetComponentInParent<OrcRangedController>();
	}

	public void hit(){
		animations.CrossFade ("Hit2");
	}

	public void idle(){
		animations.CrossFade ("Idle");
	}

	IEnumerator Attack(){
		attacking = true;
		yield return new WaitForSeconds(animations["Fire"].length);
		attacking = false;
		yield break;
	}

	IEnumerator Die(){
		dying = true;
		yield return new WaitForSeconds(animations["Death"].length);
		dying = false;
		yield break;
	}
}
