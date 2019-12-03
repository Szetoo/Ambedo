using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerHealthTestSuite
{

    private GameObject enemy;
    private GameObject player;

    [SetUp]
    public void Setup()
    {
        //enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        //Object.Destroy(enemy);
        Object.Destroy(player);
    }


    //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Sword"));
    [UnityTest]
    public IEnumerator TestPlayerStartsWithCorrectHp()
    {
        //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("../Prefabs/Droppable Objects/Sword"));

        float expectedhealth = 200;

        yield return null;


        Assert.AreEqual(expectedhealth, player.GetComponent<PlayerHealthController>().currentHp);
        //yield return null;
    }

    public IEnumerator TestPlayerHealthCheckInvincibility()
    {
        // Check that the player is invincible after taking damage

        //GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Enemy"));
        //GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));
        enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       enemy.GetComponent<Transform>().position.x - 1,
                                                                       enemy.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        player.GetComponent<PlayerMovementController>().attack();

        float healthBefore = enemy.GetComponent<BossHealth>().currentHp;
        yield return new WaitForSeconds(0.8f);
        player.GetComponent<PlayerMovementController>().attack();

        float healthAfter = enemy.GetComponent<BossHealth>().currentHp;

        Assert.AreEqual(healthBefore, healthAfter);

        
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerHealthCheckTakesDamage()
    {
        // Check that the boss takes damage upon being hit



        player.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       enemy.GetComponent<Transform>().position.x - 1,
                                                                       enemy.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        float healthBefore = enemy.GetComponent<BossHealth>().currentHp;
        player.GetComponent<PlayerMovementController>().attack();

        float healthAfter = enemy.GetComponent<BossHealth>().currentHp;

        Assert.Less(healthAfter, healthBefore);

        Object.Destroy(enemy);
        Object.Destroy(player);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerHealthCheckHealthRegen()
    {
        // Check that the boss regenerates health after several seconds

        GameObject bossObject = Object.Instantiate(Resources.Load<GameObject>("Boss"));
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));

        playerObject.GetComponent<Transform>().SetPositionAndRotation(new Vector3(
                                                                       enemy.GetComponent<Transform>().position.x - 1,
                                                                       enemy.GetComponent<Transform>().position.y),
                                                                       Quaternion.identity);

        float healthBefore = enemy.GetComponent<BossHealth>().currentHp;
        player.GetComponent<PlayerMovementController>().attack();
        yield return new WaitForSeconds(5);
        float healthAfter = enemy.GetComponent<BossHealth>().currentHp;

        Assert.Less(healthAfter, healthBefore);

     
        yield return new WaitForEndOfFrame();

    }

}


