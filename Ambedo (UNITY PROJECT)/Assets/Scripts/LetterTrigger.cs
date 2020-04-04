using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTrigger : MonoBehaviour
{
    public int code;
    public GameObject Boss;
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
        if (collision.tag == "PlayerAttackHitbox")
        {
          //  gameObject.transform.position = gameObject.transform.position - new Vector3(-1, 0, 0);
            Debug.Log(code);
            Boss.GetComponent<Boss4AI>().puzzleFunction(code);
            
        }

    }
}
