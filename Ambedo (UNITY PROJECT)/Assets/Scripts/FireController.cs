using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	public float lifespanFrames;

	public float size;
	public float sizeRange;
	public float flameSpeed;
	public float angleRange;
	public float momentumMultiplier;
	public float riseSpeed;
	public float maxVel;
	public GameObject spawnHolder;

	private Quaternion rotation;
	private Color thisColour;
	private float sizeMultiplier;

	void Start () {
		thisColour = GetComponent<SpriteRenderer> ().color;

		size = Random.Range (size - sizeRange, size + sizeRange);

		sizeMultiplier = 0;
		changeSize (0);

		Vector3 holderVelocity = spawnHolder.GetComponent<Rigidbody2D> ().velocity;

		Vector3 spread = Quaternion.Euler (0, 0, Random.Range (-angleRange, angleRange)) * transform.right;

		Vector3 rise = new Vector3 (0, riseSpeed);
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.velocity = spread * flameSpeed + (momentumMultiplier * holderVelocity) + rise;
	}
	
	// Update is called once per frame
	void Update () {
		changeVisibility ();
		if (sizeMultiplier < 1)
			changeSize (0.05f);
	}

	void FixedUpdate() {
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

	void movement () {
		riseSpeed *= riseSpeed;

		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		if (rb.velocity.y > maxVel)
			rb.velocity = new Vector3 (rb.velocity.x, maxVel);
		if (rb.velocity.x > maxVel)
			rb.velocity = new Vector3 (maxVel, rb.velocity.y);
		if (rb.velocity.y < -maxVel)
			rb.velocity = new Vector3 (rb.velocity.x, -maxVel);
		if (rb.velocity.x < -maxVel)
				rb.velocity = new Vector3 (-maxVel, rb.velocity.y);
	}

	void changeSize(float amount) {
		float toScaleBy = size * sizeMultiplier;
		gameObject.GetComponent<Transform> ().localScale = new Vector3 (toScaleBy, toScaleBy);
		sizeMultiplier += amount;
	}

}
