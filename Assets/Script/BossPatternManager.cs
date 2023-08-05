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

    public GameObject projectilePrefab;    // 발사할 오브젝트 프리팹
    public float projectileSpeed = 10f;     // 발사할 오브젝트의 속도
    private Transform playerTransform;      // 플레이어의 Transform


    void Start()
    {
        rand = new System.Random();
        bossAnimator = GetComponent<Animator>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0f, -1.80125f, 0f); // 가운데 위치
        attackPositions[1] = new Vector3(-5f, -1.80125f, 0f); // 왼쪽 위치
        attackPositions[2] = new Vector3(7.25f, -1.80125f, 0f); // 오른쪽 위치
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

    IEnumerator ExecutePatternAll() // 전범위 공격
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

    IEnumerator ExecurePatternCircle() // 원 공격
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

    IEnumerator ShootWarningLand() // 투사체 발사
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