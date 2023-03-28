using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // 포지션 변경을 위한 변수
    PlayerPositionController playerPositionController;

    // Player관련 함수
    Multiplayer multiplayer;

    void Start()
    {
        // 필요한 컨트롤러를 정의한다.
        playerPositionController = new PlayerPositionController();

        // 멀티플레이 관련 로직 
        multiplayer = GetComponent<Multiplayer>();
        multiplayer = gameObject.GetComponent<Multiplayer>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPositionController.Service(multiplayer);
    }

    public void Receive(int header, byte[] data)
    {
        try
        {
            switch (header)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 100:
                    playerPositionController.RecieveDTO(data);
                    playerPositionController.SetAct(true);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}

// 유저 움직임
public class PlayerPositionController : ReceiveController
{
    public int userId;
    public double px;
    public double py;
    public double pz;

    public void Service(Multiplayer multiplayer) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            multiplayer.MoveOtherPlayer(userId, (float)px, (float)py, (float)pz);
            this.SetAct(false);
        }
    }
}

// 모듈 컨트롤러
public class ModuleController : ReceiveController
{
    public int idxX;           // 위치
    public int idxZ;

    public int moduleType;     // 모듈 타입

    public bool wallTop ;      // 벽 모듈
    public bool wallLeft;
    public bool wallBottom;
    public bool wallRight;

    public float hp;           // 체력

    public void Service(Module module) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            //multiplayer.MoveOtherPlayer(userId, (float)px, (float)py, (float)pz);
            this.SetAct(false);
        }
    }
}

// 컨트롤러 정의
public class ReceiveController
{
    private bool isAct = false;     // 활성화 되어있으면 실행시킨다.

    public void RecieveDTO(byte[] data) // 데이터를 받으면 역직렬화 후 Class에 맞는 데이터로 변형시킨다.
    {
        Type typeClass = this.GetType();
        FieldInfo[] fields = typeClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // 이 클래스를 참조하여 필요한 필드를 찾는다.

        int n = 0;
        Debug.Log(fields.Length);
        foreach (FieldInfo field in fields) // 필드별로 돌며 역직렬화 한다.
        {

            Type type = field.FieldType;

            if (type.Equals(typeof(int)))
            {
                byte[] result = new byte[sizeof(int)];
                Array.Copy(data, n, result, 0, sizeof(int));
                field.SetValue(this, BitConverter.ToInt32(result));
                n += sizeof(int);
            }
            else if (type.Equals(typeof(float)))
            {
                byte[] result = new byte[sizeof(float)];
                Array.Copy(data, n, result, 0, sizeof(float));
                field.SetValue(this, BitConverter.ToSingle(result));
                n += sizeof(float);
            }
            else if (type.Equals(typeof(double)))
            {
                byte[] result = new byte[sizeof(double)];
                Array.Copy(data, n, result, 0, sizeof(double));
                field.SetValue(this, BitConverter.ToDouble(result));
                n += sizeof(double);
            }
        }
    }

    public bool GetAct()
    {
        return isAct;
    }

    public void SetAct(bool b)
    {
        isAct = b;
    }

    public void Service() { }
}