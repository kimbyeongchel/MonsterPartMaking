using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Player player;

    private void Awake()
    {
        gameManager = this;
    }
}
