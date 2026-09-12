using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject BossPrefab;
    public GameObject enemeyLPrefab;
    public GameObject enemeyMPrefab;
    public GameObject enemeySPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject bulletPlayrAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject followerBulletPrefab;
    public GameObject BossBulletAPrefab;
    public GameObject BossBulletBPrefab;
    public GameObject ExplosionPrefab;

    GameObject[] Boss;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayrA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] followerBullet;
    GameObject[] BossBulletA;
    GameObject[] BossBulletB;

    GameObject[] explosion;

    GameObject[] targetPool;

    void Awake()
    {
        Boss = new GameObject[1];
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayrA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        followerBullet = new GameObject[100];
        BossBulletA = new GameObject[300];
        BossBulletB = new GameObject[100];
        explosion = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < Boss.Length; i++)
        {
            Boss[i] = Instantiate(BossPrefab);
            Boss[i].SetActive(false);
        }
        for (int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemeyLPrefab);
            enemyL[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemeyMPrefab);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemeySPrefab);
            enemyS[i].SetActive(false);
        }

        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        for (int i = 0; i < bulletPlayrA.Length; i++)
        {
            bulletPlayrA[i] = Instantiate(bulletPlayrAPrefab);
            bulletPlayrA[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayerB.Length; i++)
        {
            bulletPlayerB[i] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyA.Length; i++)
        {
            bulletEnemyA[i] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyB.Length; i++)
        {
            bulletEnemyB[i] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[i].SetActive(false);
        }
        for (int i = 0; i < followerBullet.Length; i++)
        {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }
        for (int i = 0; i < BossBulletA.Length; i++)
        {
            BossBulletA[i] = Instantiate(BossBulletAPrefab);
            BossBulletA[i].SetActive(false);
        }
        for (int i = 0; i < BossBulletB.Length; i++)
        {
            BossBulletB[i] = Instantiate(BossBulletBPrefab);
            BossBulletB[i].SetActive(false);
        }
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i] = Instantiate(ExplosionPrefab);
            explosion[i].SetActive(false);
        }
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "Boss":
                targetPool = Boss;
                break;
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayrA":
                targetPool = bulletPlayrA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "BossBulletA":
                targetPool = BossBulletA;
                break;
            case "BossBulletB":
                targetPool = BossBulletB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }

        return targetPool;
    }


    public GameObject MakeObj(string type)
    {

        switch (type)
        {
            case "Boss":
                targetPool = Boss;
                break;
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayrA":
                targetPool = bulletPlayrA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "BossBulletA":
                targetPool = BossBulletA;
                break;
            case "BossBulletB":
                targetPool = BossBulletB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (targetPool[i].activeSelf != true)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }


        return null;
    }
}
