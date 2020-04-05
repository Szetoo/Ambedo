using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementController : MonoBehaviour
{
    public float speed;
    public float angle;
    public float timeToLive;

    private float nextCycle;
    private float timeUntilExpiry;


    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilExpiry = Time.time + timeToLive;

        rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.right;
        Debug.Log(direction);
        rb.velocity = speed * direction;
    }

    private void Update()
    {
        if (Time.time > timeUntilExpiry) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
}
