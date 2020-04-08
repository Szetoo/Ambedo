using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRoomLock : MonoBehaviour
{
    public GameObject doorOpen;
    public GameObject player;
    public GameObject Boss;
    private bool active = false;
    private bool closeDoor = false;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            OpenDoor();
            if (player.transform.position.y < -34f && player.transform.position.x > 302f)
            {
                closeDoor = true;
                active = false;
            }
        }
        if (closeDoor) CloseDoor();
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")  active = true; 
    }


    public void OpenDoor()
    {
        if (doorOpen.transform.position.x >= 320.9f) doorOpen.transform.position -= new Vector3(0.1f, 0, 0);
    }

    public void CloseDoor()
    {

        if (doorOpen.transform.position.x <= 325.7f) doorOpen.transform.position += new Vector3(0.1f, 0, 0);
        else Boss.SetActive(true);
    }
}
