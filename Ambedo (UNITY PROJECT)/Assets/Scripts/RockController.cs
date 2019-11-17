using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    GameObject player;
    private float activationDistance;
    private bool hasBeenActivated;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        hasBeenActivated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        activationDistance = 30;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !hasBeenActivated)
        {
            if (gameObject.transform.position.x - player.transform.position.x < activationDistance)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                hasBeenActivated = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude >0) 
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.GetComponent<PlayerHealthController>().currentHp = -100;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            

            //other.gameObject.GetComponent<PlayerHealthController>().damage.Play();
        }
        

    }

}
