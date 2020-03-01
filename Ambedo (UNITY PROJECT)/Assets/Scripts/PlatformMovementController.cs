using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementController : MonoBehaviour

    
{
    
    public enum MovementType { horizontal, vertical, diagonal };
    public MovementType movementType;

    public Vector2 speed;
    public float travelTime;
    public float waitTime;

    private Vector3 position;
    private IEnumerator toUse;

    // Start is called before the first frame update

    private void Awake()
    {
        switch (movementType)
        {
            case MovementType.horizontal:
                speed.y = 0;
                break;
            case MovementType.vertical:
                speed.x = 0;
                break;
            case MovementType.diagonal:
                break;
            default:
                Debug.Log("No movement type selected");
                break;

        }
    }

    void Start()
    {
        toUse = movementCoroutine();
        StartCoroutine(toUse);

        Debug.Log(GetComponent<Transform>().transform.position);
    }


    private IEnumerator movementCoroutine()
    {
        while (true) {
            float timeUntilStop = Time.time + travelTime;
            while (Time.time < timeUntilStop)
            {
                gameObject.GetComponent<Transform>().Translate(new Vector3(speed.x * Time.fixedDeltaTime, speed.y * Time.fixedDeltaTime));
                yield return new WaitForEndOfFrame();

            }


            yield return new WaitForSeconds(waitTime);

            timeUntilStop = Time.time + travelTime;
            while (Time.time < timeUntilStop)
            {
                gameObject.GetComponent<Transform>().Translate(new Vector3(-speed.x * Time.fixedDeltaTime, -speed.y * Time.fixedDeltaTime));
                yield return new WaitForEndOfFrame();

            }

            yield return new WaitForSeconds(waitTime);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }


}
