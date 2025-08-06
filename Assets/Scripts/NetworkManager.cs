using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private CometriaServer server;
    public int serverPort = 9000;
    void Start()
    {
        server = new CometriaServer();    
        server.StartServer(serverPort);
    }

    void OnDestroy()
    {
            
    }
}
