using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PickUpObjectsTestSuite {

    private GameObject sword;
    private GameObject player;

    [SetUp]
    public void Setup()
    {
        sword = Object.Instantiate(Resources.Load("Sword", typeof(GameObject))) as GameObject;
        player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(sword);
        Object.Destroy(player);
    }


    //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Sword"));
    [UnityTest]
    public IEnumerator TestPlayerStartsNotWielding()
    {
        //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("../Prefabs/Droppable Objects/Sword"));

        bool expectedWield = false;

        yield return null;


        Assert.AreEqual(expectedWield, sword.GetComponent<PickUpByPlayer>().beingHeld);
        //yield return null;
    }

    [UnityTest]
    public IEnumerator TestCannotPickUpIfTooFar()
    {
        //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("../Prefabs/Droppable Objects/Sword"));

        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        sword.transform.SetPositionAndRotation(new Vector3(2, 0, 0), new Quaternion(0, 0, 0, 0));
        float maxDistance = sword.GetComponent<PickUpByPlayer>().maxDistance;
        float distance = Vector3.Distance(player.GetComponent<Transform>().position, sword.GetComponent<Transform>().position);
        yield return null;
        Assert.Greater(distance, maxDistance);

    }

    [UnityTest]
    public IEnumerator TestCanPickUpIfWithinMaxDistance()
    {
        //GameObject sword = Object.Instantiate(Resources.Load<GameObject>("../Prefabs/Droppable Objects/Sword"));

        player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        sword.transform.SetPositionAndRotation(new Vector3(0.5f, 0, 0), new Quaternion(0, 0, 0, 0));
        float maxDistance = sword.GetComponent<PickUpByPlayer>().maxDistance;
        float distance = Vector3.Distance(player.GetComponent<Transform>().position, sword.GetComponent<Transform>().position);
        yield return null;
        Assert.Less(distance, maxDistance);

    }

    [UnityTest]
    public IEnumerator TestPickedUpItem()
    {
        sword.GetComponent<PickUpByPlayer>().pickUp();
        //gameObject.transform.SetParent(player.GetComponent<Transform>()
        bool isHeld = true;
        yield return null;
        Assert.AreEqual(sword.GetComponent<PickUpByPlayer>().beingHeld, isHeld);

    }

    [UnityTest]
    public IEnumerator TestDroppedItem()
    {
        sword.GetComponent<PickUpByPlayer>().drop();
        bool isHeld = false;
        yield return null;
        Assert.AreEqual(sword.GetComponent<PickUpByPlayer>().beingHeld, isHeld);

    }
}


