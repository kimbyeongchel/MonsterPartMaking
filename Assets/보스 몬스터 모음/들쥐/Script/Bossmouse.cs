using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bossmouse : Enemy
{
    public CircleCollider2D ccoll;
    public CapsuleCollider2D pcoll;
    public Vector3 initialPosition = new Vector3(7.23999977f, -3.10665798f, 0f);

    // 프리팹 모음
    public GameObject warningBottom;
    public GameObject circleWarning;
    public GameObject groundEffect;
    public GameObject[] throwPrefab;
    private GameObject choiceObject;
    private GameObject prefab_instance;
    public List<GameObject> activePrefabs;

    // 패턴에 사용할 데미지 및 위치, 재질
    public float[] pattern_damage;
    public Transform warningBottom_pos;
    public PhysicsMaterial2D newPhysicsMaterial;

    // phase 상태 변수, 현재 실행 중인 패턴, noise 범위
    public float phase_state = 0f;
    public Coroutine currentPatternCoroutine = null;
    float radius = 1f;

    //move 함수를 위한 변수
    public bool move_attack = false;
    public float distance = 3f;

    //한자 날리기 및 nail 개수 체크
    public int check_gal = 0;

    //구르는 중 표시 변수
    private bool rolling = false;

    //초기 설정
    void Start()
    {
        OnEnable();
        monsterSpeed = 3f;
        rolling = false;
        phase_state = 0f;
        check_gal = 0;
        pattern_damage = new float[5];
        pattern_damage[0] = 10f; // 한자
        pattern_damage[1] = 12f; // 목탁
        pattern_damage[2] = 15f; // 발톱 & 손톱 통일
        pattern_damage[3] = 20f; // noise는 범위 안에 있으면 지속적으로 데미지 입기
        pattern_damage[4] = 30f; // x축, y축, 부딪히는 데미지 => 모두 몸박 데미지 이기 때문에 30으로 통일 -> collider 충돌 데미지

        ccoll = this.GetComponent<CircleCollider2D>(); // 들쥐의 공 상태일 때의 충돌 크기
        pcoll = this.GetComponent<CapsuleCollider2D>(); // 들쥐 기본 상태일 때의 충돌 크기
    }

    // 도령 -> collider로 인한 충돌 off, 들쥐부터 충돌 데미지 ON ( pattern_damage[4] = 30f 적용 )

    //update로 좌우판단 및 사망 상태 확인
    void Update()
    {
        if (dead) return;

        if(!rolling) // 구르는 중이면 방향 바뀌지 않도록 설정
            DirectionEnemy();

        Debug.DrawRay(transform.position + new Vector3(1f, 0f, 0f), Vector2.right * (distance + 0.5f), Color.red);
        Debug.DrawRay(transform.position + new Vector3(-1f, 0f, 0f), Vector2.left * (distance + 0.5f), Color.green);
    }

    // 들쥐 내의 2->3 phase 변경을 위한 색 변경 ( RED )
    void ChangeColor()
    {
        render.color = new Color(0.83f, 0.37f, 0.37f, 1f);
    }

    //phase에 따른 기본 동작 방식 결정 함수
    public void IdleState()
    {
        
        if(move_attack)
        {
            Check_distance();
            ani.SetBool("run", true);
        }
       else 
        {
            if(phase_state == 0)
            {
                ShootObject();
            }
            else if(phase_state == 1)
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.7)
                {
                    ShootObject();
                }
                else
                {
                    ani.SetTrigger("goInitial");
                }
            }
            else if (phase_state == 2)
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.4)
                {
                    //오브젝트 발사
                    ShootObject();
                }
                else if (value > 0.4 && value <= 0.8)
                {
                    //x축 구르기
                    ani.SetTrigger("goInitial");
                }
                else if (value >0.8 && value <= 0.9)
                {
                    //noise
                    ani.SetTrigger("noisePattern");
                }
                else
                {
                    //updown
                    ani.SetTrigger("ready");
                }
            }
            move_attack = true;
        }


        //if (phase_state == 0f) // 도령 상태 : 달리기 및 패턴
        //{
        //    if (move_attack)
        //    {
        //        ani.SetBool("run", true);
        //    }
        //    else
        //    {
        //        ShootObject(); // 패턴에 따라 수정 필요
        //        move_attack = true;
        //    }
        //}
        //else if (phase_state == 1f) // 들쥐 phase 1
        //{
        //    if (move_attack)
        //    {
        //        ani.SetBool("run", true);
        //    }
        //    else
        //    {
        //        double value = rand.NextDouble();
        //        if (value > 0 && value <= 0.7)
        //        {
        //            ShootObject();
        //        }
        //        else
        //        {
        //            ani.SetTrigger("goInitial");
        //        }
        //        move_attack = true;
        //    }
        //}
        //else if (phase_state == 2f) // 들쥐 phase 2
        //{
        //    if (move_attack)
        //    {
        //        ani.SetBool("run", true);
        //    }
        //    else
        //    {
        //        double value = rand.NextDouble();
        //        if (value > 0 && value <= 0.4)
        //        {
        //            //오브젝트 발사
        //            ShootObject();
        //        }
        //        else if(value > 0.4 && value <= 0.8)
        //        {
        //            //x축 구르기
        //            ani.SetTrigger("goInitial");
        //        }
        //        else if(value >0.8 && value <= 0.9)
        //        {
        //            //noise
        //            ani.SetTrigger("noisePattern");
        //        }
        //        else
        //        {
        //            //updown
        //            ani.SetTrigger("ready");
        //        }
        //        move_attack = true;
        //    }
        //}
    }

    public void throw_gal() // 한자 날리기
    {
        prefab_instance = Instantiate(throwPrefab[0], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
        check_gal++;
        if (check_gal == 3)
            ani.SetTrigger("endThrow");
    }

    public void throw_mok() // 목탁 던지기 
    {
        prefab_instance = Instantiate(throwPrefab[1], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
    }

    public void throw_nail() // 손톱 날리기
    {
        int cycleTime = 4;
        
        if (phase_state == 2)
            cycleTime = 6;

        check_gal++;
        prefab_instance = Instantiate(throwPrefab[2], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);

        if (check_gal == cycleTime)
        {
            ani.SetTrigger("endThrow");
            check_gal = 0;
        }
    }

    //주위 거리에서 rayCast 막힌 상태를 인식해서 무작위로 좌우 설정하는 변수. 특정 거리를 이동하도록 특정함..
    public void Check_distance()
    {
        if (phase_state == 0)
            distance = 3f;
        else if (phase_state == 1)
            distance = 4f;
        else
            distance = 5f;

        RaycastHit2D frayHit = Physics2D.Raycast(transform.position + new Vector3(1f, 0f, 0f), Vector2.right, distance); // right
        RaycastHit2D brayHit = Physics2D.Raycast(transform.position + new Vector3(-1f, 0f, 0f), Vector2.left, distance); // left

        distance -= 1f;

        if (frayHit.collider != null && brayHit.collider == null)
        {
            if (frayHit.collider.CompareTag("ground"))
                monsterSpeed = -1f * distance;
            randomMove();
        }
        else if (brayHit.collider != null&&frayHit.collider == null)
        {
            if (brayHit.collider.CompareTag("ground"))
                monsterSpeed = distance;
            randomMove();
        }
        else if (frayHit.collider == null && brayHit.collider == null)
        {
            randomMove();
        }
        else if (frayHit.collider != null && brayHit.collider != null)
        {
            if (frayHit.collider.CompareTag("ground"))
                monsterSpeed = -1f * distance;
            else if (brayHit.collider.CompareTag("ground"))
                monsterSpeed = distance;
        }
    }

    void randomMove() // 무작위 이동
    {
        double value = rand.NextDouble();
        if (value > 0.5)
        {
            monsterSpeed = distance;
        }
        else
        {
            monsterSpeed = -1f * distance;
        }
    }

    public void ShootObject() // 투사체 발사
    {
        switch(phase_state)
        {
            case 0:
                double value = rand.NextDouble();
                if (value > 0.5)
                    ani.SetTrigger("moktak");
                else
                    ani.SetTrigger("gal");
                break;
            case 1:
            case 2:
                ani.SetTrigger("throw_nail");
                break;
        }
    }

    /// <summary>
    /// ///////////////////// 23/11/27 새벽 4시 여기까지 살펴봄
    /// </summary>
    /// <returns></returns>

    public IEnumerator bottomWarning() // state로 넘어가는게 아닌 idleState에서 실행시켜서 해당 코루티 먼저 실행시킨 후에 bottomall로 넘어가게 하면 복잡도가 줄어들듯
    {
        prefab_instance = Instantiate(warningBottom, warningBottom_pos.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(prefab_instance);
    }
   
    //x축 전범위 구르기
    public IEnumerator bottomAll() // x축 전범위 공격
    {
        float moveDuration = 1.5f;
        float moveSpeed = 20f;
        float exitTime = 0f;
        if (phase_state == 2)
            moveSpeed = 30f;

        while (exitTime < moveDuration)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            exitTime += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        
        ani.SetTrigger("bottom_all");
    }

    //소음 공격
    public IEnumerator warningCircle() // 원 공격 주의 + 데미지 인식은 애니메이션에 데미지 함수 FInd 추가
    {
        prefab_instance = Instantiate(circleWarning, transform.position, Quaternion.identity);
        activePrefabs.Add(prefab_instance);
        yield return new WaitForSeconds(1f);
        Destroy(prefab_instance);
        activePrefabs.Remove(prefab_instance);
        ani.SetTrigger("noise");
    }

    public void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D의 sharedMaterial 속성을 사용하여 물리 재질 변경
        rb.sharedMaterial = material;
    }

    //y축 내려찍기 ( 위에 천장이 없는 상태이므로 맵을 포물선으로 바운스 함수
    public IEnumerator pattern_updown()
    {
        bool isRight = target.position.x > transform.position.x;
        float gravityScale = 1f;

        float initialSpeed = Calculate_speed(Physics2D.gravity.y);
        float angle = isRight ? 75f : 105f;

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

    protected override void OnDrawGizmos() // 원 공격 범위 표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override void FindAnd() // Noise 전용 범위 만들기 및 데미지 입히기
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in colliders)
        {
            if (col.tag == "Player")
                UnityEngine.Debug.Log(col.tag);
        }
    }

    //hit를 위한 현재 동작중인 패턴 중단 및 초기화
    public void pattern_check_stop()
    {
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            currentPatternCoroutine = null; // 현재 코루틴을 멈췄으니 초기화
            RemovePrefabs(); // 화면상의 모든 저장된 프리팹들 삭제
        }
    }

    public void RemovePrefabs() // 화면 상의 프리팹들 삭제
    {
        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }
        activePrefabs.Clear();
    }

    //들쥐 상태 부터 collider2D를 활성화 시켜서 닿일 때마다 player hp 감소
    public void SetCollider(int set)
    {
        if (set == 0)
            bColl.enabled = false;
        else
            bColl.enabled = true;
    }

    //사망 시 동작하는 함수
    protected override void Die()
    {
        pattern_check_stop();
        if (phase_state == 0)
        {
            bColl.enabled = false;
            ani.SetTrigger("phase0_die");
            HP = 400f;
            Health.maxValue = HP;
            Health.value = HP;
            pcoll.enabled = true;
        }
        else if (phase_state == 1)
        {
            ani.SetTrigger("phase1_die");
            HP = 400f;
            Health.value = HP;
            ChangeColor();
        }
        else
        {
            dead = true;
            ani.SetTrigger("die");
        }
        phase_state++;
    }

    //일단 충돌 안되는데 나중에 생각하기
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && (phase_state == 1 || phase_state == 2))
        {
            Debug.Log("충돌");
        }
    }
}
