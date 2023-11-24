using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�������
// ������ ���, phase 2, 3 ���� ���� ���� �ִϸ��̼��� �����ϱ� ������ �׿� ���� ������ �����ϵ��� �ؾߵ�. -> changeColor() �� ���� �ذ�
// �� phase ���� �޸��� ����( ������ �޸���, ����� �޸��� �� ���� �ʿ� ) -> ������ Ư�� ���� �ÿ��� �޷����� �������� ����
// �� ���ϸ��� ������ ������ ���� ������ -> ����Ǵ� ������ �� �����տ� �־������ �ش� switch���� ����Ͽ� ũ���� ����� �������� ������ ����.
// collider2D�� ���� ������

public class Bossmouse : MonoBehaviour
{
    public Animator bossAni;
    public Rigidbody2D rb;
    public bool dead { get; protected set; }
    public SpriteRenderer render;
    private BoxCollider2D coll;
    public CircleCollider2D ccoll;
    public CapsuleCollider2D pcoll;
    public Vector3 initialPosition = new Vector3(7.23999977f, -3.10665798f, 0f);

    public GameObject warningBottom;
    public GameObject circleWarning;
    public GameObject[] throwPrefab;
    private GameObject choiceObject;
    private GameObject prefab_instance;
    public List<GameObject> activePrefabs; // ȭ�� ���� �����յ�

    public float[] pattern_damage;
    public Transform warningBottom_pos; // x�� ���� ���â ��ġ
    private Transform playerTransform;
    public System.Random rand;
    // y�� 3�� ��⸦ ���� ���ο� ���� ����
    public PhysicsMaterial2D newPhysicsMaterial;

    //���� phase ���¸� �Ǻ��ϱ� ���� ����, 0 = normal, 1 = 1 phase, 2 = 2 phase
    public float phase_state = 0f;
    public Coroutine currentPatternCoroutine = null;
    float radius = 1f;

    //move �Լ��� ���� ����
    public float nextMove = 4f;
    public bool move_attack = false;
    public float distance = 3f;

    //���� ������ ���� üũ
    public int check_gal = 0;

    //hp�� damageText�� ���� ����
    public GameObject damageText;
    public Transform textPos;
    public Slider Health;
    public float HP = 100f;

    //������ �� ǥ�� ����
    private bool rolling = false;

    //�ʱ� ����
    void Start()
    {
        rolling = false;
        Health.value = HP;
        phase_state = 0f;
        check_gal = 0;
        pattern_damage = new float[5];
        pattern_damage[0] = 10f; // ����
        pattern_damage[1] = 12f; // ��Ź
        pattern_damage[2] = 15f; // ���� & ���� ����
        pattern_damage[3] = 20f; // noise�� ���� �ȿ� ������ ���������� ������ �Ա�
        pattern_damage[4] = 30f; // x��, y��, �ε����� ������ => ��� ���� ������ �̱� ������ 30���� ����

        bossAni = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rand = new System.Random();
        render = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = this.GetComponent<BoxCollider2D>();
        ccoll = this.GetComponent<CircleCollider2D>();
        pcoll = this.GetComponent<CapsuleCollider2D>();
    }

    //update�� �¿��Ǵ� �� ��� ���� Ȯ��
    void Update()
    {
        if (dead) return;

        if(!rolling)
            DirectionEnemy(playerTransform.position.x, transform.position.x);
    }

    // ���� ���� 2->3 phase ������ ���� �� ����( �ִϸ��̼� ��ü�� ������� �����ϴ� �� Ȯ����)
    void ChangeColor()
    {
        render.color = new Color(0.83f, 0.37f, 0.37f, 1f);
    }

