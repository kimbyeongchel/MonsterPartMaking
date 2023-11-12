using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmouse : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    public GameObject warningBottom;
    public GameObject projectilePrefab;
    public Transform pos;
    private Transform playerTransform;
    private bool pattern = false;
    private Vector3 direction;
    public System.Random rand;
    int count = 1;
    bool ground;
    // y축 3번 찍기를 위한 새로운 물리 재질
    public PhysicsMaterial2D newPhysicsMaterial;
    float time;
    //초기 설정
    void Start()
    {
        time = 0f;
        count = 1;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rand = new System.Random();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeMaterial(newPhysicsMaterial);
        StartCoroutine(patter_updown());
    }
    ////update로 좌우판단 및 사망 상태 확인
    //void Update()
    //{
    //    if (dead) return;

    //    DirectionEnemy(playerTransform.position.x, transform.position.x);
    //}

    void Update()
    {
        time += Time.deltaTime;
        if(time >= 6f)
        {
            ChangeMaterial(null);
        }

    }

    ////idle 상태에서의 작동방식 조절
    //public void IdleState()
    //{
    //    //각 Phase마다 상태를 체크하는 변수 phase 상태 확인 및 상태에 따른 애니메이션 변신 트리거 설정
    //    if (Health.value == 1 && oneTime == true)
    //    {
    //        bossAni.SetTrigger("Bomb");
    //        oneTime = false;
    //    }

    //    //phase 변수에 따른 패턴 세분화 작성요망
    //    double value = rand.NextDouble();
    //    if (value > 0 && value <= 0.4)
    //    {
    //        bossAni.SetTrigger("arrowUP");
    //    }
    //    else if (0.7 >= value && value > 0.4)
    //    {
    //        bossAni.SetTrigger("patternTwo");
    //    }
    //    else
    //    {
    //        bossAni.SetTrigger("patternThree");
    //    }
    //}

    // 도령 상태의 한자 날리기와 목탁 던지기 두 패턴 작성
    // 만약 2phase 상태가 되면 프리팹을 교체하여 발톱 및 손톱 던지기 사용( 매개변수로 지정하거나 if문을 통한 구분 요망 )
    IEnumerator ShootWarningLand() // 투사체 발사
    {
        pattern = true;
        for (int i = 0; i < 5; i++)
        {
            ani.SetTrigger("throw");
            yield return new WaitForSeconds(0.2f);
            GameObject land = Instantiate(projectilePrefab, transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            Rigidbody2D laserRigidbody = land.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = (playerTransform.position + new Vector3(0f, 1f, 0f) - transform.position).normalized;

            laserRigidbody.velocity = targetPosition * 20f;

            Destroy(land, 3f);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        pattern = false;
    }

    //x축 전범위 구르기
    IEnumerator bottomAll() // x축 전범위 공격
    {
        pattern = true;
        GameObject warningEffectInstance = Instantiate(warningBottom, pos.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(warningEffectInstance);
        ani.SetBool("slider", true);
        float moveDuration = 1.5f;
        float moveSpeed = 20f;
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(-moveDuration * moveSpeed, 0f, 0f);

        while (elapsedTime < moveDuration)
        {
            // Interpolate the position over time using Lerp.
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame.
        }

        transform.position = targetPosition;
        ani.SetBool("slider", false);
        transform.position = initialPosition;
        yield return new WaitForSeconds(2f);
        pattern = false;
    }

    //소음 공격

    void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D의 sharedMaterial 속성을 사용하여 물리 재질 변경
        rb.sharedMaterial = material;
    }

    //y축 내려찍기 ( 위에 천장이 없는 상태이므로 맵을 포물선으로 바운스 함수
    IEnumerator patter_updown()
    {
        bool isRight = playerTransform.position.x > transform.position.x;
        float gravityScale = 1f;

        float initialSpeed = Calculate_speed(Physics2D.gravity.y);
        float angle = isRight ? 80f : 100f;

        float radianAngle = angle * Mathf.Deg2Rad;

        float VelocityX = initialSpeed * Mathf.Cos(radianAngle);
        float VelocityY = initialSpeed * Mathf.Sin(radianAngle);

        rb.velocity = new Vector2(VelocityX, VelocityY);
        rb.gravityScale = gravityScale;
        yield return new WaitForSeconds(1f);
        rb.gravityScale = 5* gravityScale;
    }

    //초기 속도 계산
    float Calculate_speed(float gravity)
    {
        return Mathf.Sqrt(2f* 6f * Mathf.Abs(gravity) / Mathf.Pow(Mathf.Sin(80f * Mathf.Deg2Rad), 2f));
    }

//    1 Phase
//  플레이어 size의 도령(변신 상태) -> 조금씩 움직이는 걸 목표로 해야겠음
//  - 한자 날리기
//  - 목탁 던지기

//2 Phase
//  큰 들쥐(ver 1)
//  - 발톱, 손톱 던지기
//  - x축 전범위 구르기

    // 점프도 만들어달라하면 좋을듯?
//  큰 들쥐(ver 2) 전체 피 20% 이하가 남았을 때, Range 상태 돌입(들쥐 색깔 바뀜 )
//  - 발톱, 손톱 던지기 + 데미지 증가 + 범위 증가
//  - x축 전범위 구르기 + 데미지 증가 + 속도 향상
//  - 맵을 3등분한 y축 내려찍기 -> 변경 -> 포물선으로 바운스하면서 튕기기
//  - 원을 범위로 한 소음공격 -> 플레이어 에게 달려가서 스킬 사용
}
