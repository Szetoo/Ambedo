using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

	public GameObject mouseReplacement;
	public GameObject player;
	public float range;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		worldMousePosition.z = 0;

		Vector3 playerPosition = player.GetComponent<Transform>().position;
		Vector3 outerLimit = (worldMousePosition - playerPosition).normalized * range;

		if (Vector3.Distance (playerPosition, worldMousePosition) <= range) {
			gameObject.transform.position = worldMousePosition;

		} else {
			gameObject.transform.position = outerLimit + playerPosition;


		}
		mouseReplacement.transform.position = worldMousePosition;


	}
}
