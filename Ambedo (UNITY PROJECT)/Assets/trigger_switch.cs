using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_switch : MonoBehaviour
{

    public GameObject activeObject;
    public GameObject deactiveLockObject;
    public GameObject lockObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            activeObject.SetActive(true);
            deactiveLockObject.SetActive(false);
            Destroy(lockObject);
            Destroy(gameObject);
        }
    }

}
 

