using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPozol : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnRate = 3f; // ��Ÿ�� (�� ����)
    private float currentTime = 0f; // ���� ��� �ð�]
    private Animator ani;
    private Transform target;
    public float speed = 1f;
    private SpriteRenderer render;
    private bool isAttacking = false;
    private float yDis = 0f;
    public System.Random rand;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider Health;
    public float HP = 100f;
    private bool takeAttack = false;
    public float SetTime;
    public bool dead { get; protected set; }

    void Start()
    { // �÷��̾� ��ġ y���� �������� ����, ȭ�� ��� ��Ÿ�� random ������ ����( �� �� �����ؼ� �� ���� �� ���� �Ź� �ٲ� ���� ���� )
        // ���� ���� ȭ�� �߻� x 
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        rand = new System.Random();
        Health.value = HP;
    }

    void Update()
    {
        if (dead) return;

        yDis = target.position.y - transform.position.y;
        DirectionEnemy(target.transform.position.x, transform.position.x);
        // ��� �ð� ������Ʈ
        currentTime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if (!isAttacking && distance < 14f && distance > 8f)
        {
            ani.SetBool("isFollow", true);
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if( distance >= 14f)
        {
            ani.SetBool("isFollow", false);
        }
        else if( distance <= 8f)
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
        yield return new WaitForSeconds(1f); // �߰� ��� �ð� (���� ����)
        isAttacking = false;
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;

        if (takeAttack)
            return;

        takeAttack = true;
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);
        ani.SetTrigger("Hit");

        if (HP <= 0)
        {
            dead = true;
            ani.SetTrigger("die");
            Invoke("SetFalse", SetTime);
        }
        takeAttack = false;
    }

    private void SetFalse()
    {
        Destroy(gameObject);
    }

    void SpawnArrow()
    {
        Instantiate(arrowPrefab, transform.position + new Vector3(0f, -0.564207f, 0f), Quaternion.identity);
    }
}