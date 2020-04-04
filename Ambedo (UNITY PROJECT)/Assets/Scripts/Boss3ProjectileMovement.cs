using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ProjectileMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float projectileSpeed;
    public GameObject ImpactEffect;
    private Rigidbody2D rigidBody;


    void Start()
    {
      //  rigidBody = GetComponent<Rigidbody2D>();
       // rigidBody.velocity = transform.up * 15f + transform.right * 6f ;
       // rigidBody.AddForce(new Vector2(Random.Range(0.5f, 3f), Random.Range(8f, 12f) ));
        Destroy(gameObject, 5f);
    }

    //private void OnDestroy()
    //{
    //   // GameObject impact = Instantiate(ImpactEffect, transform.position, Quaternion.identity);
    //    //Destroy(collision.gameObject);
    //    //Destroy(impact, 0.9f);
    //}




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthController>().damagePlayer(100);
            Destroy(gameObject);
        }

    }
}
