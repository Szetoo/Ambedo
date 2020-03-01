using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyDoorLock : MonoBehaviour
{

    public GameObject lockCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
