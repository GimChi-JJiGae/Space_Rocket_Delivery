using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MutiplayWaitingRoom : MonoBehaviour
{
    private Controller controller;

    private Button GoBtn;
    private Button QuitRoomBtn;
    private TMP_Text RoomCode;
    private bool alreadySend;
    public TMP_Text UserList;
    public List<string> userStringList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        alreadySend = false;
        controller = GetComponent<Controller>();                   // 컨트롤러 연결하기

        RoomCode = GameObject.Find("RoomCodeText").GetComponent<TMP_Text>();
        UserList = GameObject.Find("UserList").GetComponent<TMP_Text>();

        GoBtn = GameObject.Find("GoBtn").GetComponent<Button>();    // 게임 시작
        GoBtn.onClick.AddListener(StartNewGame);

        QuitRoomBtn = GameObject.Find("QuitRoomBtn").GetComponent<Button>();      // 방 나가기
        QuitRoomBtn.onClick.AddListener(QuitRoom);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        UserList.text = "";
       for (int i = 0; i < userStringList.Count; i++)
        {
            UserList.text = UserList.text + " " + userStringList[i];
        }
        RoomCode.text = "RoomCode: " + PlayerPrefs.GetString("roomCode"); // 시작한 이후에 업데이트 해줘야 이전 방이 안뜸
    }

    void StartNewGame()
    {
        if (!alreadySend)
        {
            //        controller.Send(PacketType.게임시작, PlayerPrefs.GetString("roomCode");
            controller.Send(PacketType.START_GAME, PlayerPrefs.GetString("roomCode"));
            //        SceneManager.LoadScene("Multiplay");
            alreadySend = true;
        }
    }

    void QuitRoom()
    {
        Debug.Log(controller);
        controller.Send(PacketType.OUT_USER);
        SceneManager.LoadScene("LandingPage");

        //controller.Send(PacketType.DEPARTURE_USER, PlayerPrefs.GetString("roomCode"), PlayerPrefs.GetInt("userId"));

        //        SceneManager.LoadScene("LandingPage");
    }
}
