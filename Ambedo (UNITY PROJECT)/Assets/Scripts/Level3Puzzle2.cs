using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Puzzle2 : MonoBehaviour
{

    public GameObject Pusher;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active) if (Pusher.transform.position.x <= 152.6f) Pusher.transform.position += new Vector3(0.3f,0,0);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHitbox")
        {
            active = true;
        }

    }
}
