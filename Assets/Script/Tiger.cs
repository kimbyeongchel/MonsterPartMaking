using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // ���� �� ����
    private bool isIdleAfterAttack = false; // ���� �� idle
    private SpriteRenderer render;
    public float speed = 1f;
    public Transform pos;
    //public BoxCollider2D box;
    // public Vector2 boxsize;
    public float idleTime = 1f; // ���� �� idle �ð�

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        // box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if (!isAttacking && distance > 5f)
        {
            if (!isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetBool("isFollow", true);
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        else if (distance <= 5f && distance >4f)
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack)
            {
                ani.SetBool("isFollow", false);
                Debug.Log("�߰Ÿ� �������!!!!");
                DirectionEnemy(target.transform.position.x, transform.position.x);
                StartCoroutine(AttackRoutine11());
            }
        }
        else
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                StartCoroutine(AttackRoutine());
            }
        }

        IEnumerator AttackRoutine()
        {
            isAttacking = true;
            ani.SetTrigger("attack1");
            yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ��� �ð�
            isAttacking = false;
            isIdleAfterAttack = true;
            yield return new WaitForSeconds(idleTime); // ������ �κ�: ���� �ð� ���� idle ���·� ���
            isIdleAfterAttack = false;
        }

        IEnumerator AttackRoutine11()
        {
            isAttacking = true;
            ani.SetTrigger("die");
            yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ��� �ð�
            isAttacking = false;
            isIdleAfterAttack = true;
            yield return new WaitForSeconds(idleTime); // ������ �κ�: ���� �ð� ���� idle ���·� ���
            isIdleAfterAttack = false;
        }
        //private void OnDrawGizmos() // ������ �� �� �ڵ� �����.
        //{
        //   Gizmos.color = Color.blue;
        //    Gizmos.DrawWireCube(pos.position, boxsize);
        //}

        void DirectionEnemy(float target, float baseobj)
        {
            if (target < baseobj)
                render.flipX = true;
            else
                render.flipX = false;
        }

       // void FindAnd()
       // {
        //    if (render.flipX == false)
        //    {
        //        pos.localPosition = new Vector3(0.357f, pos.localPosition.y, 0f);
        //    }
        //    else
         //   {
         //       pos.localPosition = new Vector3(-0.357f, pos.localPosition.y, 0f);
         //   }
//
        //    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
         //   foreach (Collider2D collider in collider2Ds)
         //   {
         //       if (collider.tag == "Player")
          //          UnityEngine.Debug.Log(collider.tag);
          //  }

        //}
    }
}
