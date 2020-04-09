using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerOnHead : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.GetComponent<boss2AI>().playerOnBoss = true;
            transform.parent.transform.Find("PlayerAndBoss").gameObject.SetActive(true);
        }

    }
}