    //idle ���¿����� �۵���� ����
    public void IdleState()
    {
        //phase ������ ���� ���� ����ȭ �ۼ����

        if (phase_state == 0f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                ShootObject(); // ���Ͽ� ���� ���� �ʿ�
                move_attack = true;
            }
        }
        else if (phase_state == 1f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.7)
                {
                    ShootObject();
                }
                else
                {
                    pcoll.enabled = false;
                    ccoll.enabled = true;
                    bossAni.SetTrigger("goInitial");
                }
                move_attack = true;
            }
        }
        else if (phase_state == 2f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.4)
                {
                    //������Ʈ �߻�
                    ShootObject();
                }
                else if(value > 0.4 && value <= 0.8)
                {
                    //x�� ������
                    pcoll.enabled = false;
                    ccoll.enabled = true;
                    bossAni.SetTrigger("goInitial");
                }
                else if(value >0.8 && value <= 0.9)
                {
                    //noise
                    bossAni.SetTrigger("noisePattern");
                }
                else
                {
                    //updown
                    bossAni.SetTrigger("ready");
                }
                move_attack = true;
            }
        }
    }

    // ���� ������ ���� ������� ��Ź ������ �� ���� �ۼ�
    // ���� 2phase ���°� �Ǹ� �������� ��ü�Ͽ� ���� �� ���� ������ ���( �Ű������� �����ϰų� if���� ���� ���� ��� )
    // ���� ���¿��� ������ �������� �迭 4���� �����Ͽ��� ������ phase_state ������ ���� if�� switch �� ���� ���
    public void throw_gal() // ���� ������ -> �ִϸ��̼� �ݺ��� �̻��� ���� ���� �ִϸ��̼� �Լ��� �߰��Ͽ� �����ϵ��� ��.
    {
        prefab_instance = Instantiate(throwPrefab[0], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
        check_gal++;
        if (check_gal == 3)
            bossAni.SetTrigger("endThrow");
    }

    public IEnumerator throw_mok() // ��Ź ������ 
    {
        yield return new WaitForSeconds(0.5f);
        prefab_instance = Instantiate(throwPrefab[1], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
    }

    public IEnumerator throw_nail()
    {
        if (phase_state == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(0.4f);
                prefab_instance = Instantiate(throwPrefab[2], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
                if (i != 3)
                    yield return new WaitForSeconds(0.3f);
            }
            bossAni.SetTrigger("endThrow");
        }
        else if(phase_state == 2)
        {
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.4f);
                prefab_instance = Instantiate(throwPrefab[2], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
                if (i != 3)
                    yield return new WaitForSeconds(0.3f);
            }
            bossAni.SetTrigger("endThrow");
        }
    }

    //move �Լ� �߰�
    //���� �Ÿ����� rayCast ���� ���¸� �ν��ؼ� �������� �¿� �����ϴ� ���� Ư�� �Ÿ����� �̵��ϵ��� Ư����..
    public void Check_distance()
    {
        if (phase_state == 0)
            distance = 3f;
        else if (phase_state == 1)
            distance = 4f;
        else
        {
            distance = 5f;
        }

        RaycastHit2D frayHit = Physics2D.Raycast(transform.position + new Vector3(1f, 0.1f, 0f), Vector2.right, distance); // right
        RaycastHit2D brayHit = Physics2D.Raycast(transform.position + new Vector3(-1f, 0.1f, 0f), Vector2.left, distance); // left

        if (frayHit.collider != null)
        {
            nextMove = -1f * distance;
        }
        else if (frayHit.collider == null && brayHit.collider == null)
        {
            double value = rand.NextDouble();
            if (value > 0.5)
            {
                nextMove = distance;
            }
            else
            {
                nextMove = -1f *distance;
            }
        }
        else if(frayHit.collider != null && brayHit.collider != null)
        {
            bossAni.SetBool("run", false);
        }
    }

    public void ShootObject() // ����ü �߻�
    {
        double value = rand.NextDouble();

        switch(phase_state)
        {
            case 0:
                if (value > 0.5)
                    bossAni.SetTrigger("moktak");
                else
                    bossAni.SetTrigger("gal");
                break;
            case 1:
            case 2:
                bossAni.SetTrigger("throw_nail");
                break;
        }
    }

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
        
        bossAni.SetTrigger("bottom_all");
    }

    //���� ����
    public IEnumerator warningCircle() // �� ���� ���� + ������ �ν��� �ִϸ��̼ǿ� ������ �Լ� FInd �߰�
    {
        prefab_instance = Instantiate(circleWarning, transform.position, Quaternion.identity);
        activePrefabs.Add(prefab_instance);
        yield return new WaitForSeconds(1f);
        Destroy(prefab_instance);
        activePrefabs.Remove(prefab_instance);
        bossAni.SetTrigger("noise");
    }

    public void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D�� sharedMaterial �Ӽ��� ����Ͽ� ���� ���� ����
        rb.sharedMaterial = material;
    }

    //y�� ������� ( ���� õ���� ���� �����̹Ƿ� ���� ���������� �ٿ �Լ�
    public IEnumerator pattern_updown()
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

    //�ʱ� �ӵ� ���
    float Calculate_speed(float gravity)
    {
        return Mathf.Sqrt(2f* 6f * Mathf.Abs(gravity) / Mathf.Pow(Mathf.Sin(80f * Mathf.Deg2Rad), 2f));
    }

    private void OnDrawGizmos() // �� ���� ���� ǥ��
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void DirectionEnemy(float target, float baseobj) // render �¿� ���� ���ϵ��� ����
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    public void FindAnd() // Noise ���� ���� ����� �� ������ ������
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
            coll.enabled = false;
        else
            coll.enabled = true;
    }

    //��� �� �����ϴ� �Լ�
    void Die()
    {
        pattern_check_stop();
        if (phase_state == 0)
        {
            coll.enabled = false;
            bossAni.SetTrigger("phase0_die");
            HP = 100f;
            Health.value = HP;
            pcoll.enabled = true;
        }
        else if (phase_state == 1)
        {
            bossAni.SetTrigger("phase1_die");
            HP = 100f;
            Health.value = HP;
            ChangeColor(); ;
        }
        else
        {
            dead = true;
            bossAni.SetTrigger("die");
        }
        phase_state++;
    }

    public void bossDelete()
    {
        Destroy(gameObject);
    }

    //������ �Դ� �Լ�(= takeDamage) �ϴ� ����� ������
    public void TakeDamage(int damage)
    {
        if (dead) return;
        textOut(damage);

        if (Health.value <= 0)
        {
            Die();
        }
    }

    // hp �ؽ�Ʈ �� �ֽ�ȭ ���� �Լ� �ۼ�
    void textOut(int damage)
    {
        GameObject hitText = Instantiate(damageText);
        hitText.transform.position = textPos.position;
        hitText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
    }

    //�ϴ� �浹 �ȵǴµ� ���߿� �����ϱ�
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && (phase_state == 1 || phase_state == 2))
        {
            Debug.Log("�浹");
        }
    }

    //    1 Phase
    //  �÷��̾� size�� ����(���� ����) -> ���ݾ� �����̴� �� ��ǥ�� �ؾ߰���
    //  - ���� ������
    //  - ��Ź ������
    // ---> shootObject �Լ��� �ذ�

    //2 Phase
    //  ū ����(ver 1)
    //  - ����, ���� ������
    // ---> shootObject �Լ��� �ذ�
    //  - x�� ������ ������
    // ---> bottomAll �Լ��� �ذ�

    // ������ �����޶��ϸ� ������?
    //  ū ����(ver 2) ��ü �� 20% ���ϰ� ������ ��, Range ���� ����(���� ���� �ٲ� )
    //  - ����, ���� ������ + ������ ���� + ���� ����
    //  - x�� ������ ������ + ������ ���� + �ӵ� ���
    //  - ���� 3����� y�� ������� -> ���� -> ���������� �ٿ�ϸ鼭 ƨ���
    // ---> attackUpDown �Լ��� �ذ�
    //  - ���� ������ �� �������� -> �÷��̾� ���� �޷����� ��ų ���
    // ---> noise �Լ��� �ذ�
}
