using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class CometriaClient
{
    private System.Net.Sockets.TcpClient client;
    private Thread clientReceiveThread;

    public void ConnectToServer(string host, int port)
    {
        try
        {
            client = new System.Net.Sockets.TcpClient(host, port);
            UnityEngine.Debug.Log("서버에 연결되었습니다.");

            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
    }
    public void SendMessage(string message)
    {
        if (client == null || !client.Connected)
        {
            UnityEngine.Debug.LogError("서버에 연결되지 않아 메시지를 보낼 수 없습니다.");
            return;
        }

        try
        {
            NetworkStream stream = client.GetStream();
            // 문자열을 바이트 배열로 변환합니다.
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
            // 바이트 배열을 네트워크 스트림을 통해 보냅니다.
            stream.Write(buffer, 0, buffer.Length);
            UnityEngine.Debug.Log("서버로 메시지 전송: " + message);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("메시지 전송 중 오류 발생: " + e.Message);
        }
    }
    private void ListenForData()
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (client.Connected)
            {
                // [추가된 코드] 서버로부터 받은 데이터가 있는지 확인
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        UnityEngine.Debug.Log("클라이언트가 서버로부터 메시지를 받음: " + receivedMessage);
                    }
                }
                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("데이터 수신 중 오류 발생: " + e.Message);
        }
        finally
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
