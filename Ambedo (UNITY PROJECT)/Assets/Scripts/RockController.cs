using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    GameObject player;
    private float activationDistance;
    private bool hasBeenActivated;

    // Start is called before the first frame update

    private void Awake()
    {
        hasBeenActivated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        activationDistance = 30;


    }

    void Start()
    {
        hasBeenActivated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        activationDistance = 30;



    }

    // Update is called once per frame
    void Update()
    {
      
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated) 
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            hasBeenActivated = true;
            gameObject.transform.position = new Vector2(71.45f, 22.13f);
        }
        else if (other.gameObject.tag == "Player" && hasBeenActivated)
        {
            other.gameObject.GetComponent<PlayerHealthController>().currentHp = -100;
        }



    }
    

}
