using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManger : MonoBehaviour
{
    public int stage;
    public Animator stageAnim;
    public Animator ClearAnim;

    public Animator fade;
    public Transform playerPos;


    public string[] enemyObjects;
    public Transform[] spawnPoints;
    public GameObject Player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public float maxspawnDelay;
    public float curspawnDelay;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjects = new string[] { "enemyL", "enemyM", "enemyS", "Boss" };
        StageStart();
    }
    public void StageEnd()
    {
        ClearAnim.SetTrigger("On");
        ClearAnim.GetComponent<Text>().text = "STAGE " + stage + "\n" + "CLEAR";
     
        fade.SetTrigger("Out");
        Player.transform.position = playerPos.position;
        stage++;
        if(stage > 2)
        {
            GameOver();
        }
        else
        {
            Invoke("StageStart", 5f);
        }

    }
    public void StageStart()
    {
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "STAGE " + stage + "\n" + "START";
        ReadSpawnFile();
        fade.SetTrigger("In");
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("stage " + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            spawnList.Add(spawnData);
        }

        stringReader.Close();

        maxspawnDelay = spawnList[0].delay;
        
    }

    private void Update()
    {
        curspawnDelay += Time.deltaTime;

        if(curspawnDelay > maxspawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curspawnDelay = 0;
        }

        Player playerLogic = Player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

    }
    public void RespawnPlayer()
    {
        Invoke(nameof(RespawnPlayerExe), 2.0f);
    }

    public void RespawnPlayerExe()
    {
        Player.transform.position = new Vector3(0, -3.5f, 0);
        Player.SetActive(true);

        Player playerLogic = Player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void RtryBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateLifeIcon(int life)
    {
        for(int i = 0; i < 3; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < life; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int i = 0; i < 3; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < boom; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "B":
                enemyIndex = 3;
                break;
            case "S":
                enemyIndex = 2;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 0;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjects[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        enemyLogic.player = Player;
        enemyLogic.manager = this;
        enemyLogic.objManager = objectManager;

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);

        }
        else
        {
            rigid.velocity = Vector2.down * enemyLogic.speed;
        }

        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        curspawnDelay = spawnList[spawnIndex].delay;
    }

   
}
