using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public int MAXHP = 100;
    public int moveSpeed = 5;
    public float jumpPower = 5;
    public float rollSpeed = 3;
    public float attackSpeed = 10;
    public int damage = 10;
    public Rigidbody2D rigid;
    public CapsuleCollider2D playerCollider;
    public SpriteRenderer sr;
    public Animator ani;
    public AudioSource audio;


    //public WeaponData.WeaponType weaponType;
    public bool canTakeDamage = true;


    //µ¿Áø
    public bool isSeegRight = true;

    public bool createItem = true;
    public GameObject ApplePrefab;
    public GameObject StrawShouesPrefab;
    public GameObject YakgwaPrefab;
    public GameObject TrapPrefab;
    public GameObject RockPrefab;
    public GameObject RollPaperPrefab;
    public GameObject WeaponPrefab;
    public GameObject HatPrefab;
    public GameObject AmuletPrefab;

    
    public bool canDead = true;
    public int haveAmulet = 0;
    //
    public GameObject inventory;
    //public InventoryManager inventoryManager;

    private void TestItem()
    {
        if (createItem && Input.GetKey(KeyCode.Alpha4))
        {
            GameObject Amulet = Instantiate(AmuletPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha5))
        {
            GameObject Apple = Instantiate(ApplePrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }

        if (createItem && Input.GetKey(KeyCode.Alpha6))
        {
            GameObject Rock = Instantiate(RockPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha7))
        {
            GameObject StrawShoues = Instantiate(StrawShouesPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha8))
        {
            GameObject Hat = Instantiate(HatPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha9))
        {
            GameObject Weapon = Instantiate(WeaponPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
    }
 
    public IEnumerator test()
    {
        yield return new WaitForSeconds(1.0f);
        createItem = true;
    }
    //
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();    
    }

    private void Update()
    {
        TestItem();
        if(this.transform.localScale.x>0)isSeegRight = true;
        else isSeegRight = false;
    }


}
