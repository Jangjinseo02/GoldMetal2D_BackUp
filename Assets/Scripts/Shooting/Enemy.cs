using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyScore;
    public string enemyname;
    public float speed;
    public int health;
    public Sprite[] sprites;
    float angle;

    SpriteRenderer spriteRenderer;

    public float maxShotDelay; //실제 딜레이
    public float curShotDelay; //한발 쏘고 다음 딜레이

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public GameManger manager;

    public ObjectManager objManager;
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    GameObject[] bullets;
    Vector3 nextPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyname == "B")
            anim = GetComponent<Animator>();
        bullets = new GameObject[100];
        nextPos = Vector3.right * 0.2f;
    }

    private void OnEnable()
    {
        switch (enemyname)
        {
            case "B":
                health = 200;
                Invoke("Stop", 2f);
                break;
            case "L":
                health = 40;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 3;
                break;
        }
    }
    void Stop()
    {
        if(gameObject.activeSelf)
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.zero;
            patternIndex = -1;
            Invoke("Think", 2f);
        }
          
    }
    void Think()
    {
        if (gameObject.activeSelf)
        {
            patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
            curPatternCount = 0;

            switch (patternIndex)
            {
                case 0:
                    FireFoward();
                    break;
                case 1:
                    FireShot();
                    break;
                case 2:
                    FireArc();
                    break;
                case 3:
                    FireAround();
                    break;
            }
        }
    }
    void FireFoward()
    {
        for (int i = 0; i < 4; i++)
        {
            bullets[i] = i == 1 ? objManager.MakeObj("BossBulletA") : objManager.MakeObj("BossBulletA");
            if (i == 0)
                bullets[i].transform.position = transform.position + Vector3.left * 0.2f;
            else
                bullets[i].transform.position = bullets[i - 1].transform.position + nextPos;
            bullets[i].transform.rotation = Quaternion.identity;

            Rigidbody2D rigidB = bullets[i].GetComponent<Rigidbody2D>();
            rigidB.AddForce(Vector2.down * 7, ForceMode2D.Impulse);
        }

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            curPatternCount += 1;
            Invoke("FireFoward", 2f);
        }
        else
        {
            Invoke("Think", 3f);
        }

    }

    void FireShot()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject bullet = objManager.MakeObj("BossBulletB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }


        if (curPatternCount < maxPatternCount[patternIndex])
        {
            curPatternCount += 1;
            Invoke("FireShot", 3.5f);
        }
        else
        {
            Invoke("Think", 3f);
        }

    }

    void FireArc()
    {
        GameObject bullet = objManager.MakeObj("BossBulletB");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 10 * curPatternCount/maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            curPatternCount += 1;
            Invoke("FireArc", 0.15f);

        }
        else
        {
            Invoke("Think", 3f);
        }

    }
    void FireAround()
    {
        int rounNumA = 30;
        int rounNumB = 40;
        int rounNumC = curPatternCount % 2 == 0 ? rounNumA : rounNumB;

        for(int i = 0; i < rounNumC; i++)
        {
            GameObject bullet = objManager.MakeObj("BossBulletA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / rounNumC), Mathf.Sin(Mathf.PI * 2 * i / rounNumC));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * i / rounNumC + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            curPatternCount += 1;
            Invoke("FireAround", 0.7f);

        }
        else
        {
            Invoke("Think", 3f);
        }

    }


    void Update()
    {
        if (enemyname == "B")
            return;
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;
        if(enemyname == "S")
        {
            GameObject bullet = objManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if(enemyname == "L")
        {
            GameObject bulletR = objManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.2f;
            GameObject bulletL = objManager.MakeObj("bulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.2f;

            Vector2 target = player.transform.position;
            angle = Mathf.Atan2(target.y - bulletR.transform.position.y, target.x - bulletR.transform.position.x) * Mathf.Rad2Deg;
            bulletR.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);   //LookAt 대신할 부분 총알이 플레이어를 바라보도록 회전


            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position - Vector3.right * 0.2f);
            Vector3 dirVecL = player.transform.position - (transform.position - Vector3.left * 0.2f);
           

            rigidR.AddForce(dirVecR.normalized * 3, ForceMode2D.Impulse);            
            rigidL.AddForce(dirVecL.normalized * 3, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        if (enemyname == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];

            Invoke("ReturnSprite", 0.1f);            
        }

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //아이템 드랍
            int ran = enemyname == "B" ? 0 : Random.Range(0, 10);

            if (ran < 3)
            {
                Debug.Log("Not Item");
            }
            else if (ran < 6)
            {
                GameObject itemCoin = objManager.MakeObj("itemCoin");
                itemCoin.transform.position = transform.position;
            }
            else if (ran < 8)
            {
                GameObject itemPower = objManager.MakeObj("itemPower");
                itemPower.transform.position = transform.position;
            }
            else if (ran < 10)
            {
                GameObject itemBoom = objManager.MakeObj("itemBoom");
                itemBoom.transform.position = transform.position;
            }
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            manager.CallExplosion(transform.position, enemyname);

            if (enemyname == "B")
                manager.StageEnd();
        }

    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyname != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);

            collision.gameObject.SetActive(false);
        }

    }

}
