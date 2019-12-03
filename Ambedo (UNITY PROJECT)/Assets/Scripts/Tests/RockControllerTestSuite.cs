using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RockControllerTestSuite
{

    private GameObject player; 
    private GameObject boulder;

    [SetUp]
    public void Setup()
    {
        player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
        boulder = Object.Instantiate(Resources.Load("boulder", typeof(GameObject))) as GameObject;
        player.gameObject.tag = "Player";
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(boulder);
    }

    [UnityTest]
    public IEnumerator EventNotBeenActivated()
    {
        bool expectedWield = false;
        float expectedDistance = 30;
        yield return null;
        Assert.AreEqual(expectedWield, boulder.GetComponent<RockController>().hasBeenActivated);
        Assert.AreEqual(expectedDistance, boulder.GetComponent<RockController>().activationDistance);
    }
    [UnityTest]
    public IEnumerator EventTriggered()
    {
        bool expectedWield = true;
        player.transform.SetPositionAndRotation(new Vector3(47, 6, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(1);
        Assert.AreEqual(expectedWield, boulder.GetComponent<RockController>().hasBeenActivated);
    }

    [UnityTest]
    public IEnumerator PlayerObjectExistenceCheck()
    {
        bool existence = false;
        player.transform.SetPositionAndRotation(new Vector3(47, 6, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(2);
        if (!player)
        {
            Assert.AreEqual(existence, false);
        }
    }
}


