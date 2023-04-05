using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiEnemy : MonoBehaviour
{
    Controller controller;
    Multiplayer multiplayer; // 멀티플레이인지 확인하는 변수

    public GameObject[] enemyeList = new GameObject[10000];
    Vector3[] targetPosition = new Vector3[10000];
    Quaternion[] targetRotation = new Quaternion[10000];
    public int enemyCount = 0;

    public GameObject[] enemies; // 프리팹을 저장해둘 공간

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
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            try
            {
                if (multiplayer.isMultiplayer && multiplayer.isHost == true)
                {
                    List<object> list = new List<object>();

                    int count = 0;
                    for (int i = 0; i < enemyCount; i++)
                    {
                        if (enemyeList[i] != null)
                        {
                            count++;
                            list.Add((int)i);
                            list.Add((int)0); // 타입을 여기 저장시킬 것임
                            Vector3 a = enemyeList[i].transform.position;
                            list.Add((float)a.x);
                            list.Add((float)a.y);
                            list.Add((float)a.z);
                            Quaternion q = enemyeList[i].transform.rotation;
                            list.Add((float)q.x);
                            list.Add((float)q.y);
                            list.Add((float)q.z);
                            list.Add((float)q.w);
                        }
                    }
                    List<object> sendList = new List<object>();
                    sendList.Add((int)count);
                    sendList.AddRange(list);
                    //controller.ListSend(PacketType.ENEMY_MOVE, sendList);
                }
            }
            catch
            {

            }
            
        }
    }

    public void ReceiveMoveEnemy(DTOenemymove[] DTOenemymove)
    {

        Debug.Log("받았다 이것을");
        try
        {
            for (int i = 0; i < DTOenemymove.Length; i++)
            {
                if (enemyeList[DTOenemymove[i].idxE] == null)   // null 개체면
                {
                    spawnEnemy(DTOenemymove[i]);
                    Vector3 v = new Vector3(DTOenemymove[i].px, DTOenemymove[i].py, DTOenemymove[i].pz);
                    Quaternion q = new Quaternion(DTOenemymove[i].rx, DTOenemymove[i].ry, DTOenemymove[i].rz, DTOenemymove[i].rw);
                    targetPosition[DTOenemymove[i].idxE] = v;
                    targetRotation[DTOenemymove[i].idxE] = q;
                }
                else                                            // 존재하면
                {
                    Vector3 v = new Vector3(DTOenemymove[i].px, DTOenemymove[i].py, DTOenemymove[i].pz);
                    Quaternion q = new Quaternion(DTOenemymove[i].rx, DTOenemymove[i].ry, DTOenemymove[i].rz, DTOenemymove[i].rw);
                    targetPosition[DTOenemymove[i].idxE] = v;
                    targetRotation[DTOenemymove[i].idxE] = q;
                }
            }
        }
        catch
        {
            Debug.Log("에러인가..");
        }
    }

    public void spawnEnemy(DTOenemymove DTOenemymove)
    {
        /*
        GameObject[] currentEnemies;
        int[] currentEnemyHealths;
        
        if (difficultyLevel == 0)
        {
            currentEnemies = enemies;
        }
        else if (difficultyLevel == 1)
        {
            currentEnemies = enemiesTier2;
        }
        else
        {
            currentEnemies = enemiesTier3;
        }

        if (currentEnemies.Length == 0)
        {
            Debug.LogError("currentEnemies 배열이 비어있습니다.");
            return;
        }
        */
        GameObject enemy;
        /* 벽 찾기
        GameObject closestWall = FindClosestWall();
        if (closestWall == null)
        {
            Debug.LogError("가장 가까운 벽을 찾을 수 없습니다.");
            return;
        }*/

        Vector3 v = new Vector3(DTOenemymove.px, DTOenemymove.py, DTOenemymove.pz);
        Quaternion q = new Quaternion(DTOenemymove.rx, DTOenemymove.ry, DTOenemymove.rz, DTOenemymove.rw);
        enemy = Instantiate(enemies[0], v, q);
        enemy.name = "적이다구리";
        /*
        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            RangedEnemyController rangedController = enemy.GetComponent<RangedEnemyController>();
            rangedController.target = closestWall;
        }
        else
        {
            controller.spawner = this; // spawner를 설정해주세요.
            controller.target = closestWall;
            controller.enemyDestroyedSound = enemyDestroyedSound;
        }
        */

        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            collider.size = new Vector3(0.7f, 0.7f, 0.7f); // 원거리 적의 경우 적절한 크기로 조정
        }
        else
        {
            collider.size = new Vector3(0.5f, 0.5f, 0.5f); // 근거리 적의 경우 적절한 크기로 조정
        }

        // 중력을 rigidboy로 받는다.
        /*
        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        
        Vector3 direction = (closestWall.transform.position - enemy.transform.position).normalized;
        Vector3 velocity = direction * speed;

        rb.velocity = velocity;
        rb.freezeRotation = true;

        enemy.transform.rotation = Quaternion.LookRotation(direction);
        */
        enemyeList[DTOenemymove.idxE] = enemy;
    }
}
