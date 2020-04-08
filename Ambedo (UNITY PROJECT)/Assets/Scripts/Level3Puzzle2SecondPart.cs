using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Puzzle2SecondPart : MonoBehaviour
{
    public GameObject EnemySummoner;
    public GameObject DoorBreak;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rockBreakDoor")
        {
            DoorBreak.SetActive(false);
            EnemySummoner.SetActive(true);
        }

    }
}
