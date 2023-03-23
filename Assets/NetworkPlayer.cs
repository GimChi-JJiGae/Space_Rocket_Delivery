using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public bool isHost = false;
    public int playerIndex;                             // 사용자의 플레이어 인덱스

    private GameObject playersObject;                   // players 오브젝트를 찾아서 그 폴더 안에 넣어주기 위함
    private GameObject[] players = new GameObject[5];   // 참조를 쉽게 하기 위해 오브젝트 저장

    GameObject mainCamera;                              // 메인 카메라를 연동하기 위함


    void Start()
    {
        playersObject = GameObject.Find("Players");                 // 오브젝트 연동
        mainCamera = GameObject.FindWithTag("MainCamera");          // 카메라 연동

        int[] nums = { 0, 1, 2, 3 };                                // 멀티플레이어 생성
        CreatePlayers(nums);

        AssignPlayer(1);                                            // 플레이어 지정
    }

    void FixedUpdate()
    {
        Vector3 a = players[playerIndex].transform.position;
        GameObject.Find("Server").GetComponent<SocketClient>().MovementSend(a.x, a.y, a.z);
    }
    public void MoveOtherPlayer(int idx, float px, float py, float pz)//, float rx, float ry, float rz, float rw)
    {
        Vector3 v = new Vector3(px, py, pz);
        Quaternion q = new Quaternion();
        //q.Set(rx, ry, rz, rw);
        players[idx].transform.position = v;
        //players[idx].transform.rotation = q;
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

    // 플레이어들 생성
    void CreatePlayers(int[] playerIdxList)
    {
        foreach (int n in playerIdxList)
        {
            CreatePlayer(n);
        }
    }

    // 플레이어 생성
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
        player.transform.parent = playersObject.transform;                // 모듈 위치를 Spaceship아래로 내려주기
        player.name = "Player" + n;               // 모듈 이름 변경

        // 플레이어 할당
        players[n] = player;
    }
}
