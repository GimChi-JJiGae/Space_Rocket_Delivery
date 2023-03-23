using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 라이브러리
using UnityEngine.SceneManagement; // 씬 관련 라이브러리

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // 게임오버 시 활성화할 텍스트 게임 오브젝트
    public Text timeText; // 생존 시간을 표시할 텍스트 컴포넌트
    public Text recordText; // 최고 기록을 표시할 텍스트 컴포넌트

    public PlayerInput playerInput;

    private float surviveTime; // 생존 시간
    private bool isGameover; // 게임오버 상태

    private void Start()
    {
        // 생존 시간, 게임 오버 상태 초기화
        surviveTime = 0;
        isGameover = false;
    }

    private void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time: " + (int)surviveTime;
        }

        // 게임오버 상태에서 Restart 버튼 누른 경우
        //else
        //{
        //    if (playerInput.Restart) {
        //        SceneManager.LoadScene("SampleScene");
        //    }
        //} 
    }


    // 현재 게임을 게임오버 상태로 변경하는 Method
    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;

            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        recordText.text = "Best Time: " + (int)bestTime;
    }
}