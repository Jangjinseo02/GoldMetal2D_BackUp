using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Horizontal";
    public string jumpAxisName = "Jump";
    
    public bool IsDamaged = false;

    public float move { get; private set; }
    public bool jump { get; private set; }

    // Update is called once per frame
    void Update()
    {  //gameManager가 있고 게임 오버인 경우에는 이 move를 입력받지 않게 만드는 부분이 필요.
        
        if(IsDamaged == true)
        {
            move = 0;
            jump = false;
            return;
        }

        move = Input.GetAxisRaw(moveAxisName);
        jump = Input.GetButtonDown(jumpAxisName);
    }
}
