
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerClient
{
    public int ConnectionId;
    public TcpClient Socket;
    public NetworkStream Stream;
    private byte[] ReceiveBuffer;
    public Server MainServer;
    private StringBuilder ReceiveMsgBuffer = new StringBuilder();

    public ServerClient(TcpClient socket, int connectionId, Server server)
    {
        this.Socket = socket;
        this.ConnectionId = connectionId;
        this.MainServer = server;
        this.Socket.NoDelay = true;
        this.Stream = socket.GetStream();
        ReceiveBuffer = new byte[4096];
        Stream.BeginRead(ReceiveBuffer, 0, ReceiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int ByteLength = Stream.EndRead(ar);
            if (ByteLength <= 0)
            {
                MainServer.DisconnectClient(ConnectionId);
                return;
            }

            byte[] Data = new byte[ByteLength];
            Array.Copy(ReceiveBuffer, Data, ByteLength);
            string Message = Encoding.UTF8.GetString(Data);
            ReceiveMsgBuffer.Append(Message);

            while (true)
            {
                string FullMessage = ReceiveMsgBuffer.ToString();
                int NewlineIndex = FullMessage.IndexOf('\n');
                if (NewlineIndex == -1)
                {
                    break;
                }

                string MessageToProcess = FullMessage.Substring(0, NewlineIndex);
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    string[] Parts = MessageToProcess.Split('|');
                    if (Parts.Length > 0)
                    {
                        if (Parts[0] == "MOVE")
                        {
                            string RebuiltMsg = $"MOVE|{this.ConnectionId}|{Parts[2]}|{Parts[3]}|{Parts[4]}";
                            MainServer.Broadcast(RebuiltMsg, this.ConnectionId);
                        }
                        else
                        {
                            MainServer.Broadcast(MessageToProcess, this.ConnectionId);
                        }
                    }
                });

                ReceiveMsgBuffer.Remove(0, NewlineIndex + 1);
            }

            Stream.BeginRead(ReceiveBuffer, 0, ReceiveBuffer.Length, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving TCP data: {e}");
            MainServer.DisconnectClient(ConnectionId);
        }
    }
}


public class Server : MonoBehaviour
{
    private TcpListener TcpListeners;
    private Thread ListenerThread;
    public int Port = 7777;

    public Dictionary<int, ServerClient> Clients = new Dictionary<int, ServerClient>();
    private int NextConnectionId = 1;

    public static int MyConnectionId = 0;

    public void StartServer()
    {
        if (TcpListeners != null) return;

        try
        {
            TcpListeners = new TcpListener(IPAddress.Any, Port);
            TcpListeners.Start();

            ListenerThread = new Thread(new ThreadStart(ListenForClients));
            ListenerThread.IsBackground = true;
            ListenerThread.Start();

            Debug.Log($"Server started on port {Port}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Socket error: {e}");
        }
    }

    private void ListenForClients()
    {
        while (true)
        {
            TcpClient Client = TcpListeners.AcceptTcpClient();
            Debug.Log($"Client connected from {Client.Client.RemoteEndPoint}");

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                int newConnectionId = NextConnectionId++;
                ServerClient newClient = new ServerClient(Client, newConnectionId, this);

                Broadcast($"SPAWN|{newConnectionId}", 0);

                string IdMsg = $"ASSIGN_ID|{newConnectionId}";
                byte[] IdData = Encoding.UTF8.GetBytes(IdMsg);
                newClient.Stream.Write(IdData, 0, IdData.Length);

                foreach (var otherClient in Clients.Values)
                {
                    string spawnMsg = $"SPAWN|{otherClient.ConnectionId}";
                    byte[] spawnData = Encoding.UTF8.GetBytes(spawnMsg);
                    newClient.Stream.Write(spawnData, 0, spawnData.Length);
                }

                Clients.Add(newConnectionId, newClient);
            });
        }
    }

    public void Broadcast(string msg, int excludeConnectionId)
    {
        byte[] Data = Encoding.UTF8.GetBytes(msg);
        foreach (var client in Clients.Values)
        {
            if (client.ConnectionId != excludeConnectionId)
            {
                try
                {
                    client.Stream.Write(Data, 0, Data.Length);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error sending data to client {client.ConnectionId}: {e}");
                }
            }
        }
    }

    public void DisconnectClient(int connectionId)
    {
        if (Clients.ContainsKey(connectionId))
        {
            Clients[connectionId].Socket.Close();
            Clients.Remove(connectionId);
            Debug.Log($"Client {connectionId} disconnected.");

            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                Broadcast($"DISCONNECT|{connectionId}", 0);
            });
        }
    }

    private void OnApplicationQuit()
    {
        if (TcpListeners != null)
        {
            TcpListeners.Stop();
        }
        foreach (var client in Clients.Values)
        {
            client.Socket.Close();
        }
        if (ListenerThread != null)
        {
            ListenerThread.Abort();
        }
    }
}

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
    private static UnityMainThreadDispatcher Instances = null;

    public static UnityMainThreadDispatcher Instance()
    {
        if (Instances == null)
        {
            GameObject obj = new GameObject("UnityMainThreadDispatcher");
            Instances = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
        return Instances;
    }

    public void Enqueue(Action action)
    {
        lock (ExecutionQueue)
        {
            ExecutionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (ExecutionQueue)
        {
            while (ExecutionQueue.Count > 0)
            {
                ExecutionQueue.Dequeue().Invoke();
            }
        }
    }
}