using UnityEngine;

public class PlayerMove
{
    private readonly Rigidbody PlayerRigidbody;
    private readonly float PlayerWalkSpeed;
    private readonly float PlayerSprintSpeed;

    private Vector2 PlayerMoveInput;
    private bool IsSprint = false;

    public PlayerMove(Rigidbody Rb, float WalkSpeed, float SprintSpeed)
    {
        PlayerRigidbody = Rb;
        PlayerWalkSpeed = WalkSpeed;
        PlayerSprintSpeed = SprintSpeed;
    }

    public void SetMoveInput(Vector2 Input)
    {
        PlayerMoveInput = Input;
    }

    public void SetSprint(bool Sprint)
    {
        IsSprint = Sprint;
    }

    public bool GetSprint()
    {
        return IsSprint;
    }

    public void PlayerMovement(Vector3 forward, Vector3 right)
    {
        float CurrentSpeed = IsSprint ? PlayerSprintSpeed : PlayerWalkSpeed;

        Vector3 MoveDirection = forward * PlayerMoveInput.y + right * PlayerMoveInput.x;

        PlayerRigidbody.linearVelocity = new Vector3(MoveDirection.x * CurrentSpeed, PlayerRigidbody.linearVelocity.y, MoveDirection.z * CurrentSpeed);
    }
}
