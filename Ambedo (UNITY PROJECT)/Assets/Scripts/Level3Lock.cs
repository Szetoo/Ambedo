using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Lock : MonoBehaviour
{
    public int Code;
    public GameObject LockObject;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttackHitbox")
        {
            LockObject.GetComponent<level3PuzzleCode>().CheckPassword(Code);
        }
        
    }
}
