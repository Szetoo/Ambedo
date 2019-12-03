using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DeathZoneControllerTestSuite
{
    private GameObject player;
    private GameObject deathZone;

    [SetUp]
    public void Setup()
    {
        player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
        deathZone = Object.Instantiate(Resources.Load("DeadZone", typeof(GameObject))) as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(deathZone);
    }


    [UnityTest]
    public IEnumerator PlayerNotTouchDeathZone()
    {

        float expectedHealth = 200;
        player.transform.SetPositionAndRotation(new Vector3(-3, 0, 0), new Quaternion(0, 0, 0, 0));
        yield return null;

        Assert.AreEqual(expectedHealth, player.GetComponent<PlayerHealthController>().currentHp);
    }

    [UnityTest]
    public IEnumerator PlayerTouchDeathZone()
    {
        bool existence = false;
        player.transform.SetPositionAndRotation(new Vector3(-3, -18, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(2);
        if (!player)
        {
            Assert.AreEqual(existence, false);
        }
        
    }
}