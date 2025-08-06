using UnityEngine;

public class ClientManager : MonoBehaviour
{
    private CometriaClient client;
    public string host = "127.0.0.1";
    public int port = 9000;

    void Start()
    {
        client = new CometriaClient();

        ConnectToServer();
    }

    public void ConnectToServer()
    {
        client.ConnectToServer(host, port);
    }
    void OnDestroy()
    {
        
    }
    public void SendTestMessage()
    {
        if (client != null)
        {
            client.SendMessage("Hello from Client!");
        }
    }
}
