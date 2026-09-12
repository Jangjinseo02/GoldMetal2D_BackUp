using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public Image[] HpUI;
    public Text StageText;
    public Text ScoreText;
    public GameObject RestartBtn;
    public Text text; 

    private void Awake()
    {
        text = RestartBtn.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        ScoreText.text = (GameManager.Instance.StageScore + GameManager.Instance.TotalScore).ToString();
    }

    public void UIStage()
    {
        StageText.text = "Stage " + (GameManager.Instance.StageIndex + 1);
    }

    public void UIHP(int Health)
    {
        if (Health > 0)
        {
            HpUI[Health].color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            RestartBtn.SetActive(true);
        }
    }

    public void HealUI(int Health)
    {
        for(int i = 0; i < Health; i++)
        {
            HpUI[i].color = new Color(1, 1, 1, 1);
        }
    }
}
