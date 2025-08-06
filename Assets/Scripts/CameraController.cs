using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;

    public bool useHeadbob = true;
    public float bobX = 15.0f;
    public float bobXs = 0.01f;
    public float bobY = 15.0f;
    public float bobYs = 0.05f;
    private Vector3 _startPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        if (useHeadbob && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            float bobYValue = Mathf.Sin(Time.time * bobY) * bobYs;
            float bobXValue = Mathf.Sin(Time.time * bobX / 2f) * bobXs;

            transform.localPosition = _startPosition + new Vector3(bobXValue, bobYValue, 0);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition, Time.deltaTime * 10f);
        }
    }
}