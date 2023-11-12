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
    // y�� 3�� ��⸦ ���� ���ο� ���� ����
    public PhysicsMaterial2D newPhysicsMaterial;
    float time;
    //�ʱ� ����
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
    ////update�� �¿��Ǵ� �� ��� ���� Ȯ��
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

    ////idle ���¿����� �۵���� ����
    //public void IdleState()
    //{
    //    //�� Phase���� ���¸� üũ�ϴ� ���� phase ���� Ȯ�� �� ���¿� ���� �ִϸ��̼� ���� Ʈ���� ����
    //    if (Health.value == 1 && oneTime == true)
    //    {
    //        bossAni.SetTrigger("Bomb");
    //        oneTime = false;
    //    }

    //    //phase ������ ���� ���� ����ȭ �ۼ����
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

    // ���� ������ ���� ������� ��Ź ������ �� ���� �ۼ�
    // ���� 2phase ���°� �Ǹ� �������� ��ü�Ͽ� ���� �� ���� ������ ���( �Ű������� �����ϰų� if���� ���� ���� ��� )
    IEnumerator ShootWarningLand() // ����ü �߻�
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

    //x�� ������ ������
    IEnumerator bottomAll() // x�� ������ ����
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

    //���� ����

    void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D�� sharedMaterial �Ӽ��� ����Ͽ� ���� ���� ����
        rb.sharedMaterial = material;
    }

    //y�� ������� ( ���� õ���� ���� �����̹Ƿ� ���� ���������� �ٿ �Լ�
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

    //�ʱ� �ӵ� ���
    float Calculate_speed(float gravity)
    {
        return Mathf.Sqrt(2f* 6f * Mathf.Abs(gravity) / Mathf.Pow(Mathf.Sin(80f * Mathf.Deg2Rad), 2f));
    }

//    1 Phase
//  �÷��̾� size�� ����(���� ����) -> ���ݾ� �����̴� �� ��ǥ�� �ؾ߰���
//  - ���� ������
//  - ��Ź ������

//2 Phase
//  ū ����(ver 1)
//  - ����, ���� ������
//  - x�� ������ ������

    // ������ �����޶��ϸ� ������?
//  ū ����(ver 2) ��ü �� 20% ���ϰ� ������ ��, Range ���� ����(���� ���� �ٲ� )
//  - ����, ���� ������ + ������ ���� + ���� ����
//  - x�� ������ ������ + ������ ���� + �ӵ� ���
//  - ���� 3����� y�� ������� -> ���� -> ���������� �ٿ�ϸ鼭 ƨ���
//  - ���� ������ �� �������� -> �÷��̾� ���� �޷����� ��ų ���
}
