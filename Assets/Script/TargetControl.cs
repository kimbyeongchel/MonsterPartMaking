using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine; 

public class TargetControl : MonoBehaviour
{
    float moveX, moveY;
    public float moveSpeed = 5f;
    private Animator animator;
    private SpriteRenderer render;
    private Rigidbody2D rigid;
    public float coolTime = 0.5f;
    private float curTime;
    public Transform pos;
    public Vector2 boxsize;
    private ArrowPozol arr;
    


    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y);

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
                foreach(Collider2D collider  in collider2Ds)
                {
                    UnityEngine.Debug.Log(collider.tag);
                }
                animator.SetTrigger("Attack");
                curTime = coolTime;
            }
        }
        else
            curTime -= Time.deltaTime;


        Jump();
    }
    public void Direction()
    {
        if (Input.GetAxis("Horizontal") > 0)
        { 
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    void Jump()
    {

        if (Input.GetKey(KeyCode.X))
        {
            rigid.AddForce(new Vector2(0, 5f));

        }


    }
}
