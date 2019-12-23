using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour {

	public GameObject holder;
	public float flipOffset;

	private Vector3 offset;
	private bool isFacingRight;
	private float rot;

	// Use this for initialization
	void Start () {
		//offset = transform.position - holder.transform.position;
		offset = new Vector3(0,0);
		isFacingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		worldMousePosition.z = 0;

		Quaternion mouseRotation = rotateToPoint (worldMousePosition);

		transform.SetPositionAndRotation (holder.transform.position + offset, mouseRotation);

		if ((isFacingRight && (rot > 90.0f || rot < -90.0f)) || (!isFacingRight && (rot < 90.0f && rot > -90.0f))) {
			flip (holder, true);
		}

	}

	Quaternion rotateToPoint (Vector3 point) {
		
		double xDiff = (double)point.x - transform.position.x;
		double yDiff = (double)point.y - transform.position.y;
		rot = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

		return Quaternion.AngleAxis(rot, new Vector3(0, 0, 1));
	}

	void flip(GameObject toFlip, bool flipChildren) {
		isFacingRight = !isFacingRight;
		toFlip.GetComponent<SpriteRenderer> ().flipX = !isFacingRight;

		//Needs to be cleaned up/fixed
		if (flipChildren) {
			Vector3 shiftObject = GetComponentInChildren<Transform> ().position;
			shiftObject.x += flipOffset;
			flipOffset = -flipOffset;

		}
			
	}

//	void UserFunction () {
//		float TK1;
//		float TK2;
//		float TK3;
//
//		float[] myArray = { TK1, TK2, TK3 };
//
//		for (int i = 0; i < myArray.Length; i++) {
//			if (myArray [i] < 2.55 || myArray [i] > 2.95)
//				return 1;
//			else
//				return 0;
//		}
//	}
}
