using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiLandingPage : MonoBehaviour
{
    private Controller controller;

    // 랜딩페이지의 버튼들
    private GameObject LandingPageCanvas;
    private TMP_InputField nicknameInputField;
    private TMP_InputField roomNumberInputField;
    private string userNicknameInput;
    private string roomNameInput;

    private Button nicknameBtn;
    private Button createRoomBtn;
    private Button enterRoomBtn;
    private Button QuitGameBtn;
    private TMP_InputField enterRoomInput;

    // Start is called before the first frame update
    void Start()
    {
        GameObject socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();                   // 컨트롤러 연결하기

        LandingPageCanvas = GameObject.Find("LandingPageCanvas");   // 캔버스 찾기


        nicknameBtn = LandingPageCanvas.transform.Find("NickNameBtn").GetComponent<Button>();
        nicknameBtn.onClick.AddListener(inputNickName);

        createRoomBtn = LandingPageCanvas.transform.Find("CreateRoomBtn").GetComponent<Button>();    // 방 생성
        createRoomBtn.onClick.AddListener(OnCreateRoom);            

        enterRoomBtn = LandingPageCanvas.transform.Find("EnterRoomBtn").GetComponent<Button>();      // 방 입장
        enterRoomBtn.onClick.AddListener(OnEnterRoom);

        //enterRoomInput = LandingPageCanvas.transform.Find("EnterRoomInput").GetComponent<TMP_InputField>();   // 방입장 코드

        nicknameInputField = GameObject.Find("NicknameInput").GetComponent<TMP_InputField>();
        roomNumberInputField = GameObject.Find("EnterRoomInput").GetComponent<TMP_InputField>();

        QuitGameBtn = LandingPageCanvas.transform.Find("QuitGameBtn").GetComponent<Button>();        // 방 입장
        QuitGameBtn.onClick.AddListener(OnApplicationQuit);
    }
    
    void inputNickName()
    {
        //if (nicknameInputField != null)
        //{
            
        Debug.Log("눌림");
        
        userNicknameInput = nicknameInputField.text;
        

        //}
    }

    
   
    void OnCreateRoom()
    {
        controller.Send(PacketType.CREATE_ROOM, userNicknameInput, "더미값");
        PlayerPrefs.SetString("userNickname", userNicknameInput);
        //SceneManager.LoadScene("WaitingRoom");
    }

    void OnEnterRoom()
    {
        if (roomNumberInputField.text != null && userNicknameInput != null)
        {
            roomNameInput = roomNumberInputField.text;
            Debug.Log("유저 닉네임:" + userNicknameInput + "님이");
            Debug.Log("방 코드" + roomNameInput + "방으로 진입");

            PlayerPrefs.SetString("userNickname", userNicknameInput);
            PlayerPrefs.SetString("roomCode", roomNameInput);

            controller.Send(PacketType.PARTICIPATE_USER, userNicknameInput, roomNameInput);
            Debug.Log("방 진입 완료");
            SceneManager.LoadScene("WaitingRoom");
            //Debug.Log("OnEnterRoom: " + enterRoomInput.text);
        }
    }

    void OnApplicationQuit()
    {
        Application.Quit();
    }
}
