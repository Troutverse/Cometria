using UnityEngine;
using System.Collections;

public class BallThrower : MonoBehaviour
{
    public GameObject ballObject;
    public Transform ballSpawnPosition;
    public Collider targetZoneCollider;

    public float throwtime = 2.0f;

    private Animator _pitcherAnimator;
    private Rigidbody _ballRigidbody;
    private BallController _ballController;

    private int countdownSeconds = 3;

    private void Awake()
    {
        _pitcherAnimator = GetComponent<Animator>();
        if (ballObject != null)
        {
            _ballRigidbody = ballObject.GetComponent<Rigidbody>();
            _ballController = ballObject.GetComponent<BallController>();

            ballObject.SetActive(false);
        }
    }

    public void StartPitching()
    {
        StartCoroutine(PitchingCoroutine());
    }

    private IEnumerator PitchingCoroutine()
    {
        for (int i = countdownSeconds; i > 0; i--)
        {
            ScoreManager.instance.DisplayMessage(i.ToString());
            yield return new WaitForSeconds(1.0f);
        }

        if (_pitcherAnimator != null)
        {
            _pitcherAnimator.SetTrigger("Pitching");
        }
        ScoreManager.instance.DisplayMessage("");
    }

    public void ThrowBall()
    {
        if (_ballRigidbody == null || _ballController == null || targetZoneCollider == null) return;

        Bounds targetBounds = targetZoneCollider.bounds;
        float randomX = Random.Range(targetBounds.min.x, targetBounds.max.x);
        float randomY = Random.Range(targetBounds.min.y, targetBounds.max.y);
        Vector3 randomTargetPosition = new Vector3(randomX, randomY, targetBounds.center.z);

        _ballController.ResetHitBall();
        _ballRigidbody.linearVelocity = Vector3.zero;
        _ballRigidbody.angularVelocity = Vector3.zero;

        ballObject.transform.position = ballSpawnPosition.position;
        ballObject.transform.rotation = ballSpawnPosition.rotation;
        ballObject.SetActive(true);
        ballObject.transform.parent = null;

        Vector3 initialVelocity = CalculateLaunchVelocity(randomTargetPosition);

        _ballRigidbody.linearVelocity = initialVelocity;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
    {
        Vector3 startPoint = ballSpawnPosition.position;
        Vector3 endPoint = targetPosition;

        Vector3 displacement = endPoint - startPoint;

        Vector3 horizontalVelocity = new Vector3(displacement.x, 0, displacement.z) / throwtime;
        float verticalVelocityY = (displacement.y - 0.5f * Physics.gravity.y * throwtime * throwtime) / throwtime;
        Vector3 verticalVelocity = new Vector3(0, verticalVelocityY, 0);

        return horizontalVelocity + verticalVelocity;
    }
}