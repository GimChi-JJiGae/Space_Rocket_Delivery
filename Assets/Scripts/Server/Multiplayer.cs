using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Multiplayer : MonoBehaviour
{
    public bool isHost = false;
    public int playerIndex;                             // 사용자의 플레이어 인덱스

    private GameObject playersObject;                   // players 오브젝트를 찾아서 그 폴더 안에 넣어주기 위함
    public GameObject[] players = new GameObject[5];   // 참조를 쉽게 하기 위해 오브젝트 저장

    GameObject mainCamera;                              // 메인 카메라를 연동하기 위함
    private Controller controller;

    private GameObject LandingPageCanvas;

    private Button createRoomBtn;
    private Button enterRoomBtn;
    private TMP_InputField enterRoomInput;


    void Start()
    {
        controller  = GetComponent<Controller>();                   // 컨트롤러 연결하기
        LandingPageCanvas = GameObject.Find("LandingPageCanvas");   // 캔버스 찾기

        createRoomBtn = LandingPageCanvas.transform.Find("CreateRoomBtn").GetComponent<Button>();    // 방 생성
        createRoomBtn.onClick.AddListener(OnCreateRoom);            // 방 생성 이벤트 연결

        enterRoomBtn = LandingPageCanvas.transform.Find("EnterRoomBtn").GetComponent<Button>();    // 방 생성
        enterRoomBtn.onClick.AddListener(OnEnterRoom);
        enterRoomInput = LandingPageCanvas.transform.Find("EnterRoomInput").GetComponent<TMP_InputField>();   

        playersObject = GameObject.Find("Players");                 // 오브젝트 연동
        mainCamera = GameObject.FindWithTag("MainCamera");          // 카메라 연동

        int[] nums = { 0, 1, 2, 3 };                                // 멀티플레이어 생성

        AssignPlayer(1);

        StartCoroutine(CallFunctionRepeatedly());
    }

    IEnumerator CallFunctionRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            Vector3 a = players[playerIndex].transform.position;
            Quaternion b = players[playerIndex].transform.rotation;
            controller.Send(100, playerIndex, a.x, a.y, a.z, b.x, b.y, b.z, b.w);
        }
    }

    public void MoveOtherPlayer(int idx, float px, float py, float pz, float rx, float ry, float rz, float rw)
    {
        idx = 3;
        if (idx != playerIndex)
        {
            players[idx].transform.position = new Vector3(px, py, pz);
            players[idx].transform.rotation = new Quaternion(rx, ry, rz, rw);
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

        // 카메라 연동
        mainCamera.GetComponent<MainCamera>().SetTarget(player);
    }

    void OnCreateRoom()
    {
        Debug.Log("OnCreateRoom");
        controller.Send(10, "Receive OnCreateRoom");
    }

    void OnEnterRoom()
    {
        Debug.Log("OnEnterRoom: " + enterRoomInput.text);
    }
}
