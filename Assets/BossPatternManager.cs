using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternManager : MonoBehaviour
{
    public Transform bossTransform;
    public float patternInterval = 3f;
    private float currentTime = 0f;
    private int patternIndex = 0;
    private Vector3[] attackPositions;
    private Animator bossAnimator;

    public GameObject warningEffectPrefab;
    private GameObject warningEffectInstance;
    public GameObject warningCircle;

    public GameObject projectilePrefab;    // 발사할 오브젝트 프리팹
    public float warningTime = 1.5f;        // 경고 시간 (레이저 발사까지의 대기 시간)
    public float projectileSpeed = 10f;     // 발사할 오브젝트의 속도

    private Transform playerTransform;      // 플레이어의 Transform


    void Start()
    {
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
            if (true)
            {
                Vector3 direction = (playerTransform.position - bossTransform.position).normalized;

                RaycastHit hit;
                if (Physics.Raycast(bossTransform.position, direction, out hit))
                {
                    // 경로 상의 중간 지점 계산
                    Vector3 middlePoint = bossTransform.position + (hit.point - bossTransform.position) * 0.5f;

                    // 경고 레이저 발사
                    StartCoroutine(ShootWarningLaser(middlePoint));
                }
            }
            else
            {
                StartCoroutine(ExecutePatternAll());
                StartCoroutine(ExecurePatternCircle());
            }
            currentTime= 0f;
        }
    }

    IEnumerator ExecutePatternAll()
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
    }

    IEnumerator ExecurePatternCircle()
    {
        bossAnimator.SetBool("nono",false);
        warningEffectInstance = Instantiate(warningCircle, bossTransform.position+ new Vector3(-0.2f, -0.6f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(warningEffectInstance);

        bossAnimator.SetTrigger("fall");

        yield return new WaitForSeconds(1f);
        bossAnimator.SetBool("nono", true);
    }

    private IEnumerator ShootWarningLaser(Vector3 targetPosition)
    {
        // 경고 레이저를 발사하기 전 대기 시간
        yield return new WaitForSeconds(warningTime);

        // 경고 레이저 발사
        GameObject laser = Instantiate(projectilePrefab, bossTransform.position, Quaternion.identity);
        Rigidbody laserRigidbody = laser.GetComponent<Rigidbody>();

        // 발사할 때 레이저의 방향과 힘 설정
        Vector3 direction = (targetPosition - bossTransform.position).normalized;
        laserRigidbody.velocity = direction * projectileSpeed;

        // 경고 레이저가 일정 시간 후에 사라지도록 설정
        Destroy(laser, 2f);
    }
}