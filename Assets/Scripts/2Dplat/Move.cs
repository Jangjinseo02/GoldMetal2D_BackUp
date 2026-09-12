using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : LivingEntity
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    PlayerInput playerInput;
    CapsuleCollider2D collider2;

    public Vector2 target;
    public float maxSpeed;
    public float JumpPower;
    public int JumpCount = 0;
    public int OBJ_DAMAGE = 1;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        collider2 = GetComponent<CapsuleCollider2D>();
       
    }

    private void Update()
    {
        if(GameManager.Instance.IsGameOver != true)
        {
            PlayerMove();

            Jumping();

        }
        
       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Damage = 2;
        UIManager.Instance.HealUI(Hp);
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
        UIManager.Instance.UIHP(Hp);

        KnockBack();

        anim.SetTrigger("DoDamaged");
        AudioManager.Instance.PlayAudio("DAMAGED");

        Invoke("OnInput", 0.5f); // 플레이어 입력 허용
        Invoke("OffDamaged", 3.0f); // 무적 풀기
    }

    public override void Die()
    {
        OnDeath += OnDie;

        base.Die();
    }

    //플레이어 점프 메서드
    void Jumping()
    {
        if (playerInput.jump && JumpCount < 2)
        {
            JumpCount++;
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(new Vector2(rigid.velocity.x, JumpPower), ForceMode2D.Impulse);
            anim.SetTrigger("IsJump");

            AudioManager.Instance.PlayAudio("JUMP");
        }
        else if (playerInput.jump && rigid.velocity.y > 0)
        {
            rigid.velocity = rigid.velocity * 0.5f;
        }
    }

    //플레이어 움직임 메서드
    void PlayerMove()
    {
        if (playerInput.move < 0)
            sprite.flipX = true;
        else if (playerInput.move > 0)
            sprite.flipX = false;

        if (playerInput.move != 0)
            anim.SetBool("IsRun", true);
        else
            anim.SetBool("IsRun", false);
       
        if(playerInput.IsDamaged != true)
            rigid.velocity = new Vector2(maxSpeed * playerInput.move, rigid.velocity.y); //velocity의 x값을 maxspeed * (1, 0 , -1)로 잡아라
    }

    //넉백 메서드
    void KnockBack()
    {
        rigid.velocity = Vector2.zero; //넉백을 위한 속도 없애기
        playerInput.IsDamaged = true; // 넉백 동안의 플레이어 입력 막기
        gameObject.layer = 9; // 피격 시 무적 레이어
        sprite.color = new Color(1, 1, 1, 0.4f); // 무적을 알려주는 투명 컬러

        int dirc = transform.position.x - target.x > 0 ? 1 : -1; // 히트 위치 찾기
        rigid.AddForce(new Vector2(dirc, 1f) * 4, ForceMode2D.Impulse); // 히트 위치 방향으로 넉백
    }

    //플레이어 무적 해제 메서드
    void OffDamaged()
    {
        gameObject.layer = 8;
        sprite.color = new Color(1, 1, 1, 1);
    }

    //플레이어 입력 받기 메서드
    void OnInput()
    {
        playerInput.IsDamaged = false;
    }

    //플레이어 공격 메서드
    void Attack(LivingEntity enemey)
    {
        AudioManager.Instance.PlayAudio("ATTACK");
      
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        enemey.OnDamaged(Damage);

    }

    //플레이어 죽음 메서드
    public void OnDie()
    {
        AudioManager.Instance.PlayAudio("DIE");


        sprite.color = new Color(1, 1, 1, 0.4f);
        sprite.flipY = true;
        collider2.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        GameManager.Instance.IsGameOver = true; //플레이어 사망 게임 오버
    }

    //플레이어 리포지션 메서드
    public void OnVelocity(Vector3 vec)
    {//리포지션 때문에 쓴 부분
        gameObject.transform.position = vec;
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            AudioManager.Instance.PlayAudio("ITEM");
            collision.GetComponent<IItem>().Use(gameObject);
        }

        if (collision.tag == "Finish")
        {
            AudioManager.Instance.PlayAudio("FINISH");

            GameManager.Instance.NextStage(gameObject);
            //Next Stage
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LivingEntity IsEnemy = collision.gameObject.GetComponent<LivingEntity>();
        //target = collision.transform.position;

        if (collision.gameObject.layer == 6) // Ground layer번호 == 6
        {
            JumpCount = 0; // 땅에 닿은 경우 점프 카운트 0으로 리셋
        }

        if(IsEnemy != null)
        {
            if (transform.position.y > collision.transform.position.y)
            {
                Attack(IsEnemy);

                return;
            }

            OnDamaged(IsEnemy.Damage);

        }
        else if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(OBJ_DAMAGE);
        }
    }
}
