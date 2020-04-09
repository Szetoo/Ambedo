using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryPlat : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject projectile;
    public GameObject pos;

    private Quaternion rot1;
    private Quaternion rot2;
    private Quaternion rot3;
    private Quaternion rot4;
    private Quaternion rot5;



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boss")
            Destroy(gameObject);
    }


    void OnDestroy()
    {
        // if the platform is destory it will fire 5 projectiles  
        rot1 = Quaternion.Euler(0, 0f, 180f);
        rot2 = Quaternion.Euler(0, 0f, 270f);
        rot3 = Quaternion.Euler(0, 0f, 130f);
        rot4 = Quaternion.Euler(0, 0f, 220f);
        rot5 = Quaternion.Euler(0, 0f, 330f);
        sumorProjectile(rot1);
        sumorProjectile(rot2);
        sumorProjectile(rot3);
        sumorProjectile(rot4);
        sumorProjectile(rot5);
    }


    private void sumorProjectile(Quaternion rotation)
    {

        Instantiate(projectile, pos.transform.position, rotation);

    }



}




