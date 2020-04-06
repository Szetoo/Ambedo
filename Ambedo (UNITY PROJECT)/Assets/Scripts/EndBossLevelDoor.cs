using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBossLevelDoor: MonoBehaviour
{

    public GameObject lockCollider;
    public GameObject boss;
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null)
        {
            door.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(boss);
            Destroy(gameObject);
        }

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
