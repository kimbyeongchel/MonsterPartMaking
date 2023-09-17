using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float throwForce = 3f;      // 던질 힘
    public float throwAngle = 45f;      // 던질 각도
    private Animator ani;
    public Transform pos;
    public float size;

    public Transform target;        // 목표 지점

    private Rigidbody2D rb;

    void Start()
    {
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //ani = GetComponent<Animator>();
        //float radianAngle = throwAngle * Mathf.Deg2Rad;
        //Vector2 velocity = new Vector2(Mathf.Cos(radianAngle) * throwForce, Mathf.Sin(radianAngle) * throwForce);
        //rb.velocity = velocity;

        rb = GetComponent<Rigidbody2D>();

        // 초기 속도 계산
        float radianAngle = throwAngle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(radianAngle) * throwForce, Mathf.Sin(radianAngle) * throwForce);

        // 목표 지점까지의 거리 계산
        Vector2 displacement = target.position - transform.position;

        // 중력 적용
        float gravity = Physics2D.gravity.magnitude;
        float time = 2 * velocity.y / gravity;
        velocity.x = displacement.x / time;

        // Rigidbody2D에 힘 적용
        rb.velocity = velocity;
    }

    public void DestroyBomb()
    {
        Destroy(gameObject);
    }

    void Bomb()
    {
        ani.SetTrigger("bomb");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ground" || collision.tag == "Player")
            Bomb();

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(pos.position + new Vector3(0f, -0.3f, 0f), size, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log("boom" + collider.tag);
        }
    }
    // 폭탄 수정하기 시발

    private void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos.position + new Vector3(0f, -0.3f, 0f), size);
    }
}
