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

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rand = new System.Random();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!pattern)
        {
            double value = rand.NextDouble();
            if (value > 0 && value <= 0.5)
            {
                StartCoroutine(ShootWarningLand());
            }
            else
            {
                StartCoroutine(bottomAll());
            }
        }
    }

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
    //activePrefabs.Add(warningEffectInstance);
    //
    //
    //activePrefabs.Remove(warningEffectInstance);
    //bossAnimator.SetTrigger("noise");
    //currentTime = 0f;
    //yield return new WaitForSeconds(0.5f);
    //bossAnimator.SetBool("nono", true);
    //currentPatternCoroutine = null;
}
