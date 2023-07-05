using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPozol : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnRate = 1f; // 쿨타임 (초 단위)
    private float currentTime = 0f; // 현재 경과 시간]
    public Transform pos;
    private Animator ani;
    public Transform target;
    public float speed = 1f;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 경과 시간 업데이트
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
            // 쿨타임이 지났을 때 화살 생성
            if (currentTime >= spawnRate)
            {
                ani.SetTrigger("attack1");
                SpawnArrow();
                currentTime = 0f; // 경과 시간 초기화
            }

        }
        
    }

    void SpawnArrow()
    {
        Instantiate(arrowPrefab, pos.position, Quaternion.Euler(0, 0, 90f));
    }
}