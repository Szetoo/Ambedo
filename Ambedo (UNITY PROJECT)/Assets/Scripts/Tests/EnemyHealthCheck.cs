using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class EnemyHealthCheck
{


    private GameObject enemy;
    //private GameObject player;

    [SetUp]
    public void Setup()
    {
        enemy = Object.Instantiate(Resources.Load("Enemy2", typeof(GameObject))) as GameObject;
        //   player = Object.Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(enemy);

    }



    [UnityTest]
    public IEnumerator TestEnemyStartsWithCorrectHp()
    {

        float expectedhealth = 200;
        yield return null;
        Assert.AreEqual(expectedhealth, enemy.GetComponent<EnemyHealth>().currentHp);

    }


    [UnityTest]
    public IEnumerator enemy_Healing_Check()
    {


        float expectedhealth = 150;
        enemy.GetComponent<EnemyHealth>().receiveDamage(100);
        enemy.GetComponent<EnemyHealth>().healEnemy(50);
        yield return null;
        Assert.AreEqual(expectedhealth, enemy.GetComponent<EnemyHealth>().currentHp);
    }

    [UnityTest]
    public IEnumerator enemy_Receive_Damage_Check()
    {
        float expectedhealth = 150;
        enemy.GetComponent<EnemyHealth>().receiveDamage(50);
        yield return null;
        Assert.AreEqual(expectedhealth, enemy.GetComponent<EnemyHealth>().currentHp);
    }






}