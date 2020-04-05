using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerController : MonoBehaviour
{
    public float speed;
    public float angle;

    public float fireRate;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
        fireRate = 1 / fireRate;
        StartCoroutine(spawn(fireRate)); 
    }

    IEnumerator spawn(float timeToWait)
    {
        while (true)
        {
            Vector3 position = gameObject.GetComponent<Transform>().position;
            Quaternion rotation = gameObject.GetComponent<Transform>().rotation;

            projectile.GetComponent<ProjectileMovementController>().speed = speed;
            projectile.GetComponent<ProjectileMovementController>().angle = angle;


            Instantiate(projectile, position, rotation);
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
