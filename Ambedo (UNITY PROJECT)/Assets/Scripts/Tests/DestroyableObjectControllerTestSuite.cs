using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DestoryableObjectControllerTestSuite
{

    private GameObject playerHitBox;
    private GameObject destroyableWall;

    [SetUp]
    public void Setup()
    {
        destroyableWall = Object.Instantiate(Resources.Load("Destroyable Wall", typeof(GameObject))) as GameObject;
        playerHitBox = Object.Instantiate(Resources.Load("Player Attack Box", typeof(GameObject))) as GameObject;
        //playerHitBox.gameObject.tag = "PlayerAttackHitBox";
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(destroyableWall);
        Object.Destroy(playerHitBox);
    }


    //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Sword"));
    [UnityTest]
    public IEnumerator PlayerCouldNotDestroyWall()
    {
        bool existence = true;
        destroyableWall.transform.SetPositionAndRotation(new Vector3(-3, -18, 0), new Quaternion(0, 0, 0, 0));
        playerHitBox.transform.SetPositionAndRotation(new Vector3(-100, -18, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(1);
        if (destroyableWall)
        {
            Assert.AreEqual(existence, true);
        }
    }

    [UnityTest]
    public IEnumerator PlayerDestroyWall()
    {
        bool existence = false;
        destroyableWall.transform.SetPositionAndRotation(new Vector3(-3, -18, 0), new Quaternion(0, 0, 0, 0));
        playerHitBox.transform.SetPositionAndRotation(new Vector3(-3, -18, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(1);
        if (!destroyableWall)
        {
            Assert.AreEqual(existence, false);
        }

    }
}