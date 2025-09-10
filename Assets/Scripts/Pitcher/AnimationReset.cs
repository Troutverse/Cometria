using UnityEngine;

public class AnimationReset : MonoBehaviour
{
    private Vector3 AnimatorPosition;
    private Quaternion AnimatorRotation;

    private bool NeedReset = false;

    void Awake()
    {
        AnimatorPosition = transform.position;
        AnimatorRotation = transform.rotation;
    }

    public void ResetPositionAndRataion()
    {
        NeedReset = true;
    }

    public void LateUpdate()
    {
        if (NeedReset)
        {
            transform.position = AnimatorPosition;
            transform.rotation = AnimatorRotation;
            NeedReset = false;
        }
    }
}
