using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;
using TMPro;

public class Client : MonoBehaviour
{
    private TcpClient Socket;
    private NetworkStream Stream;
    private Thread ReceiveThread;
    private bool IsConnected = false;

    public string ServerIp = "127.0.0.1";
    public int Port = 7777;

    public GameObject PlayerPrefab;
    private GameObject MyPlayer;
    private Dictionary<int, GameObject> OtherPlayers = new Dictionary<int, GameObject>();

    private Dictionary<int, Vector3> PlayerTargetPositions = new Dictionary<int, Vector3>();
    public float MovementSmoothing = 10f;

    public TMP_InputField IpInput;
    public Button HostButton;
    public Button ConnectButton;

    private int MyId = -1;

    private void Start()
    {
        UnityMainThreadDispatcher.Instance();

        HostButton.onClick.AddListener(Host);
        ConnectButton.onClick.AddListener(ConnectToServer);
        if (IpInput != null) IpInput.text = ServerIp;
    }

    public void Host()
    {
        GetComponent<Server>().StartServer();
        ConnectToServer();

        MyId = Server.MyConnectionId;
        if (MyPlayer == null)
        {
            MyPlayer = Instantiate(PlayerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
            if (MyPlayer.GetComponent<YuiController>() != null)
            {
                MyPlayer.GetComponent<YuiController>().IsLocalPlayer = true;
            }
            MyPlayer.name = $"Player_{MyId} (Me)";
        }

        HostButton.gameObject.SetActive(false);
        ConnectButton.gameObject.SetActive(false);
        IpInput.gameObject.SetActive(false);
    }

    public void ConnectToServer()
    {
        if (IsConnected) return;

        if (IpInput != null) ServerIp = IpInput.text;

        try
        {
            Socket = new TcpClient();
            Socket.Connect(ServerIp, Port);
            Stream = Socket.GetStream();
            IsConnected = true;

            ReceiveThread = new Thread(new ThreadStart(ReceiveData));
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();

            Debug.Log("Connected to server");

            HostButton.gameObject.SetActive(false);
            ConnectButton.gameObject.SetActive(false);
            IpInput.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"Socket error: {e}");
        }
    }

    private void ReceiveData()
    {
        byte[] Buffer = new byte[4096];
        while (IsConnected)
        {
            try
            {
                int BytesRead = Stream.Read(Buffer, 0, Buffer.Length);
                if (BytesRead <= 0)
                {
                    IsConnected = false;
                    break;
                }
                string ServerMessage = Encoding.UTF8.GetString(Buffer, 0, BytesRead);

                UnityMainThreadDispatcher.Instance().Enqueue(() => HandleServerMessage(ServerMessage));
            }
            catch
            {
                Debug.Log("Disconnected from server.");
                IsConnected = false;
            }
        }
    }

    private void HandleServerMessage(string msg)
    {

        string[] Message = msg.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var message in Message)
        {
            Debug.Log("Server: " + message);
            string[] Parts = message.Split('|');
            string Command = Parts[0];

            switch (Command)
            {
                case "ASSIGN_ID":
                    MyId = int.Parse(Parts[1]);
                    Debug.Log($"My ID is {MyId}");

           
                    if (MyPlayer == null)
                    {
                        MyPlayer = Instantiate(PlayerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
                        if (MyPlayer.GetComponent<YuiController>() != null)
                            MyPlayer.GetComponent<YuiController>().IsLocalPlayer = true;
                        MyPlayer.name = $"Player_{MyId} (Me)";
                    }
                    break;

                case "SPAWN":
                    int NewPlayerId = int.Parse(Parts[1]);
                    if (MyId == NewPlayerId)
                    {
                        
                    }
                    else
                    {
                        if (!OtherPlayers.ContainsKey(NewPlayerId))
                        {
                            GameObject NewPlayer = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
                            if (NewPlayer.GetComponent<YuiController>() != null)
                                NewPlayer.GetComponent<YuiController>().IsLocalPlayer = false;
                            NewPlayer.name = $"Player_{NewPlayerId}";
                            OtherPlayers.Add(NewPlayerId, NewPlayer);
                            PlayerTargetPositions.Add(NewPlayerId, NewPlayer.transform.position);

                            Camera OtherPlayerCamera = NewPlayer.GetComponentInChildren<Camera>();
                            if (OtherPlayerCamera != null)
                            {
                                OtherPlayerCamera.gameObject.SetActive(false);
                            }

                            AudioListener OtherPlayerAudioListener = NewPlayer.GetComponentInChildren<AudioListener>();
                            if (OtherPlayerAudioListener != null)
                            {
                                OtherPlayerAudioListener.enabled = false;
                            }
                        }
                    }
                    break;

                case "MOVE":
                    int MovePlayerId = int.Parse(Parts[1]);
                    if (OtherPlayers.ContainsKey(MovePlayerId))
                    {
                        float x = float.Parse(Parts[2]);
                        float y = float.Parse(Parts[3]);
                        float z = float.Parse(Parts[4]);

                        PlayerTargetPositions[MovePlayerId] = new Vector3(x, y, z);
                    }
                    break;
                case "DISCONNECT":
                    int disconnectPlayerId = int.Parse(Parts[1]);
                    if (OtherPlayers.ContainsKey(disconnectPlayerId))
                    {
                        Destroy(OtherPlayers[disconnectPlayerId]);
                        OtherPlayers.Remove(disconnectPlayerId);
                        PlayerTargetPositions.Remove(disconnectPlayerId);
                    }
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (IsConnected && MyPlayer != null)
        {
            Vector3 pos = MyPlayer.transform.position;
            string msg = $"MOVE|{0}|{pos.x}|{pos.y}|{pos.z}\n";
            SendData(msg);
        }
    }

    private void Update()
    {
        foreach (var item in OtherPlayers)
        {
            int playerId = item.Key;
            GameObject playerObject = item.Value;

            if (PlayerTargetPositions.ContainsKey(playerId))
            {
                playerObject.transform.position = Vector3.Lerp(
                    playerObject.transform.position,
                    PlayerTargetPositions[playerId],
                    Time.deltaTime * MovementSmoothing
                );
            }
        }
    }

    private void SendData(string data)
    {
        if (!IsConnected) return;
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            Stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending data: {e}");
        }
    }

    private void OnApplicationQuit()
    {
        if (Socket != null)
        {
            Socket.Close();
        }
        if (ReceiveThread != null)
        {
            ReceiveThread.Abort();
        }
    }
}