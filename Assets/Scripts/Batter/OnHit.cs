using System.Collections;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public float hitForce = 30f;
    public Transform batHitPoint;
    public float upWardAngle = 20f;
    private bool canHit = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && canHit)
        {
            Rigidbody ballRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                canHit = false;

                Vector3 hitDirection = (batHitPoint.forward + Vector3.up * Mathf.Tan(upWardAngle * Mathf.Deg2Rad)).normalized;

                ballRigidbody.linearVelocity = Vector3.zero;
                ballRigidbody.angularVelocity = Vector3.zero;

                Vector3 targetVelocity = hitDirection * hitForce;

                ballRigidbody.linearVelocity = targetVelocity;

                if (DefenseManager.instance != null) DefenseManager.instance.BatterHit(ballRigidbody);
                
                // Alert Ball hit To BallController
                BallController ballController = other.gameObject.GetComponent<BallController>();
                if (ballController != null) ballController.HitByBat();
                
                StartCoroutine(ResetCanHit());
            }
        }
    }

    private IEnumerator ResetCanHit()
    {
        yield return new WaitForSeconds(1f);
        canHit = true;
    }
}