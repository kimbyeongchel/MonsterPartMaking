using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNolbu : MonoBehaviour
{
    public bool dead { get; protected set; }
    private System.Random rand;
    private Animator bossAni;
    private SpriteRenderer render;
    private int hitCount = 0;
    private Transform playerTransform;

    public int patternIndex;
    private Vector3[] attackPositions;
    public GameObject RectWarning;
    public GameObject warningCircle;
    public GameObject arrowRains;

    void Start()
    {
        //�ʱ� ���� ���
        //count ������ ���� �÷��̾�� �´� Ƚ�� ����
        bossAni = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        rand = new System.Random();
        hitCount = 0;

        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0.03f, -0.55f, 0f); // ��� ��ġ
        attackPositions[1] = new Vector3(-5.92f, -0.55f, 0f); // ���� ��ġ
        attackPositions[2] = new Vector3(5.95f, -0.55f, 0f); // ������ ��ġ
    }

    void Update()
    {
        if (hitCount % 4 == 0)
        {
            
        }
        else
        {

        }
        // ���� ���¸� �Ǻ��Ͽ� ���� ��, ��� ó��, ���� �� �� ���� ǥ�� �� üũ
        //�ش� Update���� count�� ����͸��Ͽ� hit ���� ����(= anystate�� ���� hit trigger ���� )
    }

    //��� �� �����ϴ� �Լ�
    void Die()
    {

    }

    //���� ���� 1
    IEnumerator RangeAll() // ������ ����
    {
        for (int i = 0; i < 3; i++)
        {
            patternIndex = Random.Range(0, 3);
            GameObject warningInstance = Instantiate(RectWarning, attackPositions[patternIndex], Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Destroy(warningInstance);
            bossAni.SetTrigger("arrowUP");
            arrowRain(patternIndex);
        }
    }

    void arrowRain(int patternIndex)
    {
        GameObject rain = Instantiate(arrowRains, attackPositions[patternIndex] + new Vector3(0f, 1.12f, 0f), Quaternion.identity);
    }

    //���� ���� 2

    //���� ���� 3

    //��� ���� ( �׳� ��ź ��ȯ => ��ź ��ü�� ��ũ��Ʈ�� ���� )

    //hit �� �����ϴ� �Լ�. �Լ� ���� ��ü���� ���� ���� ���� ����, Transform ��ġ �̵� �� ������ȭ ��ȯ

    //������ �Դ� �Լ�(= takeDamage)
}
