using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInformation : MonoBehaviour {
	public Slider healthSlider;
	public int currentHealth;
	public int maxHealth;
	
	public int damage;
	public Text currDamage;
	
	public int ranged;
	public Text currRanged;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.maxValue = maxHealth;
		healthSlider.value = currentHealth;
		currDamage.text = "" + damage;
		currRanged.text = "" + ranged;
	}
}