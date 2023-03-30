using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiLandingPage : MonoBehaviour
{
    private Controller controller;

    // 랜딩페이지의 버튼들
    private GameObject LandingPageCanvas;

    private Button createRoomBtn;
    private Button enterRoomBtn;
    private Button QuitGameBtn;
    private TMP_InputField enterRoomInput;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();                   // 컨트롤러 연결하기

        LandingPageCanvas = GameObject.Find("LandingPageCanvas");   // 캔버스 찾기

        createRoomBtn = LandingPageCanvas.transform.Find("CreateRoomBtn").GetComponent<Button>();    // 방 생성
        createRoomBtn.onClick.AddListener(OnCreateRoom);            

        enterRoomBtn = LandingPageCanvas.transform.Find("EnterRoomBtn").GetComponent<Button>();      // 방 입장
        enterRoomBtn.onClick.AddListener(OnEnterRoom);

        enterRoomInput = LandingPageCanvas.transform.Find("EnterRoomInput").GetComponent<TMP_InputField>();   // 방입장 코드

        QuitGameBtn = LandingPageCanvas.transform.Find("QuitGameBtn").GetComponent<Button>();        // 방 입장
        QuitGameBtn.onClick.AddListener(OnApplicationQuit);
    }

    void OnCreateRoom()
    {
        Debug.Log("OnCreateRoom");
        controller.Send(PacketType.CREATE_ROOM, "111", "222");
    }

    void OnEnterRoom()
    {
        Debug.Log("OnEnterRoom: " + enterRoomInput.text);
    }

    void OnApplicationQuit()
    {
        Application.Quit();
    }
}
