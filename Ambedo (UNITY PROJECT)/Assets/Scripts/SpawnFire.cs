using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFire : MonoBehaviour {

	public GameObject spawnHolder;
	public GameObject fire;
	public float fireRate;
	public float sizeRange;
	public float angleRange;
	public float size;
	public float flameSpeed;
	public float cycleSeconds;
	public float waitTimer;


	private float nextFire;
	private float nextCycle;
	//private Collider2D thisCollider;
	private bool isInWater;
	private bool fireActive;


	// Use this for initialization
	void Start () {
		isInWater = false;
		fireActive = false;

		//Change this 
		if (spawnHolder.tag != "Player")
			StartCoroutine (spawnFireOnCoroutine());
	}
		
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (isInWater);
		if (Time.time > nextFire && !isInWater) {
			if (spawnHolder.tag == "Player" && Input.GetMouseButton(0)){
				spawnFireOnClick();
			}

			
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Water") {
			isInWater = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Water") {
			isInWater = false;
		}

	}

	void spawnFireOnClick(){
		nextFire = Time.time + fireRate;
		Vector3 position = gameObject.GetComponent<Transform> ().position;
		Quaternion rotation = gameObject.GetComponent<Transform> ().rotation;

		fire.GetComponent<FireController> ().flameSpeed = flameSpeed;
		fire.GetComponent<FireController> ().size = size;
		fire.GetComponent<FireController> ().sizeRange = sizeRange;
		fire.GetComponent<FireController> ().spawnHolder = spawnHolder;
		fire.GetComponent<FireController> ().angleRange = angleRange;

		Instantiate (fire, position, rotation);
	}

	IEnumerator spawnFireOnCoroutine(){
		while (true) {
			nextCycle = Time.time + cycleSeconds;
			while (Time.time < nextCycle) {
				fireActive = true;
				if (Time.time > nextFire && !isInWater)
					spawnFireOnClick ();
				yield return null;
			}
			fireActive = false;
			yield return new WaitForSeconds (waitTimer);
		}

	}


}
