using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform Target;
    public float Distance = 6.0f;
    public float Sensitivity = 2.0f;

    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    public Vector3 ReadyToHitCameraPosition = new Vector3(3f, 0.8f, 1.2f);
    public Vector3 LookAtTargetOffset = new Vector3(0f, 1.5f, 1f);
    private float ReadyToHitCameraTransitionSpeed = 8.0f;

    public GameObject ReadyToHitCanvas;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Camera.main != null && Camera.main.gameObject != this.gameObject)
        {
            Camera.main.transform.SetParent(Target);
            Camera.main.transform.localPosition = new Vector3(0, 2f, -5);
        }
    }

    private void LateUpdate()
    {
        YuiController YuiControllers = Target.GetComponent<YuiController>();
        if (YuiControllers != null && !YuiControllers.IsLocalPlayer) return;
        
        if (!YuiControllers.YuiReadyToHit) 
        {
            ReadyToHitCanvas?.SetActive(false);
            RotationX += Input.GetAxis("Mouse X") * Sensitivity;
            RotationY -= Input.GetAxis("Mouse Y") * Sensitivity;

            RotationY = Mathf.Clamp(RotationY, -70f, 60f);

            Quaternion Rotation = Quaternion.Euler(RotationY, RotationX, 0);
            Vector3 NegDistance = new Vector3(0.0f, 3f, -Distance);
            Vector3 Position = Rotation * NegDistance + Target.position;

            transform.position = Position;
            transform.rotation = Rotation; 
        }
        else
        {
            ReadyToHitCanvas?.SetActive(true);
            Vector3 TargetPosition = Target.position + Target.right * ReadyToHitCameraPosition.x + Target.up * ReadyToHitCameraPosition.y + Target.forward * ReadyToHitCameraPosition.z;
            Vector3 LookAtPoint = Target.position + Target.right * LookAtTargetOffset.x + Target.up * LookAtTargetOffset.y + Target.forward * LookAtTargetOffset.z;
            
            Quaternion TargetRotation = Quaternion.LookRotation(LookAtPoint - TargetPosition);

            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * ReadyToHitCameraTransitionSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime * ReadyToHitCameraTransitionSpeed);
        }
    }
}