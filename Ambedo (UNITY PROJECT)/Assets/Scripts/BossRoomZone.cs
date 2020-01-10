using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomZone : MonoBehaviour
{
    public bool playerNearby;
    public GameObject boss;
    public Camera cam;

    // Use this for initialization

    void Start()
    {
        playerNearby = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNearby = true;
            boss.SetActive(true);
            cam.orthographicSize = 15.0f;
         
            cam.GetComponent<CustomCamera.CameraMovement>().lowerDeadBound = 12f;
            cam.transform.localPosition = new Vector3(cam.transform.position.x, -16f,-10f);
            // objectWithOtherScript.GetComponent.< CameraMovement > ().lowerDeadBound = someNumber;
        }
    }

 
}
