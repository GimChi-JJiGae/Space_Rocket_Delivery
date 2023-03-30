using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour
{
    public bool isHost = false;

    public int playerIndex;                            // 사용자의 플레이어 인덱스

    public GameObject[] players = new GameObject[4];   // 참조를 쉽게 하기 위해 오브젝트 저장

    GameObject mainCamera;                             // 메인 카메라를 연동하기 위함
    private Controller controller;

    Vector3[] targetPosition = new Vector3[4];
    Quaternion[] targetRotation = new Quaternion[4];

    void Start()
    {
        controller  = GetComponent<Controller>();                   // 컨트롤러 연결하기

        mainCamera = GameObject.FindWithTag("MainCamera");          // 카메라 연동

        AssignPlayer(0);

        StartCoroutine(CallFunctionRepeatedly());
    }

    IEnumerator CallFunctionRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            Vector3 a = players[playerIndex].transform.position;
            Quaternion b = players[playerIndex].transform.rotation;
            
            controller.Send(PacketType.MOVE, playerIndex, a.x + 1, a.y, a.z + 1, b.x, b.y, b.z, b.w);
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < players.Length - 1; i++)
        {
            if (targetPosition[i] != null && i != playerIndex)
            {
                Vector3 v = (targetPosition[i] - players[i].transform.position) * 5.0f * Time.deltaTime;
                players[i].transform.position += v;
                players[i].transform.rotation = Quaternion.Lerp(targetRotation[i], players[i].transform.rotation, 0.1f * Time.deltaTime);
                
                players[i].GetComponent<Animator>().SetFloat("Move_GoBack", v.x * 20.0f);
                players[i].GetComponent<Animator>().SetFloat("Move_LeftRight", v.z * 20.0f);
            }
        }
    }

    public void MoveOtherPlayer(int idx, float px, float py, float pz, float rx, float ry, float rz, float rw)
    {
        idx = 3;
        if (idx != playerIndex)
        {
            Vector3 dir = new Vector3(px, py, pz);
            Quaternion q = new Quaternion(rx, ry, rz, rw);
            targetRotation[idx] = q;
            targetPosition[idx] = dir;
        }
    }

    // 생성된 플레이어들 중 n에 할당
    void AssignPlayer(int n)
    {
        playerIndex = n;
        GameObject player = players[n];

        // 캐릭터 모듈 연결
        player.AddComponent<PlayerInput>();
        player.AddComponent<PlayerMovement>();
        player.AddComponent<InteractionModule>();
        player.AddComponent<InteractionObject>();

        // 카메라 연동
        mainCamera.GetComponent<MainCamera>().SetTarget(player);
    }


}
