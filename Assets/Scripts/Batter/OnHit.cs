using UnityEngine;

public class OnHit : MonoBehaviour
{
    public float HitForce = 80f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PracticeBall") || collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody BallRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Debug.Log(collision.gameObject.name + " gg ");
            if (BallRigidbody != null)
            {
                Vector3 HitDirection = transform.forward;
                BallRigidbody.AddForce(HitDirection * HitForce, ForceMode.Impulse);
            }
        }
    }
}