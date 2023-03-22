using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    private Socket socket;
    private byte[] buffer = new byte[1024];

    private void Start()
    {
        // 서버 주소와 포트번호 설정
        string serverAddress = "192.168.30.116";
        int serverPort = 5555;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);

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
            // 수신한 데이터 처리
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received message: " + message);

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

    public void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, null);
    }

    private void SendCallback(IAsyncResult result)
    {
        socket.EndSend(result);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Disconnected from server22");
        // 앱이 종료될 때 소켓 연결 해제
        if (socket != null)
        {
            socket.Close();
        }
    }
}