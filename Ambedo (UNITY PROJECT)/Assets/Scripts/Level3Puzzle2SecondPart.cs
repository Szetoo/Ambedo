using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Puzzle2SecondPart : MonoBehaviour
{
    public GameObject EnemySummoner;
    public GameObject DoorBreak;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rockBreakDoor")
        {
            DoorBreak.SetActive(false);
            EnemySummoner.SetActive(true);
        }

    }
}
