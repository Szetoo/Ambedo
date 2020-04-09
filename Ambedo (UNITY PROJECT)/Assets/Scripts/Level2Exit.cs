using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Exit : MonoBehaviour
{

    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss2AndPlayer")
        {
            Destroy(boss);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }

    }
}
