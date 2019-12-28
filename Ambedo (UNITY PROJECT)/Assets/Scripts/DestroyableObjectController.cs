using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If player attack hitbox touches gameobject, destroy the object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            Destroy(gameObject);
        }
    }
}
