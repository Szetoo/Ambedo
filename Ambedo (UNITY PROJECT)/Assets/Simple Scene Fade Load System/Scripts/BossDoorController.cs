using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : MonoBehaviour
{
    public int step = 0;
    public GameObject door;
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (step)
        {
            case 0:
                break;
            case 1:
                DoorUp();
                break;
            case 2:
                DoorDown();
                break;

        }
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            step = 2;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().enabled = false;

           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        step = 1;
    }
   
 


    void DoorUp()
    {
        if (door.transform.position.y < -90.5)
        {
            door.transform.position = door.transform.position + new Vector3(0, 0.1f, 0);

        }
        else
        {
            step = 0;
            if (boss != null)
            {
                boss.SetActive(true);
            }
        }



    }


    void DoorDown()
    {

      
        if (door.transform.position.y > -93.5)
        {
            door.transform.position = door.transform.position - new Vector3(0, 0.1f, 0);

        }

    }
}
