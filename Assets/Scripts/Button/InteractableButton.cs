using UnityEngine;
using UnityEngine.Events; 

public class InteractableButton : MonoBehaviour, IInteractable
{
   
    public UnityEvent OnInteract;

    public bool Interact(Interactor interactor)
    {
        OnInteract.Invoke();
        Debug.Log("버튼과 상호작용했습니다!");
        return true;
    }
}