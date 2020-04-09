using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Level3to4Transition : MonoBehaviour
{
    private bool hasBeenTriggered;
    // Start is called before the first frame update
    void Start()
    {
        hasBeenTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" & !hasBeenTriggered)
        {
            gameObject.GetComponent<PlayableDirector>().Play();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            hasBeenTriggered = true;
        }


    }
}