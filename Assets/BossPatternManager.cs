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

    public GameObject projectilePrefab;    // �߻��� ������Ʈ ������
    public float warningTime = 1.5f;        // ��� �ð� (������ �߻������ ��� �ð�)
    public float projectileSpeed = 10f;     // �߻��� ������Ʈ�� �ӵ�

    private Transform playerTransform;      // �÷��̾��� Transform


    void Start()
    {
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
            if (true)
            {
                Vector3 direction = (playerTransform.position - bossTransform.position).normalized;

                RaycastHit hit;
                if (Physics.Raycast(bossTransform.position, direction, out hit))
                {
                    // ��� ���� �߰� ���� ���
                    Vector3 middlePoint = bossTransform.position + (hit.point - bossTransform.position) * 0.5f;

                    // ��� ������ �߻�
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
        // ��� �������� �߻��ϱ� �� ��� �ð�
        yield return new WaitForSeconds(warningTime);

        // ��� ������ �߻�
        GameObject laser = Instantiate(projectilePrefab, bossTransform.position, Quaternion.identity);
        Rigidbody laserRigidbody = laser.GetComponent<Rigidbody>();

        // �߻��� �� �������� ����� �� ����
        Vector3 direction = (targetPosition - bossTransform.position).normalized;
        laserRigidbody.velocity = direction * projectileSpeed;

        // ��� �������� ���� �ð� �Ŀ� ��������� ����
        Destroy(laser, 2f);
    }
}