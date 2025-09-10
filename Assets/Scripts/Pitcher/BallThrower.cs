using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public GameObject BallPrefab;
    public Transform BallSpawnPosition;

    public Transform ThrowTarget;
    public float ThrowForce = 10f;
    public void ThrowBall()
    {
        GameObject Balls = Instantiate(BallPrefab, BallSpawnPosition.position, BallSpawnPosition.rotation);

        Vector3 BallDirection = (ThrowTarget.position - BallSpawnPosition.position).normalized;

        Rigidbody BallRigidbody = Balls.GetComponent<Rigidbody>();
        BallRigidbody.AddForce(BallDirection * ThrowForce, ForceMode.Impulse);
    }
}