using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected Transform target;
    public SpriteRenderer render;
    public Rigidbody2D rb;
    public System.Random rand;
    public Animator ani;
    public bool dead { get; protected set; }
    public BoxCollider2D bColl;

    public Vector2 boxsize;
    public Vector3 attackOffset;
    public bool isFlipped = false;

    public float monsterSpeed;
    protected float yDis = 0f;

    //hp�� damageText�� ���� ����
    public GameObject damageText;
    public Transform textPos;
    public Slider Health;
    public float HP = 100f;

    //�ʱ� ���� ����
    protected virtual void OnEnable()
    {
        isFlipped = true;
        HP = 100f;
        dead = false;
        rand = new System.Random();
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ani = GetComponent<Animator>();
        bColl = GetComponent<BoxCollider2D>();
        Health.value = HP;
        monsterSpeed = 2f;
        render.flipX = true;
    }


    //���� ���� ǥ��
    protected virtual void OnDrawGizmos()
    {
        Vector3 posO = transform.position;
        posO += transform.right * attackOffset.x;
        posO += transform.up * attackOffset.y;

        Gizmos.DrawWireCube(posO, boxsize);
    }

    // ������ �ִ� �Լ�
    public virtual void FindAnd()
    {
        Vector3 posO = transform.position;
        posO += transform.right * attackOffset.x;
        posO += transform.up * attackOffset.y;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posO, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log(collider.tag);
        }
    }

    //���� �ִϸ��̼� ���� ��ȯ
    public virtual void DirectionEnemy()
    {
        if (target.position.x > transform.position.x && isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (target.position.x < transform.position.x && !isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    //������ �Դ� �Լ�(= takeDamage) �ϴ� ����� ������
    public virtual void TakeDamage(int damage)
    {
        if (dead) return;
        textOut(damage);

        if (Health.value <= 0)
        {
            Die();
        }
    }

    // hp �ؽ�Ʈ �� �ֽ�ȭ ���� �Լ� �ۼ�
    protected void textOut(int damage)
    {
        HP -= damage;
        Health.value = HP;
        GameObject hitText = Instantiate(damageText);
        hitText.transform.position = textPos.position;
        hitText.GetComponent<DamageText>().damage = damage;
    }

    protected virtual void Die()
    {
        dead = true;
        ani.SetTrigger("die");
    }

    //���� ����
    public void monsterDestroy()
    {
        Destroy(gameObject);
    }

    //���� ȿ��
    protected IEnumerator ice_effects()
    {
        Color originalColor = render.color;

        render.color = new Color(0.23f, 0.23f, 1f);
        monsterSpeed /= 2;

        yield return new WaitForSeconds(3f);

        render.color = originalColor;
        monsterSpeed *= 2;
    }
}
