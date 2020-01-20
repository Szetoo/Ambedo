using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    public float projectileSpeed;
    public GameObject ImpactEffect;
    public Rigidbody2D rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.up * projectileSpeed * -1;
        Destroy(gameObject, 4.7f);
    }

    private void OnDestroy()
    {
        GameObject impact = Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        //Destroy(collision.gameObject);
        Destroy(impact, 0.9f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {        
            Destroy(gameObject);    
        }
        
    }
}
 
