using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomZone : MonoBehaviour
{
    public bool playerNearby;
    public GameObject boss;
    public Camera cam;
    public GameObject map;
    public AudioSource bossMusic;
    private bool hasBeenTriggered;

    // Use this for initialization

    void Start()
    {
        playerNearby = false;
        hasBeenTriggered = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bossMusic.Play();
            cam.GetComponent<AudioSource>().enabled = false;
            playerNearby = true;
            boss.SetActive(true);
             map.transform.localScale = new Vector3(4.687737f, 5.408163f, 1f);
            cam.orthographicSize = 15.0f;
         
            cam.GetComponent<CustomCamera.CameraMovement>().lowerDeadBound = 12f;
            cam.transform.localPosition = new Vector3(cam.transform.position.x, -16f,-10f);
            // objectWithOtherScript.GetComponent.< CameraMovement > ().lowerDeadBound = someNumber;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

 
}
