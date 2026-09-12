using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Move : LivingEntity
{
    Rigidbody2D enermy_rigid;
    Animator anim;
    SpriteRenderer sprite;
    CapsuleCollider2D collider2;
    public int nextMove;
    public float nextThink = 3f;
    public bool Isrun = false;

    // Start is called before the first frame update
    void Awake()
    {
        enermy_rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collider2 = GetComponent<CapsuleCollider2D>();
        StartCoroutine(Thinking());
    }

    void FixedUpdate()
    {
        
        enermy_rigid.velocity = new Vector2(nextMove, enermy_rigid.velocity.y);

        Vector2 frontVec = new Vector2(enermy_rigid.position.x + nextMove, enermy_rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(1, 1, 1));
        
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        if (rayhit.collider == null)
        {
            nextMove *= -1; //반대 방향 회전
        }

        Turn();

    }

    IEnumerator Thinking()
    {
        while (enermy_rigid != null)
        {
            yield return new WaitForSeconds(nextThink);

            Think();
        }       

    }

    void Think()
    {
        nextThink = Random.Range(0, 5f);
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("IsRun", nextMove);
        
    }

    void Turn()
    {
        if(nextMove != 0)
        {
            Isrun = nextMove == 1 ? true : false;
            sprite.flipX = Isrun;
        }
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

    }

    public override void Die()
    {
        OnDeath += OnDie;
        base.Die();
    }

    public void OnDie()
    {
        sprite.color = new Color(1, 1, 1, 0.4f);
        sprite.flipY = true;
        collider2.enabled = false;
        enermy_rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("Destroy", 2f);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

}
