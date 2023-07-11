using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPozol : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnRate = 1f; // ��Ÿ�� (�� ����)
    private float currentTime = 0f; // ���� ��� �ð�]
    private Animator ani;
    public Transform target;
    public float speed = 1f;
    private SpriteRenderer render;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        DirectionEnemy(target.transform.position.x, transform.position.x);
        // ��� �ð� ������Ʈ
        currentTime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if( distance < 8f && distance > 5f )
        {
            ani.SetBool("isFollow", true);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else if( distance >= 8f)
        {
            ani.SetBool("isFollow", false);
        }
        else if( distance <= 5f)
        {
            ani.SetBool("isFollow", false);
            // ��Ÿ���� ������ �� ȭ�� ����
            if (currentTime >= spawnRate)
            {
                ani.SetTrigger("attack1");
                SpawnArrow();
                currentTime = 0f; // ��� �ð� �ʱ�ȭ
            }

        }
        
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    void SpawnArrow()
    {
        Instantiate(arrowPrefab, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
    }
}