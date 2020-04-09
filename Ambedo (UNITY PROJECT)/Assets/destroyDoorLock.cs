using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyDoorLock : MonoBehaviour
{

    public GameObject lockCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "rockBreakDoor")
        {
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        Destroy(lockCollider);
    }
}
