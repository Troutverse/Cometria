using UnityEngine;
using UnityEngine.InputSystem;


public class YuiController : MonoBehaviour
{
    public float YuiCurrentSpeed;
    public float YuiWalkSpeed = 5.0f;
    public float YuiSprintSpeed = 8.0f;

    private Vector2 YuiMoveInput;

    public bool YuiIsGround = false;
    public float YuiJumpPower = 10.0f;

    private Animator YuiAnimator;

    private Rigidbody YuiRigidBody;

    private PlayerMove YuiPlayerMove;
    private PlayerJump YuiPlayerJump;

    public Transform YuiCamera;

    private void OnMove(InputValue value)
    {
        YuiMoveInput = value.Get<Vector2>();
        YuiPlayerMove.SetMoveInput(YuiMoveInput);
    }

    private void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            bool CurretSprint = YuiPlayerMove.GetSprint();
            YuiPlayerMove.SetSprint(!CurretSprint);
        }
        
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed) 
        {
            YuiPlayerJump.StarJump();
        }
    }
    private void Start()
    {
        YuiRigidBody = GetComponent<Rigidbody>();
        YuiAnimator = GetComponent<Animator>();
        YuiPlayerMove = new PlayerMove(YuiRigidBody, YuiWalkSpeed, YuiSprintSpeed);
        YuiPlayerJump = new PlayerJump(YuiRigidBody, YuiAnimator, YuiJumpPower);

    }
    private void FixedUpdate()
    {
        if (YuiRigidBody == null || YuiAnimator == null) return;
        
        Vector3 cameraForward = YuiCamera.forward;
        Vector3 cameraRight = YuiCamera.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        YuiPlayerMove.PlayerMovement(cameraForward, cameraRight);

        float currentSpeed = YuiRigidBody.linearVelocity.magnitude;
        YuiAnimator.SetFloat("MoveSpeed", currentSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        YuiPlayerJump.PlayerCollisonOnGround(collision);
    }
}