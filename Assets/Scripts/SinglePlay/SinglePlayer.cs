using UnityEngine;
using UnityEngine.UI;

public class SinglePlayer : MonoBehaviour
{
    public Button SinglePlayButton;

    public GameObject PlayerPrefab;
    private GameObject MyPlayer;
    
    public Camera LobbyCamera;
    public GameObject Cometria;

    private void Start()
    {
        SinglePlayButton.onClick.AddListener(Single);
    }
    public void Single()
    {
        this.gameObject.SetActive(false);
        LobbyCamera.gameObject.SetActive(false);
        Cometria.SetActive(false);
        if (PlayerPrefab != null)
        {
            MyPlayer = Instantiate(PlayerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
            if (MyPlayer.GetComponent<YuiController>() != null)
            {
                MyPlayer.GetComponent<YuiController>().IsLocalPlayer = true;
            }
        }
    }
    void Update()
    {
        
    }
}
