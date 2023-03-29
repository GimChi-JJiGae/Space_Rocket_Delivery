using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;
    public PlayerInput playerInput;
    public float timeIntervalForSkillTree; // 스킬트리를 열기 위한 시간 간격

    private float surviveTime;
    private bool isGameover;
    private bool isSkillTreeOpen;

    private void Start()
    {
        surviveTime = 0;
        isGameover = false;
        isSkillTreeOpen = false;

        InvokeRepeating("OpenSkillTree", timeIntervalForSkillTree, timeIntervalForSkillTree); // 일정 시간 간격으로 OpenSkillTree 함수 호출
    }

    private void Update()
    {
        if (!isGameover && !isSkillTreeOpen)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time: " + (int)surviveTime;
        }
    }

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

    public void OpenSkillTree()
    {
        isSkillTreeOpen = true;
    }

    public void CloseSkillTree()
    {
        isSkillTreeOpen = false;
    }
}
