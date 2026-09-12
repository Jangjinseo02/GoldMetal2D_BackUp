using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;

    public int life;
    public int score;

    public float speed = 5;
    public int power;
    public int maxPower;

    public int boom;
    public int maxBoom;

    public bool IsTouchTop;
    public bool IsTouchBottom;
    public bool IsTouchLeft;
    public bool IsTouchRight;

    public bool isHit;
    public bool isBoomTime;

    public float maxShotDelay; //Ω«¡¶ µÙ∑π¿Ã
    public float curShotDelay; //«—πﬂ ΩÓ∞Ì ¥Ÿ¿Ω µÙ∑π¿Ã

    public GameObject boomEffect;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameManger manager;
    public ObjectManager objManager;
    public GameObject[] followers;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;
    public bool isRespawnTime;
    SpriteRenderer sprite;

    GameObject[] bullets;
    Vector3 nextPos;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        bullets = new GameObject[maxPower];
        nextPos = Vector3.right * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
        Fire();
        Boom();
        Reload();
    }
    private void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3f);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            
            for(int i = 0; i < followers.Length; i++)
            {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);

            for (int i = 0; i < followers.Length; i++)
            {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void JoyPanel(int type)
    {
        for(int i = 0; i < 9; i++)
        {
            joyControl[i] = i == type;
        }
    }

    public void JoyDawn()
    {
        isControl = true;
    }
    public void JoyUp()
    {
        isControl = false;
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }

        if ((IsTouchRight && h == 1) || (IsTouchLeft && h == -1) || !isControl)
            h = 0;

        if ((IsTouchTop && v == 1) || (IsTouchBottom && v == -1) || !isControl)
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            anim.SetInteger("Input", (int)h);
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }
    public void ButtonAUp()
    {
        isButtonA = false;
    }
    public void ButtonBDown()
    {
        isButtonB = true;
    }

    void Fire()
    {
        //if (!isButtonA)
        //    return;
        if (!Input.GetButton("Fire1"))
            return;
        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                GameObject bullet = objManager.MakeObj("bulletPlayrA");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                
                for (int i = 0; i < 2; i++)
                {
                    bullets[i] = objManager.MakeObj("bulletPlayrA");
                    if(i == 0)
                        bullets[i].transform.position = transform.position + Vector3.left * 0.2f;
                    else
                        bullets[i].transform.position = bullets[i - 1].transform.position + nextPos * 2;
                    Rigidbody2D rigidB = bullets[i].GetComponent<Rigidbody2D>();
                    rigidB.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                //GameObject bulletR = objManager.MakeObj("bulletPlayrA");
                //bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                //GameObject bulletL = objManager.MakeObj("bulletPlayrA");
                //bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                //Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                //Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                //rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                //rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                for (int i = 0; i < 3; i++)
                {
                    bullets[i] = i == 1 ? objManager.MakeObj("bulletPlayerB") : objManager.MakeObj("bulletPlayrA");
                    if (i == 0)
                        bullets[i].transform.position = transform.position + Vector3.left * 0.2f;
                    else
                        bullets[i].transform.position = bullets[i - 1].transform.position + nextPos;
                    Rigidbody2D rigidB = bullets[i].GetComponent<Rigidbody2D>();
                    rigidB.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                //GameObject bulletRR = objManager.MakeObj("bulletPlayrA");
                //bulletRR.transform.position = transform.position + Vector3.right * 0.25f;

                //GameObject bulletLL = objManager.MakeObj("bulletPlayrA");
                //bulletLL.transform.position = transform.position + Vector3.left * 0.25f;

                //GameObject bulletCC = objManager.MakeObj("bulletPlayerB");
                //bulletCC.transform.position = transform.position;

                //Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                //Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                //Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                //rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                //rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                //rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }       

        curShotDelay = 0;
    }

    void Boom()
    {
        if (!isButtonB)
            return;
        //if (!Input.GetButton("Fire2"))
        //    return;
        if (isBoomTime)
            return;
        if (boom == 0)
            return;
        boom--;
        isBoomTime = true;
        manager.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 3f);

        GameObject[] enemiesL = objManager.GetPool("enemyL");
        GameObject[] enemiesM = objManager.GetPool("enemyM");
        GameObject[] enemiesS = objManager.GetPool("enemyS");

        for (int i = 0; i < enemiesL.Length; i++)
        {
            if (enemiesL[i].activeSelf)
            {
                Enemy enemyLogic = enemiesL[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int i = 0; i < enemiesM.Length; i++)
        {
            if (enemiesM[i].activeSelf)
            {
                Enemy enemyLogic = enemiesM[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int i = 0; i < enemiesS.Length; i++)
        {
            if (enemiesS[i].activeSelf)
            {
                Enemy enemyLogic = enemiesS[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        GameObject[] BulletsA = objManager.GetPool("bulletEnemyA");
        GameObject[] BulletsB = objManager.GetPool("bulletEnemyB");
        for (int i = 0; i < BulletsA.Length; i++)
        {
            if (BulletsA[i].activeSelf)
                BulletsA[i].SetActive(false);
        }
        for (int i = 0; i < BulletsB.Length; i++)
        {
            if (BulletsB[i].activeSelf)
                BulletsB[i].SetActive(false); 
        }
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    IsTouchTop = true;
                    break;
                case "Bottom":
                    IsTouchBottom = true;
                    break;
                case "Left":
                    IsTouchLeft = true;
                    break;
                case "Right":
                    IsTouchRight = true;
                    break;
            }
        }else if (collision.tag == "Enemy" || collision.tag == "EnemyBullet")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;
            life--;
            manager.UpdateLifeIcon(life);
            manager.CallExplosion(transform.position, "P");

            if(life == 0)
            {
                manager.GameOver();
            }else
            {
                manager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            
        }else if(collision.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollowers();
                    }
                    break;
                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        manager.UpdateBoomIcon(boom);
                    }
                    break;


            }
            collision.gameObject.SetActive(false);
        }
        
    }
    void AddFollowers()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }
    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    IsTouchTop = false;
                    break;
                case "Bottom":
                    IsTouchBottom = false;
                    break;
                case "Left":
                    IsTouchLeft = false;
                    break;
                case "Right":
                    IsTouchRight = false;
                    break;
            }
        }
    }
}
