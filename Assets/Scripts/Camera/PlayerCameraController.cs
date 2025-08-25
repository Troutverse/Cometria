using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform Target;
    public float Distance = 6.0f;
    public float Sensitivity = 2.0f;

    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Camera.main != null && Camera.main.gameObject != this.gameObject)
        {
            Camera.main.transform.SetParent(Target);
            Camera.main.transform.localPosition = new Vector3(0, 2.5f, -5);
        }
    }

    void Update()
    {
        YuiController YuiControllers = Target.GetComponent<YuiController>();
        if (YuiControllers != null && !YuiControllers.IsLocalPlayer)
        {
            return;
        }

        RotationX += Input.GetAxis("Mouse X") * Sensitivity;
        RotationY -= Input.GetAxis("Mouse Y") * Sensitivity;

        RotationY = Mathf.Clamp(RotationY, -89f, 70f);

        Quaternion Rotation = Quaternion.Euler(RotationY, RotationX, 0);
        Vector3 NegDistance = new Vector3(0.0f, 3f, -Distance);
        Vector3 Position = Rotation * NegDistance + Target.position;

        transform.position = Position;
        transform.rotation = Rotation;
    }

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }
}