using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;

public class MultiEnemy : MonoBehaviour
{
    Controller controller;
    Multiplayer multiplayer; // 멀티플레이인지 확인하는 변수

    public GameObject[] enemyeList = new GameObject[10000];
    Vector3[] targetPosition = new Vector3[10000];
    Quaternion[] targetRotation = new Quaternion[10000];
    public int enemyCount = 0;

    

    void Start()
    {
        multiplayer = GetComponent<Multiplayer>();
        controller = GetComponent<Controller>();
        StartCoroutine(SendPositionEnemy());
    }

    void FixedUpdate()
    {
        if (multiplayer.isHost == false)
        {
            for (int i = 0; i < enemyeList.Length; i++)
            {
                if (enemyeList[i] != null)
                {
                    Vector3 v = (targetPosition[i] - enemyeList[i].transform.position) * 5.0f * Time.deltaTime;
                    enemyeList[i].transform.position += v;
                    enemyeList[i].transform.rotation = Quaternion.Lerp(targetRotation[i], enemyeList[i].transform.rotation, 0.1f * Time.deltaTime);
                }
            }
        }
    }


    IEnumerator SendPositionEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            if (multiplayer.isMultiplayer && multiplayer.isHost == true)
            {
            List<object> list = new List<object>();
            list.Add(enemyCount);
            for (int i = 0; i < enemyCount; i++)
            {
                if (enemyeList[i] != null)
                {
                    list.Add(i);
                    list.Add(0);
                    Vector3 a = enemyeList[i].transform.position;
                    list.Add(a.x);
                    list.Add(a.y);
                    list.Add(a.z);
                    Quaternion q = enemyeList[i].transform.rotation;
                    list.Add(q.x);
                    list.Add(q.y);
                    list.Add(q.z);
                    list.Add(q.w);
                }
            }
            controller.ListSend(PacketType.ENEMY_MOVE, list);

            }
           
        }

        
    }

}
