using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public int Score;

    public void Use(GameObject target)
    {
        GameManager.Instance.StageScore += Score;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
