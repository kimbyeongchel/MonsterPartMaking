using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private SpriteRenderer render;

    public BoxCollider2D box;
    public float atkCooltime = 1f;
    public float atkDelay;
    public float moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        atkDelay = 0f;
        box = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (atkDelay >= 0)
            atkDelay -= Time.deltaTime;
    }

    public void BoxOn()
    {
        box.enabled = true;
    }

    public void BoxOff()
    {
        box.enabled = false;
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target > baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }
}
