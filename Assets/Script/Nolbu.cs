using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nolbu : MonoBehaviour
{
    public Transform bossTransform;
    public float patternInterval = 2f;
    private float currentTime = 0f;
    private int patternIndex = 0;
    private Vector3[] attackPositions;
    private Animator bossAnimator;
    public System.Random rand;
    public float SetTime;
    private bool takeAttack = false;
    public bool dead { get; protected set; }
    private BoxCollider2D coll;
    private int count = 0;
    private Vector3 direction;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider Health;
    public Transform HPPos;
    public float HP = 7f;
    public float speed = 2f;

    public GameObject money;
    public GameObject warningEffectPrefab;
    private GameObject warningEffectInstance;
    public GameObject warningCircle;

    public GameObject projectilePrefab;    // �߻��� ������Ʈ ������
    public float projectileSpeed = 10f;     // �߻��� ������Ʈ�� �ӵ�
    private Transform playerTransform;      // �÷��̾��� Transform


    void Start()
    {
        Health.value = HP;
        rand = new System.Random();
        bossAnimator = GetComponent<Animator>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0f, -1.80125f, 0f); // ��� ��ġ
        attackPositions[1] = new Vector3(-5f, -1.80125f, 0f); // ���� ��ġ
        attackPositions[2] = new Vector3(7.25f, -1.80125f, 0f); // ������ ��ġ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (dead) return;

        currentTime += Time.deltaTime;
        if (currentTime >= patternInterval)
        {
            double value = rand.NextDouble();
            if (value > 0.2)
            {
                //StartCoroutine(ShootWarningLand());
                StartCoroutine(ExecutePatternAll());
            }
           // else if (0.7 >= value && value > 0.4)
           // {
            //    StartCoroutine(ExecutePatternAll());
           // }
            else
            {
                StartCoroutine(ExecurePatternCircle());
            }
            currentTime= 0f;
        }
    }

    IEnumerator ExecutePatternAll() // ������ ����
    {
        for (int i = 0; i < 3; i++)
        {
            patternIndex = Random.Range(0, 3);
            warningEffectInstance = Instantiate(warningEffectPrefab, attackPositions[patternIndex], Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Destroy(warningEffectInstance);
            bossAnimator.SetTrigger("arrowUP");

            yield return new WaitForSeconds(0.5f);
            currentTime= 0f;
        }
    }

    IEnumerator ExecurePatternCircle() // �� ����
    {
        bossAnimator.SetBool("nono", false);
        warningEffectInstance = Instantiate(warningCircle, bossTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(warningEffectInstance);

        bossAnimator.SetTrigger("noise");
        currentTime= 0f;
        yield return new WaitForSeconds(0.5f);
        bossAnimator.SetBool("nono", true);
    }

    IEnumerator ShootWarningLand() // ����ü �߻�
    {
        for (int i = 0; i < 3; i++)
        {
            bossAnimator.SetTrigger("a1");
            GameObject land = Instantiate(projectilePrefab, bossTransform.position, Quaternion.identity);
            Rigidbody2D laserRigidbody = land.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = (playerTransform.position - bossTransform.position).normalized;

            laserRigidbody.velocity = targetPosition * projectileSpeed;

            Destroy(land, 3f);
            yield return new WaitForSeconds(0.5f);
            currentTime = 0f;
        }
    }

    IEnumerator TakeDamageRoutine(int damage)
    {
        count++;
        takeAttack = true;

        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);

        if (count % 2 == 0)
        {
            bossAnimator.SetBool("hit", true);
            coll.enabled = false;
            yield return new WaitForSeconds(0.8f);
            bossAnimator.SetBool("hit", false);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, 0f);
            SetActiveMoney();
            yield return new WaitForSeconds(3f);
            coll.enabled = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - 1.2f, 0f);
            adActiveMoney();
        }
        else
        {
            bossAnimator.SetBool("isFollow", true);
            patternIndex = Random.Range(0, 3);
            direction = new Vector3(attackPositions[patternIndex].x - transform.position.x, 0f, 0f).normalized;
            // �ȱ� ���� �� ��ǥ�� ������ ��ҷ� run
            transform.Translate(direction * speed * Time.deltaTime);
            yield return new WaitForSeconds(2f);
            bossAnimator.SetBool("isFollow", false);
        }

        if (HP <= 0)
        {
            dead = true;
           // ani.SetTrigger("die");
            Invoke("SetFalse", SetTime);
        }
        takeAttack = false;
    }


    public void TakeDamage(int damage)
    {
        if (dead) return;

        if (takeAttack)
            return;

        StartCoroutine(TakeDamageRoutine(damage));
    }

    private void SetFalse()
    {
        Destroy(gameObject);
    }

    public void SetActiveMoney()
    {
        money.SetActive(true);
    }

    public void adActiveMoney()
    {
        money.SetActive(false);
    }
}