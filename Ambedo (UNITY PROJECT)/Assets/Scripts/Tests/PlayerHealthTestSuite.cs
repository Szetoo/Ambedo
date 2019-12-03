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
        if (player != null)
        {
            Object.Destroy(player);
        }
    }


    [UnityTest]
    public IEnumerator TestPlayerStartsWithCorrectHp()
    {

        float expectedhealth = 200;
        yield return null;
        Assert.AreEqual(expectedhealth, player.GetComponent<PlayerHealthController>().currentHp);
        //yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerHealthCheckInvincibility()
    {

        enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        bool expectedState = true;
        yield return new WaitForSeconds(0.8f);
        Assert.AreEqual(expectedState, player.GetComponent<PlayerHealthController>().invincible);
        Object.Destroy(enemy);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerHealthCheckTakesDamage()
    {
        enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        float expectedhealth = 100;
        yield return new WaitForSeconds(0.8f);
        Assert.AreEqual(expectedhealth, player.GetComponent<PlayerHealthController>().currentHp);
        Object.Destroy(enemy);
        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerHealthCheckHealthRegen()
    {

        enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        float expectedhealth = 200;
        yield return new WaitForSeconds(10);


        Assert.AreEqual(expectedhealth, player.GetComponent<PlayerHealthController>().currentHp);
        Object.Destroy(enemy);


        yield return new WaitForEndOfFrame();

    }

    [UnityTest]
    public IEnumerator TestPlayerDies()
    {

        enemy = Object.Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));


        yield return new WaitForSeconds(5);
        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        enemy.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(1);


        Assert.IsNull(player);
        Object.Destroy(enemy);


        yield return new WaitForEndOfFrame();

    }

}


