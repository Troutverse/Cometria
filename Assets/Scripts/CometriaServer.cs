using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class CometriaServer
{
    private TcpListener tcpListener;
    private Thread listenThread;
    private List<TcpClient> connectedClients = new List<TcpClient>();
    private bool isRunning = false;

    public void StartServer(int port)
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            isRunning = true;
            UnityEngine.Debug.Log("서버가 " + port + " 포트에서 시작되었습니다.");

            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
    }

    private void ListenForClients()
    {
        while (isRunning)
        {
            if (tcpListener.Pending())
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                connectedClients.Add(client);
                UnityEngine.Debug.Log("새 클라이언트가 연결되었습니다.");

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
            Thread.Sleep(100);
        }
    }
    private void HandleClientComm(object clientObj)
    {
        // 연결된 클라이언트 객체를 가져옵니다.
        TcpClient client = (TcpClient)clientObj;

        // 여기에 클라이언트로부터 데이터를 받는 로직을 작성할 것입니다.
        // 예를 들어, NetworkStream을 사용하여 데이터를 읽습니다.
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (client.Connected)
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        // 받은 바이트 배열을 다시 문자열로 변환합니다.
                        string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        UnityEngine.Debug.Log("서버가 클라이언트로부터 메시지를 받음: " + receivedMessage);
                    }
                }
                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("클라이언트와의 통신 중 오류 발생: " + e.Message);
        }
        finally
        {
            // 클라이언트 연결이 끊어지면 정리합니다.
            client.Close();
            connectedClients.Remove(client);
            UnityEngine.Debug.Log("클라이언트 연결이 종료되었습니다.");
        }
    }
    private void BroadcastMessage(string message, TcpClient sender)
    {
        // 메시지를 바이트 배열로 변환합니다.
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);

        // 연결된 모든 클라이언트에게 메시지를 보냅니다.
        foreach (TcpClient c in connectedClients)
        {
            if (c != sender && c.Connected)
            {
                NetworkStream stream = c.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
