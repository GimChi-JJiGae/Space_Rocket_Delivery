using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] modules; // 인스펙터에서 지정할 모듈 배열

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (!gameManager.IsGameOver())
        {
            //Debug.Log('7');
        }
    }
}
