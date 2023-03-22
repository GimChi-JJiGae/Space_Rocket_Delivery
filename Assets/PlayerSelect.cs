using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public bool isHost = false;
    public int playerIndex;

    private GameObject[] players = new GameObject[5];

    GameObject mainCamera;


    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        int[] nums = { 0, 1, 2, 3 };
        CreatePlayers(nums);
        AssignPlayer(1);
    }

    void Update()
    {
        
    }

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
        players[n] = player;
    }

    void CreatePlayers(int[] playerIdxList)
    {
        foreach (int n in playerIdxList)
        {
            CreatePlayer(n);
        }
    }

    void CreatePlayer(int n)
    {
        // 캐릭터 모듈 연결
        GameObject playerPrefab;
        playerPrefab = Resources.Load<GameObject>("Character/PlayerSchema");

        // 위치지정
        float positionX = 0;
        float positionZ = 0;
        float positionY = 0;
        Vector3 position = new Vector3(positionX + n, positionY, positionZ);    // 바닥 타일의 위치
        Quaternion rotation = Quaternion.identity;                          // 바닥 타일의 회전

        // 모듈 생성하고 space의 배열에 할당시키기
        GameObject player = Instantiate(playerPrefab, position, rotation);
        player.transform.parent = transform;                // 모듈 위치를 Spaceship아래로 내려주기
        player.name = "Player" + n;               // 모듈 이름 변경

        // 플레이어 할당
        players[n] = player;
    }
}
