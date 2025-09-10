using UnityEngine;
using UnityEngine.UI;

public class SinglePlayer : MonoBehaviour
{
    public Button SinglePlayButton;

    public GameObject PlayerPrefab;
    
    public GameObject LobbyCamera;
    public GameObject Cometria;

    public void Single()
    {
        this.gameObject.SetActive(false);
        LobbyCamera.gameObject.SetActive(false);
        Cometria.SetActive(false);

        if (PlayerPrefab != null)
        {
            Vector3 playerSpawnPoint = new Vector3(1.2f, 0.2f, 0);
            Quaternion playerSpawnQuaternion = Quaternion.identity;

            Instantiate(PlayerPrefab, playerSpawnPoint, playerSpawnQuaternion);
        }
    }
}
