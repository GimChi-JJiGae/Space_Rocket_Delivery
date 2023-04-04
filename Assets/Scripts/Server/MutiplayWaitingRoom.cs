using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("여기 열리니?");
        Debug.Log(PlayerPrefs.GetString("roomCode"));
        RoomCode = GameObject.Find("RoomCodeText").GetComponent<TMP_Text>();
        

        GoBtn = GameObject.Find("GoBtn").GetComponent<Button>();    // 게임 시작
        GoBtn.onClick.AddListener(StartGame);

        QuitRoomBtn = GameObject.Find("QuitRoomBtn").GetComponent<Button>();      // 방 나가기
        QuitRoomBtn.onClick.AddListener(QuitRoom);
    }

    // Update is called once per frame
    void Update()
    {
       
        RoomCode.text = "RoomCode: " + PlayerPrefs.GetString("roomCode"); // 시작한 이후에 업데이트 해줘야 이전 방이 안뜸
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
