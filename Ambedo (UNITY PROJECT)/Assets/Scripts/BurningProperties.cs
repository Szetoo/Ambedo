using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningProperties : MonoBehaviour {

	public int startingHealth;
	public float burnDamageDelayTime;
	public float burningTime;
	public int standardFireAmount;
	public int standardAfterburnAmount;

	private bool isOnFire;
	private int health;

	private float timeToStopBurning;
	private float timeToNextBurnDamage;

	// Use this for initialization
	void Start () {
		health = startingHealth;
	}

	void takeBurnDamage(int amount){
		
		health -= amount;
		Debug.Log("Health "+health.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if (isOnFire) {
			
			if (health <= 0) {
				Destroy (gameObject);
			}

			if (timeToStopBurning < Time.time) {
				isOnFire = false;
			} else if (timeToNextBurnDamage < Time.time){
				takeBurnDamage (standardAfterburnAmount);
				timeToNextBurnDamage = Time.time + burnDamageDelayTime;
			}
		}
	}

	void catchFire(){
		isOnFire = true;
		timeToStopBurning = Time.time + burningTime;
		timeToNextBurnDamage = Time.time + burnDamageDelayTime;
	}

	void OnCollisionEnter2D(Collision2D other){
		
		if (other.collider.tag == "Fire") {
			
			catchFire();
			Debug.Log(isOnFire);
			takeBurnDamage (standardFireAmount);
		}
	}


}
