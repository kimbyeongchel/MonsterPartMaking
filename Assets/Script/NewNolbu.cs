using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNolbu : MonoBehaviour
{
    public bool dead { get; protected set; }
    public Animator bossAni;
    public int hitCount = 0;
    private System.Random rand;
    private SpriteRenderer render;
    private Transform playerTransform;

    public int patternIndex;
    private Vector3[] attackPositions;
    private GameObject warningInstance;
    public GameObject RectWarning;
    public GameObject circleWarning;
    public GameObject arrowRains;

    public List<GameObject> activePrefabs;
    public Coroutine currentPatternCoroutine = null;

    void Start()
    {
        //�ʱ� ���� ���
        bossAni = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        rand = new System.Random();
        hitCount = 0; // arrowRain 3�� �ݺ��� ���� ī���� ����

        activePrefabs = new List<GameObject>();
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
        //�ش� Update���� count�� ����͸��Ͽ� hit ���� ����(= anystate�� ���� hit trigger ���� ) hp�� �ص� �ɵ�?
    }

    //idle ���¿����� �۵���� ����
    public void IdleState()
    {
        double value = rand.NextDouble();
        if (value > 0 && value <= 0.4)
        {
            bossAni.SetTrigger("arrowUP");
        }
        else if (0.7 >= value && value > 0.4)
        {
            bossAni.SetTrigger("patternTwo");
        }
        else
        {
            Debug.Log("num3");
        }
    }


    //��� �� �����ϴ� �Լ�
    void Die()
    {

    }

    //���� ���� 1
    public IEnumerator RangeAll() // ������ ����
    {
        patternIndex = Random.Range(0, 3);
        warningInstance = Instantiate(RectWarning, attackPositions[patternIndex], Quaternion.identity);
        activePrefabs.Add(warningInstance);
        yield return new WaitForSeconds(0.5f);
        Destroy(warningInstance);
        activePrefabs.Remove(warningInstance);
        bossAni.SetBool("arrowRe", true);
        arrowRain(patternIndex);
    }

    void arrowRain(int patternIndex)
    {
        GameObject rain = Instantiate(arrowRains, attackPositions[patternIndex] + new Vector3(0f, 1.12f, 0f), Quaternion.identity);
    }

    //���� ���� 2
    public IEnumerator NoiseCircle() // �� ����
    {
        //���⿡ ������ �ִ� �ڵ� �ۼ� ����
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator warningCircle() // �� ���� ����
    {
        warningInstance = Instantiate(circleWarning, transform.position, Quaternion.identity);
        activePrefabs.Add(warningInstance);
        yield return new WaitForSeconds(1f);
        Destroy(warningInstance);
        activePrefabs.Remove(warningInstance);
        bossAni.SetTrigger("noise");
    }


    //���� ���� 3

    //��� ���� ( �׳� ��ź ��ȯ => ��ź ��ü�� ��ũ��Ʈ�� ���� )

    //hit �� �����ϴ� �Լ�. �Լ� ���� ��ü���� ���� ���� ���� ����, Transform ��ġ �̵� �� ������ȭ ��ȯ

    //������ �Դ� �Լ�(= takeDamage) �ϴ� ����� ������
    public void TakeDamage(int damage)
    {
        if (dead) return;

        bossAni.SetTrigger("hit");
    }

    public void RemovePrefabs() // ȭ�� ���� �����յ� ����
    {
        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }
        activePrefabs.Clear();
    }
}
