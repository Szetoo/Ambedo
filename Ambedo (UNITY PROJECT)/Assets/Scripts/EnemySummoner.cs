using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummoner : MonoBehaviour
{
    public GameObject Enemy;
    public int AmountofEnemy;
    public float cooldownTime;

    private float cooldownRemain;

    // Start is called before the first frame update
    void Start()
    {
        cooldownRemain = cooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownReady()) if (AmountofEnemy > 0) SummonEnemy();
    }


    private bool CooldownReady()
    {
        if (cooldownRemain <= 0) return true;
        else cooldownRemain -= Time.deltaTime;
        return false;
    }

    private void ResetCooldown()
    {
        cooldownRemain = cooldownTime;
    }

    private void SummonEnemy()
    {

        Instantiate(Enemy, gameObject.transform.position, Quaternion.Euler(0, 0f, 0f));
        AmountofEnemy--;
        ResetCooldown();
    }
}
