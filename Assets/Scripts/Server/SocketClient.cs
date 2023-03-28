using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
//using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class SocketClient : MonoBehaviour
{
    // 소켓 연결과 직렬화 버퍼
    private Socket socket;
    private byte[] buffer = new byte[1024]; // 직렬화 버퍼

    // 로직은 컨트롤러에 위임
    Controller controller;

    private void Start()
    {
        controller = GetComponent<Controller>();

        // 서버 주소와 포트번호 설정
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.30.129"), 5555); // 서버주소, 포트번호

        // 소켓 생성 및 연결 시도
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(endPoint, ConnectCallback, null);

    }

    private void ConnectCallback(IAsyncResult result)
    {
        // 연결이 완료되면 호출됩니다.
        socket.EndConnect(result);
        Debug.Log("Connected to server");

        // 연결된 소켓으로부터 데이터 수신 대기
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        // 데이터 수신 완료
        int bytesRead = socket.EndReceive(result);
        if (bytesRead > 0)
        {
            try{
                // 수신한 데이터 역직렬화
                Receive(buffer);
            }catch { }

            // 다시 데이터 수신 대기
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        else
        {
            // 연결이 끊어졌을 때 처리
            Debug.Log("Disconnected from server");
            socket.Close();
        }
    }

    // 직렬화 후 전송
    public void Send(String header, params object[] args)          // 인자를 object배열로 받아옴
    {
        List<byte> byteList = new List<byte>();             // List를 byte로 받아옴

        // header 세팅. header를 해석하면 뒷단 정보 구조를 제공받을 수 있음
        switch (header)
        {
            case "character":
                byteList.AddRange(BitConverter.GetBytes((int)100));
                break;
            case "module":
                byteList.AddRange(BitConverter.GetBytes((int)8));
                break;
        }

        // params 직렬화
        for (int i = 0; i < args.Length; i++)               
        {
            object arg = args[i];
            Type type = arg.GetType();
            
            if (type.Equals(typeof(int)))
            {
                byteList.AddRange(BitConverter.GetBytes((int)arg));
            }
            else if (type.Equals(typeof(float)))
            {
                byteList.AddRange(BitConverter.GetBytes((float)arg));
            }
            else if (type.Equals(typeof(double)))
            {
                byteList.AddRange(BitConverter.GetBytes((double)arg));
            }
        }
        byte[] byteArray = byteList.ToArray();
        
        // 전송 시작
        socket.BeginSend(byteArray, 0, byteArray.Length, SocketFlags.None, SendCallback, null);
    }

    // 역직렬화
    private void Receive(byte[] buffer)
    {
        int header = BitConverter.ToInt32(SplitArray(buffer, 0, 4));
        byte[] data = SplitArray(buffer, 4, buffer.Length - 4);

        controller.Receive(header, data);
    }

    private void SendCallback(IAsyncResult result)
    {
        socket.EndSend(result);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Disconnected from server");
        // 앱이 종료될 때 소켓 연결 해제
        if (socket != null)
        {
            socket.Close();
        }
    }

    // Array를 나누는 함수
    public byte[] SplitArray(byte[] array, int startIndex, int length)  
    {
        byte[] result = new byte[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }

}
