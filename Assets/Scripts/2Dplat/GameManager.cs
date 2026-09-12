using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public int StageScore;
    public int TotalScore;
    public int StageIndex;
    public int BetScore = 0;

    public GameObject[] Stage;

    public bool IsGameOver = false;

    public int DownDamage = 1; //³«ÇÏ µ¥¹ÌÁö
    public Vector3 RepositionVec;

    private void Awake()
    {
        RepositionVec = new Vector3(-5, 2.5f, 0);
    }

    public void NextStage(GameObject target)
    {
        Move IsPlayer = target.GetComponent<Move>();

        if (StageIndex < Stage.Length - 1)
        {
            Stage[StageIndex++].SetActive(false);
            Stage[StageIndex].SetActive(true);

            UIManager.Instance.UIStage();

            target.SetActive(false);
            target.SetActive(true);
            Reposition(IsPlayer);
        }
        else
        {
            Time.timeScale = 0;

            UIManager.Instance.RestartBtn.SetActive(true);
            UIManager.Instance.text.text = "Clear! Restart?";
        }


        TotalScore += StageScore;
        StageScore = 0;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Move IsPlayer = collision.GetComponent<Move>();
        if(IsPlayer != null)
        {
            IsPlayer.OnDamaged(DownDamage);
            Reposition(IsPlayer);
        }
    }
    
    void Reposition(Move IsPlayer)
    {
        IsPlayer.OnVelocity(RepositionVec);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }


}
