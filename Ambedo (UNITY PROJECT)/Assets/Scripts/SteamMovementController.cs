using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamMovementController : MonoBehaviour {

	public float horizontalVelocity;
	public float verticalVelocity;
	public float lifespanFrames;

	private Color thisColour;

	// Use this for initialization
	void Start () {
		thisColour = GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		changeVisibility ();
		movement ();
	}

	public void changeVisibility() {
		float visibility = GetComponent<SpriteRenderer> ().color.a;
		visibility -= (1 / lifespanFrames);
		if (visibility <= 0)
			Destroy(gameObject);
		GetComponent<SpriteRenderer> ().color = new Color (
			thisColour.r,
			thisColour.g,
			thisColour.b,
			visibility);
		
	}

	void movement(){
		Vector3 currentPosition = GetComponent<Transform> ().position;
		GetComponent<Transform> ().position = new Vector2 (
			currentPosition.x + Random.Range(-horizontalVelocity, horizontalVelocity),
			currentPosition.y + verticalVelocity);
	}
}
