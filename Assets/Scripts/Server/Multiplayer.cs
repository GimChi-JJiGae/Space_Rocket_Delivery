using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
public class Multiplayer : MonoBehaviour
{
    public bool isHost = false;
    public int playerIndex;                             // 사용자의 플레이어 인덱스

    private GameObject playersObject;                   // players 오브젝트를 찾아서 그 폴더 안에 넣어주기 위함
    public GameObject[] players = new GameObject[5];   // 참조를 쉽게 하기 위해 오브젝트 저장

    GameObject mainCamera;                              // 메인 카메라를 연동하기 위함


    void Start()
    {
        playersObject = GameObject.Find("Players");                 // 오브젝트 연동
        mainCamera = GameObject.FindWithTag("MainCamera");          // 카메라 연동

        int[] nums = { 0, 1, 2, 3 };                                // 멀티플레이어 생성

        AssignPlayer(1);                                            // 플레이어 지정
    }

    void FixedUpdate()
    {
        Vector3 a = players[playerIndex].transform.position;
        double x = a.x;
        double y = a.y;
        double z = a.z;
        GameObject.Find("Server").GetComponent<SocketClient>().Send("character", playerIndex, x, y, z);
    }

    public void MoveOtherPlayer(int idx, float px, float py, float pz)
    {
        idx = 3;
        if (idx != playerIndex)
        {
            players[idx].transform.position = new Vector3(px, py, pz);

        }
    }


    // 생성된 플레이어들 중 n에 할당
    void AssignPlayer(int n)
    {
        playerIndex = n;
        GameObject player = players[n];

        // 캐릭터 모듈 연결
        player.AddComponent<PlayerInput>();
        player.AddComponent<PlayerInteraction>();
        player.AddComponent<PlayerMovement>();
        player.AddComponent<PlayerServer>();

        // 카메라 연동
        mainCamera.GetComponent<MainCamera>().SetTarget(player);
    }
}
