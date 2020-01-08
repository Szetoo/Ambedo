using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour {

    public GameObject hitbox;
    private bool attackDelay;
    private IEnumerator attackCoroutine;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    

    // Update is called once per frame
    private void LateUpdate()
    {
        try
        {
            if (Input.GetMouseButtonDown(0))
            {
                string heldObjectTag = gameObject.transform.GetChild(0).tag;
                if (heldObjectTag == "Sword" && !attackDelay)
                {
                    attackCoroutine = attack();
                    StartCoroutine(attackCoroutine);
                }
            }
        }
        catch (UnityException)
        {
            Debug.Log("Not holding an object");
        }
    }

    public IEnumerator attack()
    {
        //float timeToDespawn = Time.time + (1 / 6f);
        //while (timeToDespawn > Time.time)
        // {

        // }
        Debug.Log("Attack Starts");
        attackDelay = true;
        GameObject tempHitbox = Instantiate(hitbox, gameObject.GetComponent<Transform>());
        if (sprite.flipX)
        {
            tempHitbox.transform.localPosition = new Vector2(1 / 3f, 0.04f);
        }
        else
        {
            tempHitbox.transform.localPosition = new Vector2(-1 / 3f, 0.04f);
        }

        yield return new WaitForSeconds(0.4f);
        Destroy(tempHitbox);

        yield return new WaitForSeconds(0.4f);
        attackDelay = false;
        Debug.Log("Attack Ends");
    }
}
