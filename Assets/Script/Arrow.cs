using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyArrow", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * 1f * speed * Time.deltaTime);
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("player¿¡°Ô arrow ¸ÂÃã");
            
        }
        DestroyArrow();
    }
}
