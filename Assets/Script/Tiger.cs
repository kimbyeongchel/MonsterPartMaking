using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // 공격 중 상태
    private bool isIdleAfterAttack = false; // 공격 후 idle
    private SpriteRenderer render;
    public float speed = 1f;
    public Transform pos;
    //public BoxCollider2D box;
    // public Vector2 boxsize;
    public float idleTime = 1f; // 공격 후 idle 시간

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        // box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance >5f)
        {
            ani.SetBool("isFollow", true);
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (distance <= 5f && distance >3f)
        {
            Debug.Log("중거리 도끼찍기!!!!");
            ani.SetBool("isFollow", false);
        }
        else
        {
            //if (enemy.rand.NextDouble() > 0.6)
            //{
            //  animator.SetTrigger("attack1");
            //}
            // else
            //    animator.SetTrigger("attack2");
            ani.SetTrigger("die");
        }

        //private void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
        //{
        //   Gizmos.color = Color.blue;
        //    Gizmos.DrawWireCube(pos.position, boxsize);
        //}

        void DirectionEnemy(float target, float baseobj)
        {
            if (target < baseobj)
                render.flipX = true;
            else
                render.flipX = false;
        }

       // void FindAnd()
       // {
        //    if (render.flipX == false)
        //    {
        //        pos.localPosition = new Vector3(0.357f, pos.localPosition.y, 0f);
        //    }
        //    else
         //   {
         //       pos.localPosition = new Vector3(-0.357f, pos.localPosition.y, 0f);
         //   }
//
        //    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
         //   foreach (Collider2D collider in collider2Ds)
         //   {
         //       if (collider.tag == "Player")
          //          UnityEngine.Debug.Log(collider.tag);
          //  }

        //}
    }
}
