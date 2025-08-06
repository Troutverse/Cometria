using UnityEngine;
using UnityEngine.InputSystem;


public class YuiController : MonoBehaviour
{
    #region
    public float YuiCurrentSpeed;
    public float YuiWalkSpeed = 5.0f;
    public float YuiSprintSpeed = 8.0f;

    private bool YuiSprinting = false;

    private Vector2 YuiMoveInput;

    public bool YuiIsGround = false;

    public float YuiJumpPower = 10.0f;

    private Animator YuiAnimator;

    private Rigidbody YuiRB;

    public Transform YuiCamera;
    #endregion

    // Yui Move Input
    private void OnMove(InputValue value)
    {
        YuiMoveInput = value.Get<Vector2>();
    }

    // Yui Sprint Input
    private void OnSprint(InputValue value)
    {
        if (value.isPressed) YuiSprinting = !YuiSprinting;
    }

    // Yui Jump Input
    private void OnJump(InputValue value)
    {
        if (value.isPressed && YuiIsGround)
        {
            YuiRB.AddForce(Vector3.up * YuiJumpPower, ForceMode.Impulse);
            YuiIsGround = false;
            YuiAnimator.SetBool("IsJumping", false);
        }
    }
    private void Start()
    {
        YuiRB = GetComponent<Rigidbody>();
        YuiAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (YuiRB == null || YuiAnimator == null) return;
        YuiCurrentSpeed = YuiSprinting ? YuiSprintSpeed : YuiWalkSpeed;

        Vector3 cameraForward = YuiCamera.forward;
        Vector3 cameraRight = YuiCamera.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 YuiMoveDirection = cameraForward * YuiMoveInput.y + cameraRight * YuiMoveInput.x;

        // ������ٵ� �ӵ� ���� (Y�� �ӵ� ����)
        YuiRB.linearVelocity = new Vector3(YuiMoveDirection.x * YuiCurrentSpeed, YuiRB.linearVelocity.y, YuiMoveDirection.z * YuiCurrentSpeed);

        float currentSpeed = YuiMoveDirection.magnitude * YuiCurrentSpeed;
        YuiAnimator.SetFloat("MoveSpeed", currentSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� "Ground" �±׸� ������ �ִٸ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            YuiIsGround = true; // ���� ��Ҵٰ� ����
            YuiAnimator.SetBool("IsJumping", true);
        }
    }
}