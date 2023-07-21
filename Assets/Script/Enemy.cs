using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private SpriteRenderer render;
    public System.Random rand;

    public BoxCollider2D box;
    public float atkCooltime = 1f;
    public float atkDelay;
    public float moveSpeed = 3f;
    public Transform pos;
    public Vector2 boxsize;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        atkDelay = 0f;
        rand = new System.Random(); 
        box = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (atkDelay >= 0)
            atkDelay -= Time.deltaTime;


    }

    private void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    public void FindAnd()
    {
        if (render.flipX == false)
        {
            pos.localPosition = new Vector3(0.4421317f, 0f, 0f);
        }
        else
        {
            pos.localPosition = new Vector3(-0.4421317f, 0f, 0f);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log(collider.tag);
        }

    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target > baseobj)
        {
            render.flipX = false;
        }
        else
        { 
            render.flipX = true;
        }

    }


}
