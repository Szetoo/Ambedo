using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObjectInteractionsController : MonoBehaviour {

	public GameObject steam;

	private Color steamColor;

	// Use this for initialization
	void Start () {
		steamColor = steam.GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Fire") {
			createSteam (other.gameObject);
			Destroy (other.gameObject);
		}
	}

	void createSteam(GameObject fire){
		Transform forSteam = fire.GetComponent<Rigidbody2D>().transform;
		forSteam.localScale /= 2;
		steamColor = new Color (
			steamColor.r,
			steamColor.g,
			steamColor.b,
			fire.GetComponent<SpriteRenderer> ().color.a);
			
		Instantiate (steam, fire.GetComponent<Rigidbody2D>().position, Quaternion.identity);
	}
}
