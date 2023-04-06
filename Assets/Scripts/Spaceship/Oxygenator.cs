using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygenator : MonoBehaviour
{
    public float oxygen = 100f; // 총 2분 산소 100
    public float decreaseAmount = 0.834f; // 매 초 일정량 감소 ( 100 / 120 )
    public float increaseAmount = 41.7f; // 연료 주입 시 50초의 산소 양만큼 증가

    public Image oxygenBarFilled; // 게이지 UI의 이미지 컴포넌트

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(Decrease), 1.0f, 1.0f);
        oxygenBarFilled = FindAnyObjectByType<FindO2Fill>().gameObject.GetComponent<Image>();//GameObject.Find("GameUiCanvas").transform.Find("o2fill").GetComponent<Image>();// GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (oxygen <= 0f)
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            if (!gameManager.isGameover)
            {
                gameManager.EndGame();
            }
        }

        // oxygenBarFilled 이미지의 fillAmount 값을 현재 oxygen 값에 따라 변경
        oxygenBarFilled.fillAmount = oxygen / 100f;
    }


    private void Decrease()
    {
        oxygen -= decreaseAmount;
    }

    public void Increase()
    {
        oxygen += increaseAmount;
        Debug.Log(oxygen);

        if (oxygen > 100)
        {
            oxygen = 100;
        }
    }
}
