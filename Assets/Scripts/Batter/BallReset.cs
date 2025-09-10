using UnityEngine;

public class BallReset : MonoBehaviour
{
    public GameObject BallPrefab;

    private Vector3 BallPosition;
    private Rigidbody BallRigidbody;

    void Start()
    {
        if (BallPrefab != null)
        {
            BallPosition = BallPrefab.transform.position;
            BallRigidbody = BallPrefab.gameObject.GetComponent<Rigidbody>();
        }
    }

    public void ResetBallPosition()
    {
        BallPrefab.transform.position = BallPosition;
        BallRigidbody.linearVelocity = Vector3.zero;
        BallRigidbody.angularVelocity = Vector3.zero;
    }
}