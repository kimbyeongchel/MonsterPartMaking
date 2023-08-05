using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPatternManager : MonoBehaviour
{
    public Transform bossTransform;
    public float patternInterval = 2f;
    private float currentTime = 0f;
    private int patternIndex = 0;
    private Vector3[] attackPositions;
    private Animator bossAnimator;
    public System.Random rand;
    public Slider bossHP;

    public GameObject warningEffectPrefab;
    private GameObject warningEffectInstance;
    public GameObject warningCircle;

    public GameObject projectilePrefab;    // �߻��� ������Ʈ ������
    public float projectileSpeed = 10f;     // �߻��� ������Ʈ�� �ӵ�
    private Transform playerTransform;      // �÷��̾��� Transform


    void Start()
    {
        rand = new System.Random();
        bossAnimator = GetComponent<Animator>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0f, -1.80125f, 0f); // ��� ��ġ
        attackPositions[1] = new Vector3(-5f, -1.80125f, 0f); // ���� ��ġ
        attackPositions[2] = new Vector3(7.25f, -1.80125f, 0f); // ������ ��ġ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= patternInterval)
        {
            double value = rand.NextDouble();
            if (value > 0.7)
            {
                StartCoroutine(ShootWarningLand());
            }
            else if (0.7 >= value && value > 0.4)
            {
                StartCoroutine(ExecutePatternAll());
            }
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
            bossTransform.position = attackPositions[patternIndex];
            bossAnimator.SetTrigger("a" + (patternIndex + 1));

            yield return new WaitForSeconds(1f);
            currentTime= 0f;
        }
        bossTransform.position = attackPositions[2];
    }

    IEnumerator ExecurePatternCircle() // �� ����
    {
        bossAnimator.SetBool("nono",false);
        warningEffectInstance = Instantiate(warningCircle, bossTransform.position+ new Vector3(-0.2f, -0.6f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(warningEffectInstance);

        bossAnimator.SetTrigger("fall");
        currentTime= 0f;
        yield return new WaitForSeconds(1f);
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
}