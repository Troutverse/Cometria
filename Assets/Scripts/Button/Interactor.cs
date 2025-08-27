using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float InteractorDistance = 3.0f;
    public LayerMask InteractionLayer;
    public Transform InteractorPosition;

    private Camera MainCamera;
    private IInteractable CurrentInteractable;

    void Start()
    {
        MainCamera = Camera.main;
    }
    void Update()
    {

        Vector3 InteractorRay = InteractorPosition.transform.position + new Vector3(0f, 1.2f, 0f);
        Ray ray = new Ray(InteractorRay, MainCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.yellow);

        if (Physics.Raycast(ray, out RaycastHit hit, InteractorDistance, InteractionLayer))
        {
    
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable != CurrentInteractable)
                {
                    CurrentInteractable = interactable;
                }
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }

        if (Input.GetKeyDown(KeyCode.F) && CurrentInteractable != null)
        {
            Debug.Log("상호작용 키를 눌렀습니다.");
            CurrentInteractable.Interact(this);
        }
    }

    private void ClearInteraction()
    {
        if (CurrentInteractable != null)
        {
            CurrentInteractable = null;
        }
    }
}