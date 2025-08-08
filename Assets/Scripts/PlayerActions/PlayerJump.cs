using UnityEngine;

public class PlayerJump
{
    private readonly Rigidbody PlayerRigidbody;
    private readonly Animator PlayerAnimator;
    private readonly float PlayerJumpPower;

    private bool IsGrounded = false;

    public PlayerJump(Rigidbody Rb,Animator animator ,float JumpPower)
    {
        PlayerRigidbody = Rb;
        PlayerAnimator  = animator;
        PlayerJumpPower = JumpPower;
    }

    public void StarJump()
    {
        if (IsGrounded)
        {
            PlayerRigidbody.AddForce(Vector3.up * PlayerJumpPower, ForceMode.Impulse);
            IsGrounded = false;
            PlayerAnimator.SetBool("IsJumping", IsGrounded);
        }
    }
    public void PlayerCollisonOnGround(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            PlayerAnimator.SetBool("IsJumping", IsGrounded);
        }
    }
    public void SetGrounded(bool isGrounded)
    {
        IsGrounded = isGrounded;
        if (IsGrounded)
        {
            PlayerAnimator.SetBool("IsJumping", IsGrounded);
        }
    }
}
