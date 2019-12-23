using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWater : MonoBehaviour {

	public GameObject spawnHolder;
	public GameObject waterObject;
	public float length;
	public float jetRiseSpeed;
	public float jetLowerSpeed;
	public float waitTimeActive;
	public float waitTimeIdle;
	public float flowMagnitude;

	private float erectionIncrement = 0;

	// Use this for initialization
	void Start () {
		GameObject waterStream = Instantiate (waterObject, GetComponent<Transform> ());
		waterStream.transform.SetParent (GetComponent<Transform> ());

		waterStream.GetComponent<BuoyancyEffector2D> ().flowMagnitude = flowMagnitude;

		gameObject.transform.localScale = new Vector2 (0.5f, 0.1f);

		StartCoroutine (waterJetCycleCoroutine (waterStream));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator waterJetCycleCoroutine (GameObject waterStream) {

		Vector2 extended = new Vector2 (gameObject.transform.localScale.x, length);
		Vector2 contracted = new Vector2 (gameObject.transform.localScale.x, 0);
		Vector2 startingPos = new Vector2 (0, 0.5f);
		
		while (true) {



			while (gameObject.transform.localScale.y < length) {
				waterStream.transform.localPosition = startingPos;
				gameObject.transform.localScale = Vector2.Lerp (contracted, extended, erectionIncrement);
				erectionIncrement += (Time.deltaTime * jetRiseSpeed);
				yield return null;
			}

			yield return new WaitForSeconds (waitTimeIdle);
			while (gameObject.transform.localScale.y > 0) {
				gameObject.transform.localScale = contracted;
				gameObject.transform.localScale = Vector2.Lerp (contracted, extended, erectionIncrement);
				erectionIncrement -= (Time.deltaTime * jetLowerSpeed);
				yield return null;
			}
			yield return new WaitForSeconds (waitTimeIdle);
		}
	}
} 
