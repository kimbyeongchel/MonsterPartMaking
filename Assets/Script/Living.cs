using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Living : MonoBehaviour
{
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