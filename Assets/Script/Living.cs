using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Living : MonoBehaviour
{ // 해당 클래스는 상위 클래스로써 하위 클래스로 몬스터와 플레이어에게 변수 및 함수를 물려준다.
    //그 중 필수적인 함수만을 구성해야 된다.
    //HP, Rigidbody, Animator, SpriteRenderer 등을 변수로 갖고
    //함수로는 OnEnable을 통한 초기 세팅 및 필요한 컴포넌트 불러오기
    //그리고 die, onDrawGizmo 기타 등등
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    protected Animator animator; // protected로 변경
    protected Transform pos; // protected로 변경
    protected Vector2 boxsize; // protected로 변경
    protected SpriteRenderer render; // protected로 변경

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Invoke("TriggerHurt", 0.8f);

        if (health <= 0 && !dead)
        {
            Invoke("Die", 1f);
        }
    }

    public virtual void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    public virtual void FindAnd()
    {
        if (render.flipX == false)
        {
            pos.localPosition = new Vector3(0.357f, pos.localPosition.y, 0f);
        }
        else
        {
            pos.localPosition = new Vector3(-0.357f, pos.localPosition.y, 0f);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log(collider.tag);
        }
    }

    private void TriggerHurt()
    {
        animator.SetTrigger("Hit");
    }

    public virtual void Die()
    {
        dead = true;
        animator.SetTrigger("Die");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}