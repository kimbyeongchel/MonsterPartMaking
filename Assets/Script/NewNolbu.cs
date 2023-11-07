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
        //초기 설정 사용
        //count 변수를 통해 플레이어에게 맞는 횟수 판정
        bossAni = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        rand = new System.Random();
        hitCount = 0;

        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0.03f, -0.55f, 0f); // 가운데 위치
        attackPositions[1] = new Vector3(-5.92f, -0.55f, 0f); // 왼쪽 위치
        attackPositions[2] = new Vector3(5.95f, -0.55f, 0f); // 오른쪽 위치
    }

    void Update()
    {
        if (hitCount % 4 == 0)
        {
            
        }
        else
        {

        }
        // 보스 상태를 판별하여 스턴 중, 사망 처리, 공격 중 등 상태 표현 및 체크
        //해당 Update에서 count를 모니터링하여 hit 동작 실행(= anystate를 통한 hit trigger 실행 )
    }

    //사망 시 동작하는 함수
    void Die()
    {

    }

    //공격 패턴 1
    IEnumerator RangeAll() // 전범위 공격
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

    //공격 패턴 2

    //공격 패턴 3

    //즉사 패턴 ( 그냥 폭탄 소환 => 폭탄 자체의 스크립트로 날라감 )

    //hit 시 동작하는 함수. 함수 내에 전체적인 스턴 상태 변수 수정, Transform 위치 이동 및 금은보화 소환

    //데미지 입는 함수(= takeDamage)
}
