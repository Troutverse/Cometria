using UnityEngine;

public class YuiController : MonoBehaviour
{
    public float YuiMoveSpeed = 5.0f;
    public float YuiSprintMoveSpeed = 8.0f;
    public bool IsSprinting = false;
    public bool IsLocalPlayer = false;
    public float YuiRotationSpeed = 10.0f;

    private Animator YuiAnimation;

    public PlayerCameraController PlayerCamera;

    private Rigidbody YuiRigidbody;

    private void Start()
    {
        YuiRigidbody = GetComponent<Rigidbody>();
        YuiAnimation = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");

            Vector3 YuiMovement = new Vector3(Horizontal, 0, Vertical).normalized;

            float CurrentYuiMoveSpeed = IsSprinting ? YuiSprintMoveSpeed : YuiMoveSpeed;

            if (YuiAnimation != null)
            {
                YuiAnimation.SetFloat("MoveSpeed", CurrentYuiMoveSpeed);
            }

            Vector3 CameraForward = PlayerCamera.transform.forward;
            Vector3 CameraRight = PlayerCamera.transform.right;
            CameraForward.y = 0;
            CameraRight.y = 0;
            CameraForward.Normalize();
            CameraRight.Normalize();

            Vector3 YuiMoveDirection = CameraForward * Vertical + CameraRight * Horizontal;
            YuiMoveDirection.Normalize();

            Vector3 YuiVelocity = YuiMoveDirection * YuiMoveSpeed;

            YuiVelocity.y = YuiRigidbody.linearVelocity.y;

            YuiRigidbody.linearVelocity = YuiVelocity;

            if (YuiMoveDirection != Vector3.zero)
            {
                Quaternion YuiRotation = Quaternion.LookRotation(YuiMoveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, YuiRotation, YuiRotationSpeed * Time.deltaTime);
            }
        }
    }
}
