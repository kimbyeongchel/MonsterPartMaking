using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float throwForce = 6f;      // ���� ��
    public float explosionRadius = 1f; // ���� ����

    private Animator ani;
    public float size;
    public GameObject pos;
    private Rigidbody2D rb;
    private bool isExploded = false;
    private Transform playerTransform; // �÷��̾��� Transform

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾ ã�Ƽ� Transform ��������
        ThrowBomb();
        Invoke("destroyBomb", 3f);

    }

    void ThrowBomb()
    {
        // �÷��̾�� ��ź ���� ���� ���� ���
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // �ʱ� �ӵ� ��� (��ź�� ���� �������� Y ���� �ӵ��� ������� �մϴ�)
        Vector2 velocity = direction * throwForce;
        velocity.y = Mathf.Abs(velocity.y); // Y ���� �ӵ��� ����� ����

        // Rigidbody2D�� �� ����
        rb.velocity = velocity;
    }

    void Explode()
    {
        if (isExploded) return;

        isExploded = true;

        // ���� ȿ���� ���⿡ �߰��ϼ���.
        // ���� ���, ���� ���带 ����ϰų� ���� ����Ʈ�� ������ �� �ֽ��ϴ�.

        // �ֺ��� �ִ� ��� Collider2D ��������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.transform.position, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            // ���⿡�� �� Collider�� ���� �۾��� �����ϼ���.
            if (col.CompareTag("Player"))
            {
                // �÷��̾�� ������ �ֱ� �Ǵ� �ٸ� ���� ����
                Debug.Log("�÷��̾�� ������ �ֱ�");
            }
            else
            {
                Debug.Log("������Ʈ �浹");
            }
        }
    }

    void destroyBomb()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos() // ������ �� �� �ڵ� �����.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos.transform.position, size);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded)
        {
            if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Player"))
            {
                ani.SetTrigger("bomb");
                transform.localScale = new Vector3(2f, 2f, 0f);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Explode();
            }
        }
    }
}
