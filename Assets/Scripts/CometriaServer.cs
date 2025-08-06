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
            UnityEngine.Debug.Log("������ " + port + " ��Ʈ���� ���۵Ǿ����ϴ�.");

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
                UnityEngine.Debug.Log("�� Ŭ���̾�Ʈ�� ����Ǿ����ϴ�.");

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
            Thread.Sleep(100);
        }
    }
    private void HandleClientComm(object clientObj)
    {
        // ����� Ŭ���̾�Ʈ ��ü�� �����ɴϴ�.
        TcpClient client = (TcpClient)clientObj;

        // ���⿡ Ŭ���̾�Ʈ�κ��� �����͸� �޴� ������ �ۼ��� ���Դϴ�.
        // ���� ���, NetworkStream�� ����Ͽ� �����͸� �н��ϴ�.
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
                        // ���� ����Ʈ �迭�� �ٽ� ���ڿ��� ��ȯ�մϴ�.
                        string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        UnityEngine.Debug.Log("������ Ŭ���̾�Ʈ�κ��� �޽����� ����: " + receivedMessage);
                    }
                }
                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Ŭ���̾�Ʈ���� ��� �� ���� �߻�: " + e.Message);
        }
        finally
        {
            // Ŭ���̾�Ʈ ������ �������� �����մϴ�.
            client.Close();
            connectedClients.Remove(client);
            UnityEngine.Debug.Log("Ŭ���̾�Ʈ ������ ����Ǿ����ϴ�.");
        }
    }
    private void BroadcastMessage(string message, TcpClient sender)
    {
        // �޽����� ����Ʈ �迭�� ��ȯ�մϴ�.
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);

        // ����� ��� Ŭ���̾�Ʈ���� �޽����� �����ϴ�.
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
