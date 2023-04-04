//using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
//using UnityEditor.VersionControl;
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
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.30.116"), 5555); // 서버주소, 포트번호

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
    public void Send(byte[] byteArray)         
    {
        Debug.Log("보낸다: " + byteArray[0]);
        // 전송 시작
        socket.BeginSend(byteArray, 0, byteArray.Length, SocketFlags.None, SendCallback, null);
    }

    // 역직렬화
    private void Receive(byte[] buffer)
    {
        Debug.Log("여기서 한번만 받나?");
        byte[] header = SplitArray(buffer, 0, 1);
        //Debug.Log((int)header[0]);
        byte[] data = SplitArray(buffer, 1, buffer.Length - 1);
        controller.Receive((PacketType)header[0], data);
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

