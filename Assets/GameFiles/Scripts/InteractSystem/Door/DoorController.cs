using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;

    private bool isOpen = false;

    public InteractionType GetInteractionType() => InteractionType.Open;

    public string GetInteractionDescription() => "открыть дверь";

    public void Interact()
    {
        if (isOpen)
            return;

        isOpen = true;
        animator.SetBool("Open", true);

        this.enabled = false;
    }
}
