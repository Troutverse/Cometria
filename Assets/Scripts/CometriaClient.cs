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
            UnityEngine.Debug.Log("������ ����Ǿ����ϴ�.");

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
            UnityEngine.Debug.LogError("������ ������� �ʾ� �޽����� ���� �� �����ϴ�.");
            return;
        }

        try
        {
            NetworkStream stream = client.GetStream();
            // ���ڿ��� ����Ʈ �迭�� ��ȯ�մϴ�.
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
            // ����Ʈ �迭�� ��Ʈ��ũ ��Ʈ���� ���� �����ϴ�.
            stream.Write(buffer, 0, buffer.Length);
            UnityEngine.Debug.Log("������ �޽��� ����: " + message);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("�޽��� ���� �� ���� �߻�: " + e.Message);
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
                // [�߰��� �ڵ�] �����κ��� ���� �����Ͱ� �ִ��� Ȯ��
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        UnityEngine.Debug.Log("Ŭ���̾�Ʈ�� �����κ��� �޽����� ����: " + receivedMessage);
                    }
                }
                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("������ ���� �� ���� �߻�: " + e.Message);
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
