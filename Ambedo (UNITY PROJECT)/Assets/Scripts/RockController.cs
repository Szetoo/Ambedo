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
        //DontDestroyOnLoad(gameObject);
        //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        hasBeenActivated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        activationDistance = 30;


    }

    void Start()
    {
        //gameObject.SetActive(false);
        hasBeenActivated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        activationDistance = 30;
        //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;


    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.x);
        if(gameObject.transform.position.x - player.transform.position.x > activationDistance)
            {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;


            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            //hasBeenActivated = true;
        }

       else if (player != null && !hasBeenActivated)
        {
            if (gameObject.transform.position.x - player.transform.position.x < activationDistance)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                hasBeenActivated = true;
            }
        }*/
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated) 
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            hasBeenActivated = true;
            //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.transform.position = new Vector2(71.45f, 22.13f);



            
            

            //other.gameObject.GetComponent<PlayerHealthController>().damage.Play();
        }
        else if (other.gameObject.tag == "Player" && hasBeenActivated)
        {
            other.gameObject.GetComponent<PlayerHealthController>().currentHP = -100;
        }



    }
    

}
