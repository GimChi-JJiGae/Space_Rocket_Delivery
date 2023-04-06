using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretSpinController : MonoBehaviour
{
    public GameObject socketObj;
    public Controller controller;

    public GameObject TurretShootingHead;
    public float degree = 15f;
    // Start is called before the first frame update
    void Start()
    {
        socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();                   // 컨트롤러 연결하기


        StartCoroutine(CallFunctionRepeatedly());
    }


    IEnumerator CallFunctionRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            Quaternion currentSpin = transform.rotation;
            Quaternion verticalSpin = TurretShootingHead.transform.rotation;
            Debug.Log("포탑송신");
            controller.Send(PacketType.MOVE, controller.roomCode, controller.userId, currentSpin.x, currentSpin.y, currentSpin.z, currentSpin.w, verticalSpin.x, verticalSpin.y, verticalSpin.z, verticalSpin.w);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 수평 입력 값

        if(socketObj == null)
        {
            socketObj = GameObject.Find("SocketClient");
        }
        if (controller == null)
        {
            controller = socketObj.GetComponent<Controller>();
        }


        if (horizontal != 0)
        {
            Vector3 rotationDirection = new Vector3(0, horizontal, 0f);
            transform.Rotate(rotationDirection * degree * Time.deltaTime);
        }
        
    }
}
