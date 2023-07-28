using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pozol : MonoBehaviour // �ϴ� �ش� ������ hp ���ҿ� ������ ��� �׽�Ʈ
{ 

    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // ���� �� ����
    private bool isIdleAfterAttack = false; // ���� �� idle
    private SpriteRenderer render;
    public float speed = 1f;
    public Transform pos;
    public Vector2 boxsize;
    public float idleTime = 1f; // ���� �� idle �ð�
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider monsterHealth;
    public Transform HPPos;
    public float HP = 100f;
    private Slider Health;
    public float SetTime;

    public bool dead { get; protected set; }

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        Health = Instantiate(monsterHealth);
        Health.value = HP;
    }

    void Update()
    {
        if(dead) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (!isAttacking && distance < 8f && distance > 2f)
        {
            
            if (!isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetBool("isFollow", true);
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        else if (distance >= 8f)
        {
            ani.SetBool("isFollow", false);
        }
        else if (distance <= 2f)
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack) // ���� ���̰ų� �̹� idle ���̶�� �������� ����
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetTrigger("attack");
                StartCoroutine(ResumeAttack());
            }
        }
    }

    IEnumerator ResumeAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f); // ���� �ִϸ��̼� ��� �ð� (1��)
        isAttacking = false;
        isIdleAfterAttack = true;
        yield return new WaitForSeconds(idleTime); // ������ �κ�: ���� �ð� ���� idle ���·� ���
        isIdleAfterAttack = false;
    }


    private void OnDrawGizmos() // ������ �� �� �ڵ� �����.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    void FindAnd()
    {
        if (render.flipX == false)
        {
            pos.localPosition = new Vector3(0.357f, pos.localPosition.y, 0f);
        }
        else
        {
            pos.localPosition = new Vector3(-0.357f, pos.localPosition.y, 0f);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log(collider.tag);
        }
    }

    ////public void TakeDamage(int damage)
    ////{
    ////    GameObject hudText = Instantiate(hudDamageText);
    ////    hudText.transform.position = hudPos.position;
    ////    hudText.GetComponent<DamageText>().damage = damage;
    //    HP -= damage;
    //    Health.value = HP;
    //    Debug.Log(damage);

    //    if(HP == 0)
    //    {
    //        dead = true;
    //        ani.SetTrigger("die");
    //        Invoke("SetFalse", SetTime);
    //    }
    //}

    private void SetFalse()
    {
        Destroy(gameObject);
    }
}
