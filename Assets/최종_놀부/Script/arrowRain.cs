using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowRain : MonoBehaviour
{
    public Transform pos;
    public Vector2 boxsize;

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos() // ������ �� �� �ڵ� �����.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    void FindAnd()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                TargetControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<TargetControl>();
                player.Hit(30);
            }
        }
    }
}
