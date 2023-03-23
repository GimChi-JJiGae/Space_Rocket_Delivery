using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class SocketClient : MonoBehaviour
{
    private Socket socket;
    private byte[] buffer = new byte[1024];
    NetworkPlayer NetworkPlayer;

    private void Start()
    {
        // 서버 주소와 포트번호 설정
        string serverAddress = "192.168.30.116";
        int serverPort = 5555;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);

        // 소켓 생성 및 연결 시도
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(endPoint, ConnectCallback, null);
        NetworkPlayer = gameObject.GetComponent<NetworkPlayer>();
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
            //Debug.Log("recieve Message" + message);
            
            try
            {
                DeSerialization(message);
            }
            catch { 
            }
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

    public void MovementSend(double px, double py, double pz)
    {
        byte[] data = new byte[0];
        //byte[] messageDatas = Encoding.UTF8.GetBytes(message);
        int a = 1;
        byte[] header = BitConverter.GetBytes(a);
        a = 3;
        byte[] header2 = BitConverter.GetBytes(a);
        byte[] bpx = BitConverter.GetBytes(px);
        byte[] bpy = BitConverter.GetBytes(py);
        byte[] bpz = BitConverter.GetBytes(pz);

        foreach (byte b in header)
        {
            data = data.Append(b);
        }
        foreach (byte b in header2)
        {
            data = data.Append(b);
        }
        foreach (byte b in bpx)
        {
            data = data.Append(b);
        }
        foreach (byte b in bpy)
        {
            data = data.Append(b);
        }
        foreach (byte b in bpz)
        {
            data = data.Append(b);
        }

        //Debug.Log("Byte Array is: " + String.Join(" ", data));

        socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, null);
    }

    private void DeSerialization(string data)
    {
        int head = 0;
        byte[] header1 = ByteSubstring(data, head, sizeof(int));
        head += sizeof(int);
        int d1 = BitConverter.ToInt32(header1);
        byte[] header2 = ByteSubstring(data, head, sizeof(int));
        head += sizeof(int);
        int d2 = BitConverter.ToInt32(header2);
        byte[] header3 = ByteSubstring(data, head, sizeof(double));
        head += sizeof(double);
        double d3 = BitConverter.ToDouble(header3);
        byte[] header4 = ByteSubstring(data, head, sizeof(double));
        head += sizeof(double);
        double d4 = BitConverter.ToDouble(header4);
        byte[] header5 = ByteSubstring(data, head, sizeof(double));
        double d5 = BitConverter.ToDouble(header5);

        Debug.Log("size :" + sizeof(float));
        Debug.Log("recieve: header: " + d1 + ", header: " + d2 + ", x: " + d3 + ", y: " + d4 + ", z: " + d5);
        
        NetworkPlayer.MoveOtherPlayer(3, (float)d3, (float)d4, (float)d5);
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

    public byte[] ByteSubstring(String Data, int StartIdx, int byteLength)
    {
        byte[] byteTEMP = Encoding.Default.GetBytes(Data, StartIdx, byteLength);

        return byteTEMP;
    }
}

public static class Extensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        if (array == null)
        {
            return new T[] { item };
        }
        Array.Resize(ref array, array.Length + 1);
        array[array.Length - 1] = item;

        return array;
    }
}
