using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public string horizonAxisName = "Horizontal";
    public string vertiAxisName = "Vertical";
    public string fireAxisName = "Fire1";
    public string skilAxisName = "Fire2";

    public float horizon { get; private set; }
    public float verti { get; private set; }
    public bool fire { get; private set; }
    public bool skil { get; private set; }

    void Update()
    {
        horizon = Input.GetAxisRaw(horizonAxisName);
        verti = Input.GetAxisRaw(vertiAxisName);
        fire = Input.GetButton(fireAxisName);
        skil = Input.GetButton(skilAxisName);
    }


}
