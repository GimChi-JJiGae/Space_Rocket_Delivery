using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygenator : MonoBehaviour
{
    private float oxygen = 100f; // 총 2분 산소 100
    public float decreaseAmount = 0.834f; // 매 초 일정량 감소 ( 100 / 120 )
    public float increaseAmount = 41.7f; // 연료 주입 시 50초의 산소 양만큼 증가

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(Decrease), 1.0f, 1.0f);
    }

    // Update is called once per frame
    private void Decrease()
    {
        oxygen -= decreaseAmount;
    }

    public void Increase()
    {
        oxygen += increaseAmount;

        if (oxygen > 100)
        {
            oxygen = 100;
        }
    }
}
