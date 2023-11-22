using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private SpriteRenderer render;
    private BoxCollider2D coll;

    public GameObject warningBottom;
    public GameObject circleWarning;
    public GameObject[] throwPrefab;
    private GameObject choiceObject;
    private GameObject prefab_instance;
    public List<GameObject> activePrefabs; // ȭ�� ���� �����յ�

    private float[] pattern_damage;
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
    public float nextMove = 1f;
    public bool move_attack = false;

    //�ʱ� ����
    void Start()
    {
        phase_state = 0f;
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
    }

    //update�� �¿��Ǵ� �� ��� ���� Ȯ��
    void Update()
    {
        if (dead) return;

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
                ShootObject();
                move_attack = true;
            }
        }
    }

    // ���� ������ ���� ������� ��Ź ������ �� ���� �ۼ�
    // ���� 2phase ���°� �Ǹ� �������� ��ü�Ͽ� ���� �� ���� ������ ���( �Ű������� �����ϰų� if���� ���� ���� ��� )
    // ���� ���¿��� ������ �������� �迭 4���� �����Ͽ��� ������ phase_state ������ ���� if�� switch �� ���� ���
    public IEnumerator throw_gal() // ���� ������
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            prefab_instance = Instantiate(throwPrefab[0], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            if (i != 4)
            {
                yield return new WaitForSeconds(0.6f);
            }
        }
        bossAni.SetTrigger("endThrow");
    }

    public IEnumerator throw_mok() // ��Ź ������
    {
        yield return new WaitForSeconds(0.5f);
        prefab_instance = Instantiate(throwPrefab[1], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
    }

    //move �Լ� �߰�
    //���� �Ÿ����� rayCast ���� ���¸� �ν��ؼ� �������� �¿� �����ϴ� ���� Ư�� �Ÿ����� �̵��ϵ��� Ư����..
    public void Check_distance()
    {
        RaycastHit2D frayHit = Physics2D.Raycast(transform.position + new Vector3(1f, 0.1f, 0f), Vector2.right, 2f); // right
        RaycastHit2D brayHit = Physics2D.Raycast(transform.position + new Vector3(-1f, 0.1f, 0f), Vector2.left, 2f); // left

        if (frayHit.collider != null)
        {
            nextMove = -2f;
        }
        else if (frayHit.collider == null && brayHit.collider == null)
        {
            double value = rand.NextDouble();
            if (value > 0.5)
            {
                nextMove = 2f;
            }
            else
            {
                nextMove = -2f;
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
            //case 1:
            //    if (value > 0.5)
            //        choiceObject = throwPrefab[2];
            //    else
            //        choiceObject = throwPrefab[3];
            //    break;
            //case 2:
            //    if (value > 0.5)
            //        choiceObject = throwPrefab[2];
            //    else
            //        choiceObject = throwPrefab[3];

            //    choiceObject.transform.localScale = new Vector3(2f, 2f, 0f);
            //    // collider2D ũ�� ������ �ʿ���. -> �������� ������ ���� ������ ���ؼ�
            //    coll.size = new Vector3(2f, 2f, 0);
            //    // ������ ������ �ʿ��ҵ�
            //    pattern_damage[2] = 24f;
            //    break;
        }

        //for (int i = 0; i < 5; i++)
        //{
        //    yield return new WaitForSeconds(0.2f);
        //    prefab_instance = Instantiate(choiceObject, transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
        //    if (i != 4)
        //    {
        //        yield return new WaitForSeconds(0.6f);
        //    }
        //}
        //bossAni.SetTrigger("endShoot");
    }

    //x�� ������ ������
    IEnumerator bottomAll() // x�� ������ ����
    {
        prefab_instance = Instantiate(warningBottom, warningBottom_pos.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(prefab_instance);
        bossAni.SetBool("slider", true);
        float moveDuration = 1.5f;
        float moveSpeed = 20f;
        float exitTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(-moveDuration * moveSpeed, 0f, 0f);

        while (exitTime < moveDuration)
        { 
            transform.position = Vector3.Lerp(initialPosition, targetPosition, exitTime / moveDuration);
            exitTime += Time.deltaTime;
        }

        transform.position = targetPosition;
        bossAni.SetBool("slider", false);
        transform.position = initialPosition;
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

    void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D�� sharedMaterial �Ӽ��� ����Ͽ� ���� ���� ����
        rb.sharedMaterial = material;
    }

    //y�� ������� ( ���� õ���� ���� �����̹Ƿ� ���� ���������� �ٿ �Լ�
    IEnumerator pattern_updown()
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

    //���� ������ Ȱ���� phase ��ȯ �Լ�
    public void Change_phase()
    {
        if(phase_state == 1 && phase_state == 2)
        {
            bossAni.SetTrigger("Phase_update");
        }
    }

    //��� �� �����ϴ� �Լ�
    void Die()
    {
        dead = true;
        pattern_check_stop();
        bossAni.SetTrigger("die");
    }

    void bossDelete()
    {
        Destroy(gameObject);
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
