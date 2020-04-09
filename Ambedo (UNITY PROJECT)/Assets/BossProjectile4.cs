using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile4 : MonoBehaviour
{

    public float projectileSpeed;
    public GameObject ImpactEffect;


    // Start is called before the first frame update
    void Start()
    {

        Destroy(gameObject, 15f);
    }

    private void OnDestroy()
    {
        GameObject impact = Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 0.9f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthController>().damagePlayer(100);
            DestroyImmediate(gameObject);

        }

    }
}

