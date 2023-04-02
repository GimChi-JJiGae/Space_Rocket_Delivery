using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MutiplayWaitingRoom : MonoBehaviour
{
    private Controller controller;

    private Button GoBtn;
    private Button QuitRoomBtn;
    private Text RoomCode;
    
    // Start is called before the first frame update
    void Start()
    {
        RoomCode = GameObject.Find("RoomCodeText").GetComponent<Text>();
        RoomCode.text = "RoomCode:" + PlayerPrefs.GetString("roomCode");

        GoBtn = GameObject.Find("GoBtn").GetComponent<Button>();    // 게임 시작
        GoBtn.onClick.AddListener(StartGame);

        QuitRoomBtn = GameObject.Find("QuitRoomBtn").GetComponent<Button>();      // 방 나가기
        QuitRoomBtn.onClick.AddListener(QuitRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
//        controller.Send(PacketType.게임시작, PlayerPrefs.GetString("roomCode");
        
//        SceneManager.LoadScene("Multiplay");
    }

    void QuitRoom()
    {
                //controller.Send(PacketType.DEPARTURE_USER, PlayerPrefs.GetString("roomCode"), PlayerPrefs.GetInt("userId"));

        //        SceneManager.LoadScene("LandingPage");
    }
}
