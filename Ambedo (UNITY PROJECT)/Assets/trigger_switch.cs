using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_switch : MonoBehaviour
{

    public GameObject activeObject;
    public GameObject deactiveLockObject;
    public GameObject lockObject;
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
        if (other.gameObject.tag == "PlayerAttackHitbox")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        activeObject.SetActive(true);
        Destroy(lockObject);

        deactiveLockObject.SetActive(false);
    }
}

