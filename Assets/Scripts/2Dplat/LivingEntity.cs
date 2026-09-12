using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour
{
    public int Hp { get; protected set; }
    public int Damage { get; protected set; }
    public bool Dead { get; protected set; }


    public event Action OnDeath;

    protected virtual void OnEnable()
    {//부활 혹은 리스폰 하는 거 추가할 거고
        Dead = false;
        Damage = 1;
        Hp = 3;
    }

    public virtual void OnDamaged(int damage)
    {
        Hp -= damage;


        if (Hp <= 0 && !Dead)
        {
            Die();
        }
        
    }

    public virtual void Die()
    {
        if(OnDeath != null)
        {
            OnDeath();
        }

        Dead = true;
    }

}
