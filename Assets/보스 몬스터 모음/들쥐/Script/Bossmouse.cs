using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bossmouse : Enemy
{
    public CircleCollider2D ccoll;
    public CapsuleCollider2D pcoll;
    public Vector3 initialPosition = new Vector3(7.23999977f, -3.10665798f, 0f);

    // ������ ����
    public GameObject warningBottom;
    public GameObject circleWarning;
    public GameObject groundEffect;
    public GameObject[] throwPrefab;
    private GameObject choiceObject;
    private GameObject prefab_instance;
    public List<GameObject> activePrefabs;

    // ���Ͽ� ����� ������ �� ��ġ, ����
    public float[] pattern_damage;
    public Transform warningBottom_pos;
    public PhysicsMaterial2D newPhysicsMaterial;

    // phase ���� ����, ���� ���� ���� ����, noise ����
    public float phase_state = 0f;
    public Coroutine currentPatternCoroutine = null;
    float radius = 1f;

    //move �Լ��� ���� ����
    public bool move_attack = false;
    public float distance = 3f;

    //���� ������ �� nail ���� üũ
    public int check_gal = 0;

    //������ �� ǥ�� ����
    private bool rolling = false;

    //�ʱ� ����
    void Start()
    {
        OnEnable();
        monsterSpeed = 3f;
        rolling = false;
        phase_state = 0f;
        check_gal = 0;
        pattern_damage = new float[5];
        pattern_damage[0] = 10f; // ����
        pattern_damage[1] = 12f; // ��Ź
        pattern_damage[2] = 15f; // ���� & ���� ����
        pattern_damage[3] = 20f; // noise�� ���� �ȿ� ������ ���������� ������ �Ա�
        pattern_damage[4] = 30f; // x��, y��, �ε����� ������ => ��� ���� ������ �̱� ������ 30���� ���� -> collider �浹 ������

        ccoll = this.GetComponent<CircleCollider2D>(); // ������ �� ������ ���� �浹 ũ��
        pcoll = this.GetComponent<CapsuleCollider2D>(); // ���� �⺻ ������ ���� �浹 ũ��
    }

    // ���� -> collider�� ���� �浹 off, ������� �浹 ������ ON ( pattern_damage[4] = 30f ���� )

    //update�� �¿��Ǵ� �� ��� ���� Ȯ��
    void Update()
    {
        if (dead) return;

        if(!rolling) // ������ ���̸� ���� �ٲ��� �ʵ��� ����
            DirectionEnemy();

        Debug.DrawRay(transform.position + new Vector3(1f, 0f, 0f), Vector2.right * (distance + 0.5f), Color.red);
        Debug.DrawRay(transform.position + new Vector3(-1f, 0f, 0f), Vector2.left * (distance + 0.5f), Color.green);
    }

    // ���� ���� 2->3 phase ������ ���� �� ���� ( RED )
    void ChangeColor()
    {
        render.color = new Color(0.83f, 0.37f, 0.37f, 1f);
    }

    //phase�� ���� �⺻ ���� ��� ���� �Լ�
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
                    //������Ʈ �߻�
                    ShootObject();
                }
                else if (value > 0.4 && value <= 0.8)
                {
                    //x�� ������
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


        //if (phase_state == 0f) // ���� ���� : �޸��� �� ����
        //{
        //    if (move_attack)
        //    {
        //        ani.SetBool("run", true);
        //    }
        //    else
        //    {
        //        ShootObject(); // ���Ͽ� ���� ���� �ʿ�
        //        move_attack = true;
        //    }
        //}
        //else if (phase_state == 1f) // ���� phase 1
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
        //else if (phase_state == 2f) // ���� phase 2
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
        //            //������Ʈ �߻�
        //            ShootObject();
        //        }
        //        else if(value > 0.4 && value <= 0.8)
        //        {
        //            //x�� ������
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

    public void throw_gal() // ���� ������
    {
        prefab_instance = Instantiate(throwPrefab[0], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
        check_gal++;
        if (check_gal == 3)
            ani.SetTrigger("endThrow");
    }

    public void throw_mok() // ��Ź ������ 
    {
        prefab_instance = Instantiate(throwPrefab[1], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
    }

    public void throw_nail() // ���� ������
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

    //���� �Ÿ����� rayCast ���� ���¸� �ν��ؼ� �������� �¿� �����ϴ� ����. Ư�� �Ÿ��� �̵��ϵ��� Ư����..
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

    void randomMove() // ������ �̵�
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

    public void ShootObject() // ����ü �߻�
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
    /// ///////////////////// 23/11/27 ���� 4�� ������� ���캽
    /// </summary>
    /// <returns></returns>

    public IEnumerator bottomWarning()
    {
        prefab_instance = Instantiate(warningBottom, warningBottom_pos.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(prefab_instance);
    }
   
    //x�� ������ ������
    public IEnumerator bottomAll() // x�� ������ ����
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

    //���� ����
    public IEnumerator warningCircle() // �� ���� ���� + ������ �ν��� �ִϸ��̼ǿ� ������ �Լ� FInd �߰�
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
        // Rigidbody2D�� sharedMaterial �Ӽ��� ����Ͽ� ���� ���� ����
        rb.sharedMaterial = material;
    }

    //y�� ������� ( ���� õ���� ���� �����̹Ƿ� ���� ���������� �ٿ �Լ�
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

    //�ʱ� �ӵ� ���
    float Calculate_speed(float gravity)
    {
        return Mathf.Sqrt(2f* 6f * Mathf.Abs(gravity) / Mathf.Pow(Mathf.Sin(80f * Mathf.Deg2Rad), 2f));
    }

    protected override void OnDrawGizmos() // �� ���� ���� ǥ��
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override void FindAnd() // Noise ���� ���� ����� �� ������ ������
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in colliders)
        {
            if (col.tag == "Player")
                UnityEngine.Debug.Log(col.tag);
        }
    }

    //hit�� ���� ���� �������� ���� �ߴ� �� �ʱ�ȭ
    public void pattern_check_stop()
    {
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            currentPatternCoroutine = null; // ���� �ڷ�ƾ�� �������� �ʱ�ȭ
            RemovePrefabs(); // ȭ����� ��� ����� �����յ� ����
        }
    }

    public void RemovePrefabs() // ȭ�� ���� �����յ� ����
    {
        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }
        activePrefabs.Clear();
    }

    //���� ���� ���� collider2D�� Ȱ��ȭ ���Ѽ� ���� ������ player hp ����
    public void SetCollider(int set)
    {
        if (set == 0)
            bColl.enabled = false;
        else
            bColl.enabled = true;
    }

    //��� �� �����ϴ� �Լ�
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

    //�ϴ� �浹 �ȵǴµ� ���߿� �����ϱ�
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && (phase_state == 1 || phase_state == 2))
        {
            Debug.Log("�浹");
        }
    }
}
