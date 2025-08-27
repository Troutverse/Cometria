using UnityEngine;

public class YuiController : MonoBehaviour
{
    public float YuiMoveSpeed = 5.0f;
    public float YuiSprintMoveSpeed = 8.0f;
    public bool IsSprinting = false;
    public bool IsLocalPlayer = false;
    public float YuiRotationSpeed = 10.0f;

    public bool YuiReadyToHit = false;
    public GameObject YuiBats;

    public LayerMask GroundLayerMask;
    public float YuiJumpForce = 5.0f;
    public bool YuiIsJumping = false;
    public float GroundCheckDistance = 0.01f;

    private Animator YuiAnimation;

    public PlayerCameraController PlayerCamera;

    private float YuiCurrentMoveSpeed;
    private Rigidbody YuiRigidbody;

    private void Start()
    {
        YuiRigidbody = GetComponent<Rigidbody>();
        YuiAnimation = GetComponent<Animator>();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GroundCheckDistance, GroundLayerMask);
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && !YuiIsJumping)
            {
                YuiIsJumping = true;
                YuiRigidbody.AddForce(Vector3.up * YuiJumpForce, ForceMode.Impulse);
                YuiAnimation?.SetBool("IsJumping", YuiIsJumping);
            }
            if (YuiIsJumping && YuiRigidbody.linearVelocity.y < -0.1f)
            {
                if (IsGrounded())
                {
                    YuiIsJumping = false;
                    YuiAnimation?.SetBool("IsJumping", YuiIsJumping);
                }
            }
            if (Input.GetMouseButton(1) && !YuiIsJumping)
            {
                YuiReadyToHit = true;
                YuiAnimation?.SetBool("Hit", YuiReadyToHit);
                if (Input.GetMouseButtonUp(0))
                {
                    YuiAnimation?.SetTrigger("Attack");
                }
                YuiBats.gameObject.SetActive(true);
            }
            else
            {
                YuiReadyToHit = false;
                YuiAnimation?.SetBool("Hit", YuiReadyToHit);
                YuiBats.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");

            Vector3 CameraForward = PlayerCamera.transform.forward;
            Vector3 CameraRight = PlayerCamera.transform.right;
            CameraForward.y = 0;
            CameraRight.y = 0;
            CameraForward.Normalize();
            CameraRight.Normalize();

            Vector3 YuiMoveDirection = CameraForward * Vertical + CameraRight * Horizontal;

            if (YuiMoveDirection.magnitude > 0.1f)
            {
                IsSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                YuiCurrentMoveSpeed = IsSprinting ? YuiSprintMoveSpeed : YuiMoveSpeed;

                YuiMoveDirection.Normalize();

                Vector3 YuiVelocity = YuiMoveDirection * YuiCurrentMoveSpeed;
                YuiVelocity.y = YuiRigidbody.linearVelocity.y;
                YuiRigidbody.linearVelocity = YuiVelocity;

                Quaternion YuiRotation = Quaternion.LookRotation(YuiMoveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, YuiRotation, YuiRotationSpeed * Time.deltaTime);
            }
            else
            {
                YuiCurrentMoveSpeed = 0f;
                YuiRigidbody.linearVelocity = new Vector3(0, YuiRigidbody.linearVelocity.y, 0);
            }
            if (YuiAnimation != null)
            {
                YuiAnimation.SetFloat("MoveSpeed", YuiCurrentMoveSpeed, 0.1f, Time.deltaTime);
            }
        }
    }
}
