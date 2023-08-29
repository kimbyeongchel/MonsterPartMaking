using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nolbu : MonoBehaviour
{
    public Transform bossTransform;
    public float patternInterval = 2f;
    private float currentTime = 0f;
    private int patternIndex = 0;
    private Vector3[] attackPositions;
    private Animator bossAnimator;
    public System.Random rand;
    public float SetTime;
    private bool takeAttack = false;
    public bool dead { get; protected set; }
    private BoxCollider2D coll;
    private int count = 0;
    private Vector3 direction;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider Health;
    public Transform HPPos;
    public float HP = 7f;
    public float speed = 2f;
    private List<GameObject> activePrefabs;
    public GameObject[] money;
    public GameObject warningEffectPrefab;
    private GameObject warningEffectInstance;
    public GameObject warningCircle;

    public GameObject coinBomb;
    public GameObject projectilePrefab;    // 발사할 오브젝트 프리팹
    public float projectileSpeed = 10f;     // 발사할 오브젝트의 속도
    private Transform playerTransform;      // 플레이어의 Transform
    private Coroutine currentPatternCoroutine = null;


    void Start()
    {
        Health.value = HP;
        activePrefabs = new List<GameObject>();
        rand = new System.Random();
        bossAnimator = GetComponent<Animator>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0f, -1.80125f, 0f); // 가운데 위치
        attackPositions[1] = new Vector3(-5f, -1.80125f, 0f); // 왼쪽 위치
        attackPositions[2] = new Vector3(7.25f, -1.80125f, 0f); // 오른쪽 위치
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (dead) return;


        bossAnimator.SetBool("back", false);
        currentTime += Time.deltaTime;
        if (currentTime >= patternInterval)
        {
            activePrefabs.Clear();
            double value = rand.NextDouble();
            if (value > 0 && value <= 0.4)
            {
                currentPatternCoroutine = StartCoroutine(ShootWarningLand());

            }
            else if (0.7 >= value && value > 0.4)
            {
                currentPatternCoroutine = StartCoroutine(ExecutePatternAll());
            }
            else
            {
                currentPatternCoroutine = StartCoroutine(ExecurePatternCircle());
            }
            currentTime= 0f;
        }
    }

    IEnumerator ExecutePatternAll() // 전범위 공격
    {
        activePrefabs.Clear();
        for (int i = 0; i < 3; i++)
        {
            patternIndex = Random.Range(0, 3);
            warningEffectInstance = Instantiate(warningEffectPrefab, attackPositions[patternIndex], Quaternion.identity);
            activePrefabs.Add(warningEffectInstance);
            yield return new WaitForSeconds(0.5f);
            Destroy(warningEffectInstance);
            activePrefabs.Remove(warningEffectInstance);
            bossAnimator.SetTrigger("arrowUP");

            yield return new WaitForSeconds(0.5f);
            currentTime= 0f;
        }

        currentPatternCoroutine = null;
    }

    IEnumerator ExecurePatternCircle() // 원 공격
    {
        activePrefabs.Clear();
        bossAnimator.SetBool("nono", false);
        warningEffectInstance = Instantiate(warningCircle, bossTransform.position, Quaternion.identity);
        activePrefabs.Add(warningEffectInstance);
        yield return new WaitForSeconds(1f);
        Destroy(warningEffectInstance);
        activePrefabs.Remove(warningEffectInstance);
        bossAnimator.SetTrigger("noise");
        currentTime= 0f;
        yield return new WaitForSeconds(0.5f);
        bossAnimator.SetBool("nono", true);

        currentPatternCoroutine = null;
    }

    IEnumerator ShootWarningLand() // 투사체 발사
    {
        for (int i = 0; i < 5; i++)
        {
            bossAnimator.SetTrigger("throw");
            yield return new WaitForSeconds(0.2f);
            GameObject land = Instantiate(projectilePrefab, bossTransform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            Rigidbody2D laserRigidbody = land.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = (playerTransform.position + new Vector3(0f, 1f, 0f) - bossTransform.position).normalized;

            laserRigidbody.velocity = targetPosition * projectileSpeed;

            Destroy(land, 3f);
            yield return new WaitForSeconds(0.5f);
            currentTime = 0f;
        }
        bossAnimator.SetBool("back", true);
        currentPatternCoroutine = null;
    }

    IEnumerator TakeDamageRoutine(int damage)
    {
        count++;
        takeAttack = true;

        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);

        if (count % 2 == 0)
        {
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);
                foreach (var prefab in activePrefabs)
                {
                    Destroy(prefab);
                }
                activePrefabs.Clear();
                currentPatternCoroutine = null;
            }
            bossAnimator.SetBool("hit", true);
            coll.enabled = false;

            yield return new WaitForSeconds(0.7f);
            bossAnimator.SetBool("hit", false);
            yield return new WaitForSeconds(0.34f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, 0f);
            yield return new WaitForSeconds(4f);

            while (currentPatternCoroutine != null)
            {
                yield return null;
            }
            coll.enabled = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - 1.2f, 0f);
            adActiveMoney(0);

        }
        else
        {
            coll.enabled = false;
            yield return new WaitForSeconds(1f);
            coll.enabled = true;
        }


        if(Health.value == 2f)
        {
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);

                foreach (var prefab in activePrefabs)
                {
                    Destroy(prefab);
                }
                activePrefabs.Clear();
                currentPatternCoroutine = null;
            }

            double value = rand.NextDouble();
            if(value < 0.5)
            {
                HP--;
                bossAnimator.SetTrigger("throw");
                GameObject land = Instantiate(coinBomb, bossTransform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            }
        }
        else if (Health.value <= 0f)
        {
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);

                foreach (var prefab in activePrefabs)
                {
                    Destroy(prefab);
                }
                activePrefabs.Clear();
                currentPatternCoroutine = null;
            }
            dead = true;
            bossAnimator.SetTrigger("die");
            Invoke("SetFalse", SetTime);
        }
        takeAttack = false;

        yield return new WaitForEndOfFrame();
    }


    public void TakeDamage(int damage)
    {
        if (dead) return;

        if (takeAttack)
            return;

        StartCoroutine(TakeDamageRoutine(damage));
    }

    private void SetFalse()
    {
        Destroy(gameObject);
    }

    public void SetActiveMoney(int index)
    {
        money[index].SetActive(true);
    }

    public void adActiveMoney(int index)
    {
        money[index].SetActive(false);
    }
}