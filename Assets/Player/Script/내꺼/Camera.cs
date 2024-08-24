using System.Collections;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Vector3 initialPos;
    public float shakeAmount;
    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        if (GameManager.gameManager.player.ani.GetBool("Hit"))
            StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {

        transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
        yield return null;

        if (!GameManager.gameManager.player.ani.GetBool("Hit"))
            transform.position = initialPos;
    }
}
