using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InteractionUI interactionUI;

    [Header("View Settings")]
    [SerializeField] private float maxViewAngle = 35f;

    private IInteractable currentInteractable;
    private Transform currentTransform;

    private void Update()
    {
        if (currentInteractable == null || currentTransform == null)
        {
            interactionUI.Hide();
            Clear();
            return;
        }

        if (IsLookingAtInteractable())
        {
            interactionUI.Show(
                currentInteractable.GetInteractionType(),
                currentInteractable.GetInteractionDescription()
            );
        }
        else
        {
            interactionUI.Hide();
        }
    }


    public void OnInteract()
    {
        if (currentInteractable == null)
            return;

        if (IsLookingAtInteractable())
        {
            currentInteractable.Interact();
            Clear();
        }
    }

    private bool IsLookingAtInteractable()
    {
        if (currentTransform == null)
            return false;

        Vector3 toTarget = currentTransform.position - playerCamera.transform.position;
        toTarget.y = 0f;
        toTarget.Normalize();

        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        return Vector3.Angle(forward, toTarget) <= maxViewAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            currentTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == currentTransform)
        {
            Clear();
        }
    }

    private void Clear()
    {
        currentInteractable = null;
        currentTransform = null;
        interactionUI.Hide();
    }
}
