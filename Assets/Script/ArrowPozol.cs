using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPozol : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnRate = 3f; // ��Ÿ�� (�� ����)
    private float currentTime = 0f; // ���� ��� �ð�]
    private Animator ani;
    public Transform target;
    public float speed = 1f;
    private SpriteRenderer render;
    private bool isAttacking = false;


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


        if (isAttacking)
        {
            // ���� ���� �� ��ġ �������� ����
            return;
        }

        if ( distance < 8f && distance > 5f )
        {
            ani.SetBool("isFollow", true);
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
                StartCoroutine(AttackRoutine());
                currentTime = 0f; // ��� �ð� �ʱ�ȭ
            }

        }
        
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        ani.SetTrigger("attack1");
        yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ��� �ð�
        SpawnArrow();
        yield return new WaitForSeconds(0.5f); // �߰� ��� �ð� (���� ����)
        isAttacking = false;
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
        Instantiate(arrowPrefab, transform.position + new Vector3(0f, -0.2f, 0f), Quaternion.identity);
    }
}